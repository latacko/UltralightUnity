using System.IO;
using System.IO.MemoryMappedFiles;

public static class CreateMMF
{
    public const string LINUX_PATH = "/dev/shm/";
    public static MemoryMappedFile CreateMemoryMappedFile(string name, int totalSize)
    {
        #if UNITY_STANDALONE_WIN
            return MemoryMappedFile.CreateNew(name, totalSize);
        #elif UNITY_STANDALONE_LINUX
        #else
            if (OperatingSystem.IsWindows())
                return MemoryMappedFile.CreateNew(name, totalSize);
        #endif
        using var _fs = new FileStream(LINUX_PATH + name, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        _fs.SetLength(totalSize);
        return MemoryMappedFile.CreateFromFile(_fs, null, totalSize, MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, false);
    }

    public static MemoryMappedFile OpenMemoryMappedFile(string name)
    {
        #if UNITY_STANDALONE_WIN
            return MemoryMappedFile.OpenExisting(name);
        #elif UNITY_STANDALONE_LINUX
        #else
            if (OperatingSystem.IsWindows())
                return MemoryMappedFile.OpenExisting(name);
        #endif

        using var _fs = new FileStream(LINUX_PATH + name, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

        long length = _fs.Length;

        return MemoryMappedFile.CreateFromFile(LINUX_PATH + name, FileMode.Open, null, length, MemoryMappedFileAccess.ReadWrite);
    }
}