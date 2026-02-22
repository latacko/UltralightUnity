
using UltralightUnity;

public static class UltralightPlatform
{
    private static ULString _baseDir;
    private static ULString _logPath;

    public static void Initialize()
    {
        ULPlatform.EnablePlatformFontLoader();

        ULPlatform.EnablePlatformFileSystem(AppDomain.CurrentDomain.BaseDirectory);

        ULPlatform.EnableDefaultLogger(AppDomain.CurrentDomain.BaseDirectory + "data.log");
    }
}
