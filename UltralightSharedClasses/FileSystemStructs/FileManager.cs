using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using UltralightSharedClasses.Classes;
using UltralightSharedClasses.Structs;

namespace UltralightSharedClasses.FileSystemStructs
{

    public static class FileManager
    {
        readonly static Dictionary<uint, FileHolder> generatedFiles = new();
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
            generatedFiles.Add(_id, new(_mmf, _accessor));

            return _id;
        }

        static readonly List<uint> toDelete = new();
        public static void TestIfDelete()
        {
            toDelete.Clear();
            foreach (var item in generatedFiles)
            {
                item.Value.Accessor.Read(0, out FileHeader header);
                if (header.ToDispose == 0) continue;

                toDelete.Add(item.Key);
            }

            foreach (var id in toDelete)
            {
                DisposeFile(id);
            }
        }

        public static void DeleteAll()
        {
            toDelete.Clear();
            foreach (var item in generatedFiles)
            {
                toDelete.Add(item.Key);
            }

            foreach (var id in toDelete)
            {
                DisposeFile(id);
            }
        }

        private static void DisposeFile(uint id)
        {
            generatedFiles[id].Accessor.Dispose();
            generatedFiles[id].Mmf.Dispose();
            generatedFiles.Remove(id);

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                File.Delete(CreateMMF.LINUX_PATH + BASE_FILE_NAME.FILE + id.ToString());
            }
        }
    }
}