using UltralightUnity.Native;

namespace UltralightUnity;

public static class ULPlatform
{
    public static void EnablePlatformFontLoader()
    {
        NativePlatform.ulEnablePlatformFontLoader();
    }

    public static void EnablePlatformFileSystem(string baseDir)
    {
        using var s = new ULString(baseDir);
        NativePlatform.ulEnablePlatformFileSystem(s.Handle);
    }

    public static void EnableDefaultLogger(string log_path)
    {
        using var s = new ULString(log_path);
        NativePlatform.ulEnableDefaultLogger(s.Handle);
    }
}