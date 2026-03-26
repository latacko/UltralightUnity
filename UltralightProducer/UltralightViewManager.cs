using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using UltralightSharedClasses.Classes;
using UltralightSharedClasses.Enums;
using UltralightSharedClasses.StringHeaders;
using UltralightSharedClasses.Structs;
using UltralightUnity;
using UltralightUnity.HandlesJs;
using UltralightUnity.NativeJs;

public unsafe partial class UltralightViewManager : IDisposable
{
    readonly uint ID;
    readonly string PATH;
    const uint MAGIC = 0x6C617461;

    public uint WIDTH;
    public uint HEIGHT;

    const int BUFS = 3;

    int bufferSize;
    readonly MemoryMappedFile mmf;
    readonly MemoryMappedViewAccessor accessor;

    #region MEMORY INFO
    readonly ViewHeader* header;
    readonly byte* basePtr;
    readonly int headerSize;
    readonly int mouseOffset;
    readonly int keyOffset;
    readonly int textOffset;

    readonly int resizeOffset;
    readonly int loadEventsOffset;
    readonly int setupHTML_OR_URL_Offset;
    readonly int executeJSOffset;
    readonly int baseEventsOffset;

    readonly int frameOffset;
    #endregion

    public readonly ULView View;

    public UltralightViewManager(uint id, uint width, uint height, bool isTransparent)
    {
        this.ID = id;

        this.WIDTH = width;
        this.HEIGHT = height;

        using var _viewConfig = new ULViewConfig();
        _viewConfig.SetIsAccelerated(false);
        _viewConfig.SetIsTransparent(isTransparent);
        View = Program.Renderer.CreateView(width, height, _viewConfig);
        RegisterEvents();


        this.PATH = BASE_FILE_NAME.BASE_PATH + BASE_FILE_NAME.VIEW + id.ToString();

        bufferSize = (int)WIDTH * (int)HEIGHT * 4;
        int mouseEventsSize = sizeof(MouseEvent) * ChunksData.MOUSE_EVENT_CHUNKS;
        int keyEventsSize = sizeof(KeyEvent) * ChunksData.KEY_EVENT_CHUNKS;
        int textEventsSize = sizeof(InputTextEvent) * ChunksData.TEXT_EVENT_CHUNKS;
        int resizeEventsSize = sizeof(ResizeEvent) * ChunksData.RESIZE_EVENT_CHUNKS;
        int loadEventsSize = sizeof(LoadEventId) * ChunksData.LOAD_EVENT_CHUNKS;
        int setUp_HTML_OR_URL_Size = sizeof(LoadEventId) * ChunksData.SETUP_HTML_OR_URL;
        int executeJSSize = sizeof(LoadEventIdWithCallback) * ChunksData.EXECUTE_JS_CHUNKS;
        int baseEventSize = sizeof(BaseEvent) * ChunksData.BASE_EVENT_CHUNKS;

        int total =
            Marshal.SizeOf<ViewHeader>() +
            mouseEventsSize +
            keyEventsSize +
            textEventsSize +
            resizeEventsSize +
            loadEventsSize +
            setUp_HTML_OR_URL_Size +
            executeJSSize +
            baseEventSize +
            bufferSize * BUFS;

        mmf = CreateMMF.CreateMemoryMappedFile(BASE_FILE_NAME.VIEW + id.ToString(), total);

        accessor = mmf.CreateViewAccessor();

        byte* ptr = (byte*)accessor.SafeMemoryMappedViewHandle.DangerousGetHandle();
        basePtr = ptr;
        header = (ViewHeader*)ptr;
        headerSize = Marshal.SizeOf<ViewHeader>();

        mouseOffset = headerSize;
        keyOffset = mouseOffset + mouseEventsSize;
        textOffset = keyOffset + keyEventsSize;
        resizeOffset = textOffset + textEventsSize;
        loadEventsOffset = resizeOffset + resizeEventsSize;
        setupHTML_OR_URL_Offset = loadEventsOffset + loadEventsSize;
        executeJSOffset = setupHTML_OR_URL_Offset + setUp_HTML_OR_URL_Size;
        baseEventsOffset = executeJSOffset + executeJSSize;
        frameOffset = baseEventsOffset + baseEventSize;

        header->magic = MAGIC;

        /* OFFSETS */
        header->mouseOffset = (uint)mouseOffset;
        header->keyOffset = (uint)keyOffset;
        header->textOffset = (uint)textOffset;
        header->resizeOffset = (uint)resizeOffset;
        header->loadEventsOffset = (uint)loadEventsOffset;
        header->setUpHTML_OR_URL_Offset = (uint)setupHTML_OR_URL_Offset;
        header->executeJSOffset = (uint)executeJSOffset;
        header->baseEventsOffset = (uint)baseEventsOffset;
        header->frameOffset = (uint)frameOffset;

        /* FRAMES */
        header->width = WIDTH;
        header->height = HEIGHT;
        header->bufferSize = (uint)bufferSize;
    }

    public void BeforeUpdate()
    {
        ReadMouseEvents();

        ReadKeyEvents();

        ReadTextEvent();

        ReadResizeEvent();

        ReadSetUpEvents();
        ReadOpenInspector();
    }

    public void Update()
    {
        var surface = View.GetSurface();
        if (!surface.DirtyBounds.IsEmpty)
        {
            // choose next buffer
            uint next = (header->writeIndex + 1) % BUFS;
            byte* buffer = basePtr + frameOffset + (header->bufferSize * (int)next);
            WriteBitmapToBuffer(surface.GetBitmap(), buffer);

            // memory barrier
            Thread.MemoryBarrier();

            header->writeIndex = next;

            surface.ClearDirty();
        }
    }

    #region Read

    void ReadMouseEvents()
    {
        while (header->buttonEventRead < header->buttonEventWrite)
        {
            int index = (int)(header->buttonEventRead % ChunksData.MOUSE_EVENT_CHUNKS);
            MouseEvent* ev = (MouseEvent*)(basePtr + mouseOffset + index * sizeof(MouseEvent));

            MouseManager.ReadEvent(ev, View);

            header->buttonEventRead++;
        }
    }

    void ReadKeyEvents()
    {
        while (header->keyEventRead < header->keyEventWrite)
        {
            int index = (int)(header->keyEventRead % ChunksData.KEY_EVENT_CHUNKS);
            KeyEvent* ev = (KeyEvent*)(basePtr + keyOffset + index * sizeof(KeyEvent));

            KeysManager.ReadEvent(ev, View);

            header->keyEventRead++;
        }
    }

    void ReadTextEvent()
    {
        while (header->inputTextEventRead < header->inputTextEventWrite)
        {
            int index = (int)(header->inputTextEventRead % ChunksData.TEXT_EVENT_CHUNKS);
            InputTextEvent* ev = (InputTextEvent*)(basePtr + textOffset + index * sizeof(InputTextEvent));

            KeysManager.InputText(ev, View);

            header->inputTextEventRead++;
        }
    }

    void ReadResizeEvent()
    {
        while (header->resizeEventRead < header->resizeEventWrite)
        {
            int index = (int)(header->resizeEventRead % ChunksData.RESIZE_EVENT_CHUNKS);
            ResizeEvent* ev = (ResizeEvent*)(basePtr + resizeOffset + index * sizeof(ResizeEvent));

            View.Resize(ev->width, ev->height);

            header->width = ev->width;
            header->height = ev->height;

            bufferSize = (int)WIDTH * (int)HEIGHT * 4;
            header->bufferSize = (uint)bufferSize;


            header->resizeCounter++;
            header->resizeEventRead++;
        }
    }

    void WriteBitmapToBuffer(ULBitmap bmp, byte* buffer)
    {
        var pixels = bmp.LockPixels();
        try
        {
            Buffer.MemoryCopy((void*)pixels, buffer, (long)bmp.Size, (long)bmp.Size);
        }
        finally
        {
            bmp.UnlockPixels();
        }
    }

    void ReadSetUpEvents()
    {
        Thread.MemoryBarrier();
        while (header->setUpEventRead < header->setUpEventWrite)
        {
            int index = (int)(header->setUpEventRead % ChunksData.SETUP_HTML_OR_URL);
            LoadEventId* ev = (LoadEventId*)(basePtr + setupHTML_OR_URL_Offset + index * sizeof(LoadEventId));

            (var eventType, var headerObject, var stringList) = StringManager.ReadString(ev->id);

            if (eventType == EventType.Set_HTML_OR_URL && headerObject != null)
            {
                SetUpHTMLORURLHeader _header = (SetUpHTMLORURLHeader)headerObject;
                if (_header.type == SetUpType.url)
                    View.LoadURL(stringList[0]);
                else
                {
                    View.LoadHTML(stringList[0]);
                }
            }

            header->setUpEventRead++;
        }
    }

    void ReadJsExecuteEvent()
    {
        Thread.MemoryBarrier();
        while (header->executeJSEventRead < header->executeJSEventWrite)
        {
            int index = (int)(header->executeJSEventRead % ChunksData.EXECUTE_JS_CHUNKS);
            LoadEventIdWithCallback* ev = (LoadEventIdWithCallback*)(basePtr + executeJSOffset + index * sizeof(LoadEventIdWithCallback));

            (var eventType, var headerObject, var stringList) = StringManager.ReadString(ev->id);

            string _result = View.EvaluateScript(stringList[0], out string exception);

            if (_result.Length > 0 && exception.Length > 0)
                ev->idCallback = StringManager.GenerateMMF<EmptyHeader>(EventType.EvaluateScript, null, _result, exception);
            else
                ev->idCallback = 1;

            header->executeJSEventRead++;
        }
    }

    void ReadOpenInspector()
    {
        if (header->openInspector == 1)
        {
            header->openInspector = 0;
            View.OpenInspector();
            Console.WriteLine("Requested inspector");
        }
    }

    #endregion

    #region Write
    void WriteBaseEvent(BaseEventType type)
    {
        int index = (int)(header->baseEventsWrite % ChunksData.KEY_EVENT_CHUNKS);

        BaseEvent* ev = (BaseEvent*)(basePtr + baseEventsOffset + index * sizeof(BaseEvent));

        ev->type = type;

        Thread.MemoryBarrier();

        header->baseEventsWrite++;
    }

    void WriteLoadAdvancedEvent(uint id)
    {
        int index = (int)(header->loadEventsWrite % ChunksData.LOAD_EVENT_CHUNKS);

        LoadEventId* ev = (LoadEventId*)(basePtr + loadEventsOffset + index * sizeof(LoadEventId));

        ev->id = id;

        Thread.MemoryBarrier();

        header->loadEventsWrite++;
    }
    
    #endregion
    

    void Test()
    {
        JSContextRef _context = View.LockJSContext();
        var _name = JSString.CreateWithUTF8CString("OnButtonClick");
        JSObjectRef func = JSObject.JSObjectMakeFunctionWithCallback(_context, _name, OnButtonClick);
    }

    private static JSValueRef OnButtonClick(JSContextRef ctx, JSObjectRef function, JSObjectRef thisObject, nint argumentCount, JSValueRef[] arguments, out JSValueRef exception)
    {
        exception = JSValueRef.Null;
        using JSString _script = JSString.CreateWithUTF8CString("document.getElementById('result').innerText = 'Ultralight rocks!'");
        JSBase.JSEvaluateScript(ctx, _script, JSObjectRef.Null, null, 0, out var script_exception);

        return NativeJSValueRef.JSValueMakeNull(ctx);
    }

    public void Dispose()
    {

        Console.WriteLine("cleaning up...");

        if (inspectorView != null)
        {
            inspectorView.Dispose();
        }

        try
        {
            View?.Dispose();
            accessor?.Dispose();
            mmf?.Dispose();
        }
        catch { }

        try
        {
            if (File.Exists(PATH))
                File.Delete(PATH);
        }
        catch (Exception ex)
        {
            Console.WriteLine("cleanup failed: " + ex.Message);
        }

        GC.SuppressFinalize(this);
    }
}