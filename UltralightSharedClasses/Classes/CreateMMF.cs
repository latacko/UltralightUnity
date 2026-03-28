using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace UltralightSharedClasses.Classes
{
    public static class CreateMMF
    {
        public const string LINUX_PATH = "/dev/shm/";
        public static MemoryMappedFile CreateMemoryMappedFile(string name, int totalSize)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                return MemoryMappedFile.CreateNew(name, totalSize);


            using var _fs = new FileStream(LINUX_PATH + name, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            _fs.SetLength(totalSize);
            return MemoryMappedFile.CreateFromFile(_fs, null, totalSize, MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, false);
        }

        public static MemoryMappedFile OpenMemoryMappedFile(string name)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                return MemoryMappedFile.OpenExisting(name);

            using var _fs = new FileStream(LINUX_PATH + name, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

            long length = _fs.Length;

            return MemoryMappedFile.CreateFromFile(LINUX_PATH + name, FileMode.Open, null, length, MemoryMappedFileAccess.ReadWrite);
        }
    }
}