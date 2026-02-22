using System.Runtime.InteropServices;

public static class SharedMemoryFactory
{
    public static ISharedMemory Create()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return new WindowsSharedMemory();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return new LinuxSharedMemory();

        throw new PlatformNotSupportedException();
    }
}