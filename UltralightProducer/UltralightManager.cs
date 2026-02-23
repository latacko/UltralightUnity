using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using UltralightUnity;

public unsafe class UltralightManager : IDisposable
{
    const string PATH = "/dev/shm/browser_shared_texture";
    const uint MAGIC = 0xBEEFBEEF;

    public const int WIDTH = 1920;
    public const int HEIGHT = 1080;
    const int BUFS = 3;

    int bufferSize;
    MemoryMappedFile mmf;
    MemoryMappedViewAccessor accessor;

    Header* header;
    byte* basePtr;
    int headerSize;
    int mouseOffset;
    int keyOffset;
    int textOffset;
    int frameOffset;

    public void Init()
    {
        bufferSize = WIDTH * HEIGHT * 4;
        int mouseEventsSize = sizeof(MouseEvent) * 128;
        int keyEventsSize = sizeof(KeyEvent) * 128;
        int textEventsSize = sizeof(InputTextEvent) * 128;

        int total =
            Marshal.SizeOf<Header>() +
            mouseEventsSize +
            keyEventsSize +
            textEventsSize +
            bufferSize * BUFS;

        var fs = new FileStream(PATH, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        fs.SetLength(total);

        mmf = MemoryMappedFile.CreateFromFile(
            fs,
            null,
            total,
            MemoryMappedFileAccess.ReadWrite,
            HandleInheritability.None,
            false);

        accessor = mmf.CreateViewAccessor();

        byte* ptr = (byte*)accessor.SafeMemoryMappedViewHandle.DangerousGetHandle();
        basePtr = ptr;
        header = (Header*)ptr;
        headerSize = Marshal.SizeOf<Header>();

        mouseOffset = headerSize;
        keyOffset = mouseOffset + sizeof(MouseEvent) * 128;
        textOffset = keyOffset + sizeof(KeyEvent) * 128;
        frameOffset = textOffset + sizeof(InputTextEvent) * 128;

        header->magic = MAGIC;

        header->mouseOffset = (uint)mouseOffset;
        header->keyOffset = (uint)keyOffset;
        header->textOffset = (uint)textOffset;
        header->frameOffset = (uint)frameOffset;

        header->width = WIDTH;
        header->height = HEIGHT;
        header->bufferSize = (uint)bufferSize;
        header->writeIndex = 0;
        header->frameCounter = 0;
        header->requestCounter = 0;
        header->resizeCounter = 0;


        header->buttonEventRead = 0;
        header->buttonEventWrite = 0;
        header->inputTextEventWrite = 0;
        header->inputTextEventRead = 0;
        header->keyEventWrite = 0;
        header->keyEventRead = 0;
        header->buttonEventWrite = 0;
        header->buttonEventRead = 0;

        DebugLayout();

        Console.WriteLine($"mouseOffset: {mouseOffset}");
        Console.WriteLine($"keyOffset: {keyOffset}");
        Console.WriteLine($"textOffset: {textOffset}");
        Console.WriteLine($"frameOffset: {frameOffset}");

        Console.WriteLine($"Magic: {header->magic:X}");
    }

    public void DebugLayout()
    {
        Console.WriteLine($"Header size: {Marshal.SizeOf<Header>()}");
        Console.WriteLine($"MouseEvent size: {sizeof(MouseEvent)}");
        Console.WriteLine($"KeyEvent size: {sizeof(KeyEvent)}");
        Console.WriteLine($"InputTextEvent size: {sizeof(InputTextEvent)}");
    }

    public void Update(ULBitmap bitmap)
    {
        while (header->requestCounter <= header->frameCounter)
        {
            Thread.Sleep(1);
        }

        Header* h = (Header*)basePtr;
        ReadMouseEvents(h);

        ReadKeyEvents(h);

        ReadTextEvent(h);

        // choose next buffer
        uint next = (header->writeIndex + 1) % BUFS;
        byte* buffer = basePtr + frameOffset + (header->bufferSize * (int)next);
        WriteBitmapToBuffer(bitmap, buffer);

        // memory barrier
        Thread.MemoryBarrier();

        header->writeIndex = next;
        header->frameCounter++;
    }

    public void ReadMouseEvents(Header* h)
    {
        while (h->buttonEventRead < h->buttonEventWrite)
        {
            int index = (int)(h->buttonEventRead % 128);
            MouseEvent* ev = (MouseEvent*)(basePtr + mouseOffset + index * sizeof(MouseEvent));

            MouseManager.ReadEvent(ev);

            h->buttonEventRead++;
        }
    }

    public void ReadKeyEvents(Header* h)
    {
        while (h->keyEventRead < h->keyEventWrite)
        {
            int index = (int)(h->keyEventRead % 128);
            KeyEvent* ev = (KeyEvent*)(basePtr + keyOffset + index * sizeof(KeyEvent));

            KeysManager.ReadEvent(ev);

            h->keyEventRead++;
        }
    }

    public void ReadTextEvent(Header* h)
    {
        while (h->inputTextEventRead < h->inputTextEventWrite)
        {
            int index = (int)(h->inputTextEventRead % 128);
            InputTextEvent* ev = (InputTextEvent*)(basePtr + textOffset + index * sizeof(InputTextEvent));

            KeysManager.InputText(ev);

            h->inputTextEventRead++;
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

    public void Dispose()
    {

        Console.WriteLine("cleaning up...");

        try
        {
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