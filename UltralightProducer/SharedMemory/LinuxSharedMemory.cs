using System.IO;
using System.IO.MemoryMappedFiles;

public unsafe class LinuxSharedMemory : ISharedMemory
{
    const string PATH = "/dev/shm/browser_shared_texture";

    FileStream fs;
    MemoryMappedFile mmf;
    MemoryMappedViewAccessor accessor;
    byte* ptr;

    public MemoryMappedViewAccessor Accessor => accessor;
    public byte* Pointer => ptr;

    public void CreateOrOpen(long size)
    {
        fs = new FileStream(PATH, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        fs.SetLength(size);

        mmf = MemoryMappedFile.CreateFromFile(fs, null, size, MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, false);
        accessor = mmf.CreateViewAccessor();
        ptr = (byte*)accessor.SafeMemoryMappedViewHandle.DangerousGetHandle();
    }

    public void Dispose()
    {
        accessor?.Dispose();
        mmf?.Dispose();
        fs?.Dispose();
    }
}