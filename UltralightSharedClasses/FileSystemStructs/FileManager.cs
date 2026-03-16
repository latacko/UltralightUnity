using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using UltralightSharedClasses.Classes;
using UltralightSharedClasses.Structs;

namespace UltralightSharedClasses.FileSystemStructs
{
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

            FileHeader _bHeader = new FileHeader()
            {
                length = fileData.Length,
                offset = (uint)_fileHeaderSize
            };
            _accessor.Write(0, ref _bHeader);
            _accessor.WriteArray(_fileHeaderSize, fileData, 0, fileData.Length);

            return _id;
        }
    }
}