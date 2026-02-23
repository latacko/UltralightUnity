using System.Drawing;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using UltralightUnity;

public unsafe class UltralightViewManager : IDisposable
{
    const string BASE_FILE_NAME = "ultralight_view_";
    readonly uint ID;
    readonly string PATH;
    const uint MAGIC = 0x6C617461;

    public uint WIDTH;
    public uint HEIGHT;

    const int BUFS = 3;

    readonly int bufferSize;
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
    readonly int loadEventOffset;
    readonly int frameOffset;
    #endregion

    #region buffers
    string toLoad = "";
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

        this.PATH = UltralightManager.BASE_PATH + BASE_FILE_NAME + id.ToString();

        bufferSize = (int)WIDTH * (int)HEIGHT * 4;
        int mouseEventsSize = sizeof(MouseEvent) * ChunksData.MOUSE_EVENT_CHUNKS;
        int keyEventsSize = sizeof(KeyEvent) * ChunksData.KEY_EVENT_CHUNKS;
        int textEventsSize = sizeof(InputTextEvent) * ChunksData.TEXT_EVENT_CHUNKS;
        int resizeEventsSize = sizeof(ResizeEvent) * ChunksData.RESIZE_EVENT_CHUNKS;
        int loadEventsSize = sizeof(LoadEvent) * ChunksData.LOAD_EVENT_CHUNKS;

        int total =
            Marshal.SizeOf<ViewHeader>() +
            mouseEventsSize +
            keyEventsSize +
            textEventsSize +
            resizeEventsSize +
            loadEventsSize +
            bufferSize * BUFS;

        var fs = new FileStream(PATH, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        fs.SetLength(total);

        mmf = MemoryMappedFile.CreateFromFile(fs, null, total, MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, false);

        accessor = mmf.CreateViewAccessor();

        byte* ptr = (byte*)accessor.SafeMemoryMappedViewHandle.DangerousGetHandle();
        basePtr = ptr;
        header = (ViewHeader*)ptr;
        headerSize = Marshal.SizeOf<ViewHeader>();

        mouseOffset = headerSize;
        keyOffset = mouseOffset + mouseEventsSize;
        textOffset = keyOffset + keyEventsSize;
        resizeOffset = textOffset + textEventsSize;
        loadEventOffset = resizeOffset + resizeEventsSize;
        frameOffset = loadEventOffset + loadEventsSize;

        header->magic = MAGIC;

        /* OFFSETS */
        header->mouseOffset = (uint)mouseOffset;
        header->keyOffset = (uint)keyOffset;
        header->textOffset = (uint)textOffset;
        header->resizeOffset = (uint)resizeOffset;
        header->loadEventOffset = (uint)loadEventOffset;
        header->frameOffset = (uint)frameOffset;

        /* FRAMES */
        header->width = WIDTH;
        header->height = HEIGHT;
        header->bufferSize = (uint)bufferSize;
        header->writeIndex = 0;
        header->resizeCounter=0;


        /* RESIZE */
        header->resizeEventWrite = 0;
        header->resizeEventRead = 0;


        /* MOUSE */
        header->buttonEventRead = 0;
        header->buttonEventWrite = 0;

        /* INPUT TEXT */
        header->inputTextEventWrite = 0;
        header->inputTextEventRead = 0;

        /* KEY EVENT */
        header->keyEventWrite = 0;
        header->keyEventRead = 0;

        /* Load Event */
        header->LoadEventWrite = 0;
        header->LoadViewEventRead = 0;
    }

    public void BeforeUpdate()
    {
        ReadMouseEvents();

        ReadKeyEvents();

        ReadTextEvent();

        ReadResizeEvent();

        ReadLoadEvents();
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

    
    void ReadLoadEvents()
    {
        Thread.MemoryBarrier();
        while (header->LoadViewEventRead < header->LoadEventWrite)
        {
            int index = (int)(header->LoadViewEventRead % ChunksData.LOAD_EVENT_CHUNKS);
            LoadEvent* ev = (LoadEvent*)(basePtr + loadEventOffset + index * sizeof(LoadEvent));

            var _chunk = ReadStringFromBuffer(ev->length, ev->data);
            toLoad += _chunk;
            if (ev->thereIsNextChunk == 0)
            {
                if (ev->type == 0)
                    View.LoadURL(toLoad);
                else
                    View.LoadHTML(toLoad);
                toLoad = "";
            }

            header->LoadViewEventRead++;
        }
    }

    public static string ReadStringFromBuffer(uint length, byte* dataPtr)
    {
        if (length > 256)
            length = 256;

        return Encoding.UTF8.GetString(dataPtr, (int)length);
    }

    public static List<byte[]> GetStringChunks(string str)
    {
        List<byte[]> chunks = [];

        byte[] utf8 = Encoding.UTF8.GetBytes(str);
        int offset = 0;

        while (offset < utf8.Length)
        {
            int take = Math.Min(ChunksData.MAX_BYTES_PER_CHUNK, utf8.Length - offset);

            byte[] chunk = new byte[take];
            Array.Copy(utf8, offset, chunk, 0, take);

            chunks.Add(chunk);

            offset += take;
        }

        return chunks;
    }

    public void Dispose()
    {

        Console.WriteLine("cleaning up...");

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