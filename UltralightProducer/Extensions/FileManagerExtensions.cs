using System.Runtime.InteropServices;
using UltralightSharedClasses.Classes;
using UltralightSharedClasses.FileSystemStructs;
using UltralightSharedClasses.Structs;
using UltralightUnity;

public static class FileManagerExtensions
{
    public static ULBuffer ReadFile(uint id)
    {
        string _name = BASE_FILE_NAME.FILE + id;

        using var _mmf = CreateMMF.OpenMemoryMappedFile(_name);
        using var _accessor = _mmf.CreateViewAccessor();
        _accessor.Read(0, out FileHeader header);

        byte[] buffer = new byte[header.length];
        _accessor.ReadArray(header.offset, buffer, 0, header.length);

        var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);


        var _buffer = new ULBuffer(handle.AddrOfPinnedObject(), (uint)buffer.Length);
        _buffer.OnDestroyBuffer += (userData, data) =>
        {
            Console.WriteLine("On buffer destroy for file " + id);
            handle.Free();
            DisposeFile(id);
        };
        return _buffer;
    }

    private static void DisposeFile(uint id)
    {
        Console.WriteLine("Deleting file " + id);
#if UNITY_STANDALONE_WIN
#elif UNITY_STANDALONE_LINUX
            File.Delete(CreateMMF.LINUX_PATH + BASE_FILE_NAME.FILE + id.ToString());
#else
        if (OperatingSystem.IsLinux())
        {
            Console.WriteLine("Deleting on path " + (CreateMMF.LINUX_PATH + BASE_FILE_NAME.FILE + id.ToString()));
            File.Delete(CreateMMF.LINUX_PATH + BASE_FILE_NAME.FILE + id.ToString());

        }
#endif
    }
}