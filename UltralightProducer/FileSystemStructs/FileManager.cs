using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using UltralightUnity;

public struct FilesHolder
{
    public readonly MemoryMappedFile Mmf;
    public readonly MemoryMappedViewAccessor Accessor;

    public FilesHolder(MemoryMappedFile mmf, MemoryMappedViewAccessor accessor)
    {
        Mmf = mmf;
        Accessor = accessor;
    }
}

public static class FileManager
{
    public static uint GenerateMMF(byte[] fileData)
    {
        uint _id = (uint)Guid.NewGuid().GetHashCode();

        string _name = BASE_FILE_NAME.FILE + _id.ToString();

        int _fileHeaderSize = Marshal.SizeOf<FileHeader>();


        int _totalSize = _fileHeaderSize + fileData.Length;

        var _mmf = CreateMMF.CreateMemoryMappedFile(_name, _totalSize);
        var _accessor = _mmf.CreateViewAccessor();

        FileHeader _bHeader = new()
        {
            length = fileData.Length,
            offset = (uint)_fileHeaderSize
        };
        _accessor.Write(0, ref _bHeader);
        _accessor.WriteArray(_fileHeaderSize, fileData, 0, fileData.Length);

        return _id;
    }
#if NET
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
#endif
}