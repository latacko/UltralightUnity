using System.Runtime.InteropServices;

static class NativeLoader
{
    public static void Load(string baseName)
    {
        string fileName = GetPlatformLibraryName(baseName);

        string runtime = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "win_x64" : "linux_x64";

        string path = Path.Combine( AppContext.BaseDirectory, "runtimes", runtime, "native", fileName);

        if (!File.Exists(path))
            throw new Exception($"Native library not found: {path}");

        NativeLibrary.Load(path);
    }

    private static string GetPlatformLibraryName(string baseName)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return baseName + ".dll";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return "lib" + baseName + ".so";

        throw new PlatformNotSupportedException();
    }
}