using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

[SupportedOSPlatform("windows")]
public unsafe class WindowsSharedMemory : ISharedMemory
{
    MemoryMappedFile mmf;
    MemoryMappedViewAccessor accessor;
    byte* ptr;

    public MemoryMappedViewAccessor Accessor => accessor;
    public byte* Pointer => ptr;

    public void CreateOrOpen(long size)
    {
#pragma warning disable CA1416
        mmf = MemoryMappedFile.CreateOrOpen("browser_shared_texture", size);
#pragma warning restore CA1416

        accessor = mmf.CreateViewAccessor();
        ptr = (byte*)accessor.SafeMemoryMappedViewHandle.DangerousGetHandle();
    }

    public void Dispose()
    {
        accessor?.Dispose();
        mmf?.Dispose();
    }
}