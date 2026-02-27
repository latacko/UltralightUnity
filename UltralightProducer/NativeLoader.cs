using System.Reflection;
using System.Runtime.InteropServices;

static class NativeLoader
{
    private static string GetResourcePath(string fileName)
    {
        string platformFolder = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "win-x64" : "linux-x64";

        return $"{Assembly.GetExecutingAssembly().GetName().Name}.libs.{platformFolder}.{fileName}";
    }
    public static void Load(string baseName)
    {
        string fileName = GetPlatformLibraryName(baseName);

        string resourceName = GetResourcePath(fileName);

        string tempDir = Path.Combine(Path.GetTempPath(), "UltralightUnity", Assembly.GetExecutingAssembly().GetName().Version!.ToString());

        Directory.CreateDirectory(tempDir);

        string tempPath = Path.Combine(tempDir, fileName);

        using Stream? stream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(resourceName);

        if (stream == null)
            throw new Exception($"Resource {resourceName} not found");

        using FileStream fs = new FileStream(tempPath, FileMode.Create);
        stream.CopyTo(fs);

        NativeLibrary.Load(tempPath);
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