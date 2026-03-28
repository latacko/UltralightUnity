using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using UltralightSharedClasses.Classes;
using UltralightSharedClasses.FileSystemStructs;
using UltralightSharedClasses.StringHeaders;
using UltralightSharedClasses.Structs;
using UltralightUnity;

public unsafe class UnityFileSystem : ULFileSystem, IDisposable
{
    const string PATH = BASE_FILE_NAME.BASE_PATH + BASE_FILE_NAME.FILE_MANAGER;

    MemoryMappedFile mmf;
    MemoryMappedViewAccessor accessor;

    FileSystemHeader* header;
    uint existOffset;
    uint fileOffset;

    public UnityFileSystem()
    {
        int fileExistSize = sizeof(FileExistEvent) * ChunksData.FILE_EXIST_CHUNKS;
        int fileLoadIdSize = sizeof(FileOpenId) * ChunksData.FILE_OPEN_CHUNKS;

        int total = Marshal.SizeOf<FileSystemHeader>() + fileLoadIdSize + fileExistSize;

        mmf = CreateMMF.CreateMemoryMappedFile(BASE_FILE_NAME.FILE_MANAGER, total);

        accessor = mmf.CreateViewAccessor();

        header = (FileSystemHeader*)accessor.SafeMemoryMappedViewHandle.DangerousGetHandle();

        existOffset = (uint)Marshal.SizeOf<FileSystemHeader>();
        fileOffset = (uint)(existOffset + fileExistSize);

        header->existOffset = existOffset;
        header->fileOffset = fileOffset;

        header->openFileRead = 0;
        header->openFileWrite = 0;

        header->fileExistRead = 0;
        header->fileExistWrite = 0;
    }

    FileExistEvent* WriteExistEvent(string path)
    {
        int index = (int)(header->fileExistWrite % ChunksData.FILE_EXIST_CHUNKS);


        byte* basePtr = (byte*)header;
        FileExistEvent* ev = (FileExistEvent*)(basePtr + existOffset + index * sizeof(FileExistEvent));

        ev->id = StringManager.GenerateMMF<EmptyHeader>(EventType.FileExist, null, path);

        Thread.MemoryBarrier();

        header->fileExistWrite++;

        return ev;
    }

    FileOpenId* WriteOpenFileEvent(string path)
    {
        int index = (int)(header->openFileWrite % ChunksData.FILE_OPEN_CHUNKS);

        byte* basePtr = (byte*)header;
        FileOpenId* ev = (FileOpenId*)(basePtr + fileOffset + index * sizeof(FileOpenId));
        ev->path_id = StringManager.GenerateMMF<EmptyHeader>(EventType.FileOpen, null, path);
        Thread.MemoryBarrier();

        header->openFileWrite++;

        return ev;
    }

    public override bool FileExists(string path)
    {
        var _header = WriteExistEvent(path);
        int _attempts = 0;
        while (_header->set == 0 && _attempts < 30000)
        {
            _attempts++;
            Thread.Sleep(1);
        }

        return _header->exist == 1;
    }

    public override ULBuffer OpenFile(string path)
    {
        Console.WriteLine("opening file " + path);
        var _header = WriteOpenFileEvent(path);
        int _attempts = 0;
        while (_header->file_id == 0 && _attempts < 30000)
        {
            _attempts++;
            Thread.Sleep(1);
        }
        if (_attempts >= 3000)
        {
            throw new Exception("fatal error file doesn't exist");
            return null;
        }
        var _buffer = FileManagerExtensions.ReadFile(_header->file_id);
        Console.WriteLine("opening file 2 " + path);

        return _buffer;
    }

    public void Dispose()
    {
        Console.WriteLine("cleaning up fs...");

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