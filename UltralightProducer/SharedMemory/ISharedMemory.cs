using System.IO.MemoryMappedFiles;

public interface ISharedMemory : IDisposable
{
    void CreateOrOpen(long size);
    MemoryMappedViewAccessor Accessor { get; }
    unsafe byte* Pointer { get; }
}