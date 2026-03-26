using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using UltralightSharedClasses.Classes;
using UltralightSharedClasses.Structs;
using UltralightUnity;

public unsafe class UltralightManager : IDisposable
{
    const uint MAGIC = 0x6C617461;

    const string PATH = BASE_FILE_NAME.BASE_PATH + BASE_FILE_NAME.MANAGER;

    MemoryMappedFile mmf;
    MemoryMappedViewAccessor accessor;

    Header* header;
    byte* basePtr;

    int requestViewOffset;
    int destroyViewOffset;

    public readonly Dictionary<uint, UltralightViewManager> Views = [];

    public void Init()
    {
        int requestViewEventsSize = sizeof(RequestViewEvent) * ChunksData.REQUEST_VIEW_EVENT_CHUNKS;
        int destroyViewEventsSize = sizeof(DestroyViewEvent) * ChunksData.DESTORY_VIEW_EVENT_CHUNKS;

        int total =
            Marshal.SizeOf<Header>() +
            requestViewEventsSize +
            requestViewEventsSize;

        mmf = CreateMMF.CreateMemoryMappedFile(BASE_FILE_NAME.MANAGER, total);

        accessor = mmf.CreateViewAccessor();

        byte* ptr = (byte*)accessor.SafeMemoryMappedViewHandle.DangerousGetHandle();
        basePtr = ptr;
        header = (Header*)ptr;
        int headerSize = Marshal.SizeOf<Header>();

        requestViewOffset = headerSize;
        destroyViewOffset = requestViewOffset + requestViewEventsSize;

        header->magic = MAGIC;


        /* OFFSETS */
        header->requestViewOffset = (uint)requestViewOffset;
        header->destroyViewOffset = (uint)destroyViewOffset;
    }

    public bool TestIfSleep()=>header->requestCounter <= header->frameCounter;

    public (uint, uint) GetScreenSize()
    {
        return (header->ScreenWidth, header->ScreenHeight);
    }

    public void BeforeUpdate()
    {
        ReadRequestViewEvents();
        ReadDestroyViewEvents();

        foreach (var view in Views)
        {
            view.Value.BeforeUpdate();
        }

        header->frameCounter++;
    }

    public void Update()
    {
        foreach (var view in Views)
        {
            view.Value.Update();
        }
    }

    void ReadRequestViewEvents()
    {
        while (header->RequestViewEventRead < header->RequestViewEventWrite)
        {
            int index = (int)(header->RequestViewEventRead % 128);
            RequestViewEvent* ev = (RequestViewEvent*)(basePtr + requestViewOffset + index * sizeof(RequestViewEvent));

            Console.WriteLine("Got request for view");
            ev->id = CreateView(ev);

            header->RequestViewEventRead++;
        }
    }
    
    public uint CreateView(RequestViewEvent* ev)
    {
        var _random = new Random();
        uint _id;
        do
        {
            _id = (uint)_random.Next(1000000000, int.MaxValue);
        } while (Views.ContainsKey(_id));

        Console.WriteLine("Creating view it id: " + _id);
        Views[_id] = new UltralightViewManager(_id, ev->width, ev->height, ev->isTransparent == 1);
        Console.WriteLine("Created view it id: " + _id);
        return _id;
    }

    void ReadDestroyViewEvents()
    {
        while (header->DestroyViewEventRead < header->DestroyViewEventWrite)
        {
            int index = (int)(header->DestroyViewEventRead % 128);
            DestroyViewEvent* ev = (DestroyViewEvent*)(basePtr + destroyViewOffset + index * sizeof(DestroyViewEvent));

            Console.WriteLine("Destroying view");
            Views[ev->id].Dispose();
            Views.Remove(ev->id);
            Console.WriteLine("Destroyed view");

            header->DestroyViewEventRead++;
        }
    }

    public void Dispose()
    {

        Console.WriteLine("cleaning up...");

        foreach (var view in Views)
        {
            view.Value.Dispose();
        }

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

    internal bool TestIfRun()
    {
        return header->magic == MAGIC;
    }
}