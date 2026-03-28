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
    [OffsetAfter(nameof(mouseEventsSize))] readonly int keyOffset;
    [OffsetAfter(nameof(keyEventsSize))] readonly int textOffset;
    [OffsetAfter(nameof(textEventsSize))] readonly int resizeOffset;
    [OffsetAfter(nameof(resizeEventsSize))] readonly int loadEventsOffset;
    [OffsetAfter(nameof(loadEventsSize))] readonly int setupHTML_OR_URL_Offset;
    [OffsetAfter(nameof(setUp_HTML_OR_URL_Size))] readonly int executeJSOffset;
    [OffsetAfter(nameof(executeJSSize))] readonly int messageConsoleOffset;
    [OffsetAfter(nameof(messageConsoleSize))] readonly int messageEmittedOffset;
    [OffsetAfter(nameof(messageEmittedSize))] readonly int postMessageOffset;
    [OffsetAfter(nameof(postMessageSize))] readonly int baseEventsOffset;
    [OffsetAfter(nameof(baseEventSize))] readonly int frameOffset;
    #endregion

    #region SIZES
    [FieldSize] readonly int mouseEventsSize;
    [FieldSize] readonly int keyEventsSize;
    [FieldSize] readonly int textEventsSize;
    [FieldSize] readonly int resizeEventsSize;
    [FieldSize] readonly int loadEventsSize;
    [FieldSize] readonly int setUp_HTML_OR_URL_Size;
    [FieldSize] readonly int executeJSSize;
    [FieldSize] readonly int messageConsoleSize;
    [FieldSize] readonly int messageEmittedSize;
    [FieldSize] readonly int postMessageSize;
    [FieldSize] readonly int baseEventSize;
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
        mouseEventsSize = sizeof(MouseEvent) * ChunksData.MOUSE_EVENT_CHUNKS;
        keyEventsSize = sizeof(KeyEvent) * ChunksData.KEY_EVENT_CHUNKS;
        textEventsSize = sizeof(InputTextEvent) * ChunksData.TEXT_EVENT_CHUNKS;
        resizeEventsSize = sizeof(ResizeEvent) * ChunksData.RESIZE_EVENT_CHUNKS;
        loadEventsSize = sizeof(LoadEventId) * ChunksData.LOAD_EVENT_CHUNKS;
        setUp_HTML_OR_URL_Size = sizeof(LoadEventId) * ChunksData.SETUP_HTML_OR_URL;
        executeJSSize = sizeof(LoadEventIdWithCallback) * ChunksData.EXECUTE_JS_CHUNKS;
        messageConsoleSize = sizeof(LoadEventId) * ChunksData.MESSAGE_CONSOLE_CHUNKS;
        messageEmittedSize = sizeof(LoadEventId) * ChunksData.MESSAGE_EMITTED_CHUNKS;
        postMessageSize = sizeof(LoadEventId) * ChunksData.POST_MESSAGE_CHUNKS;
        baseEventSize = sizeof(BaseEvent) * ChunksData.BASE_EVENT_CHUNKS;


        int total =
            Marshal.SizeOf<ViewHeader>() +
            ComputeFildSizes() +
            bufferSize * BUFS;


        mmf = CreateMMF.CreateMemoryMappedFile(BASE_FILE_NAME.VIEW + id.ToString(), total);

        accessor = mmf.CreateViewAccessor();

        byte* ptr = (byte*)accessor.SafeMemoryMappedViewHandle.DangerousGetHandle();
        basePtr = ptr;
        header = (ViewHeader*)ptr;
        headerSize = Marshal.SizeOf<ViewHeader>();

        // mouseOffset = headerSize;
        // keyOffset = mouseOffset + mouseEventsSize;
        // textOffset = keyOffset + keyEventsSize;
        // resizeOffset = textOffset + textEventsSize;
        // loadEventsOffset = resizeOffset + resizeEventsSize;
        // setupHTML_OR_URL_Offset = loadEventsOffset + loadEventsSize;
        // executeJSOffset = setupHTML_OR_URL_Offset + setUp_HTML_OR_URL_Size;
        // messageConsoleOffset = executeJSOffset + executeJSSize;
        // messageEmittedOffset = messageConsoleOffset + messageConsoleSize;
        // baseEventsOffset = messageEmittedOffset + messageEmittedSize;
        // frameOffset = baseEventsOffset + baseEventSize;

        ComputeOffsets();

        header->magic = MAGIC;

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

        ReadJsExecuteEvent();

        ReadPostMessageEvent();

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

    void ReadPostMessageEvent()
    {
        while (header->postMessageEventRead < header->postMessageEventWrite)
        {
            int index = (int)(header->postMessageEventRead % ChunksData.POST_MESSAGE_CHUNKS);
            LoadEventId* ev = (LoadEventId*)(basePtr + postMessageOffset + index * sizeof(LoadEventId));

            (var eventType, var headerObject, var stringList) = StringManager.ReadString(ev->id);
            View.PostMessage(stringList[0]);

            header->postMessageEventRead++;
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

    void WriteMessageConsole(uint id)
    {
        int index = (int)(header->messageConsoleEventWrite % ChunksData.MESSAGE_CONSOLE_CHUNKS);

        LoadEventId* ev = (LoadEventId*)(basePtr + messageConsoleOffset + index * sizeof(LoadEventId));

        ev->id = id;

        Thread.MemoryBarrier();

        header->messageConsoleEventWrite++;
    }

    void WriteMessageEmitted(uint id)
    {
        int index = (int)(header->messageEmittedEventWrite % ChunksData.MESSAGE_CONSOLE_CHUNKS);

        LoadEventId* ev = (LoadEventId*)(basePtr + messageEmittedOffset + index * sizeof(LoadEventId));
        Console.WriteLine("Message emitted: " + id);
        ev->id = id;

        Thread.MemoryBarrier();

        header->messageEmittedEventWrite++;
    }

    #endregion

    public void Dispose()
    {
        Console.WriteLine("cleaning up view... " + ID);

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