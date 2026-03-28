using System.Runtime.InteropServices;
using UltralightSharedClasses.Classes;
using UltralightSharedClasses.FileSystemStructs;
using UltralightSharedClasses.Structs;
using UltralightUnity;

public static class FileManagerExtensions
{
    private static readonly HashSet<ULBuffer> _alive = new();
    private static readonly object _lock = new();

    public static ULBuffer ReadFile(uint id)
    {
        string _name = BASE_FILE_NAME.FILE + id;

        using var _mmf = CreateMMF.OpenMemoryMappedFile(_name);
        using var _accessor = _mmf.CreateViewAccessor();
        _accessor.Read(0, out FileHeader header);

        byte[] buffer = new byte[header.length];
        _accessor.ReadArray(header.offset, buffer, 0, header.length);

        Console.WriteLine($"Buffer size: {header.length}");
        Console.WriteLine($"First bytes: {BitConverter.ToString(buffer, 0, Math.Min(8, buffer.Length))}");


        var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        var _buffer = new ULBuffer(handle.AddrOfPinnedObject(), (uint)buffer.Length);

        lock (_lock)
        {
            _alive.Add(_buffer);
            Console.WriteLine($"Buffer added to alive, count: {_alive.Count}"); // ✅ add this
        }

        _buffer.OnDestroyBuffer += (_, _) =>
        {
            Console.WriteLine($"Buffer destroyed for file {id}"); // ✅ does this print?
            handle.Free();
            lock (_lock) { _alive.Remove(_buffer); }
        };

        DisposeFile(id);
        return _buffer;
    }

    private static void DisposeFile(uint id)
    {
        string _name = BASE_FILE_NAME.FILE + id;
        using var _mmf = CreateMMF.OpenMemoryMappedFile(_name);
        using var _accessor = _mmf.CreateViewAccessor();
        _accessor.Read(0, out FileHeader header);
        header.ToDispose = 1;
        _accessor.Write(0, ref header);
    }
}