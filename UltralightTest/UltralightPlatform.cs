
using UltralightUnity;

public static class UltralightPlatform
{
    private static ULString _baseDir;
    private static ULString _logPath;

    public static void Initialize()
    {
        ULPlatform.EnablePlatformFontLoader();

        var _path = AppDomain.CurrentDomain.BaseDirectory+"../../../html/";

        ULPlatform.EnablePlatformFileSystem(_path);
        Console.WriteLine(_path);
        ULPlatform.EnableDefaultLogger(_path+ "data.log");
    }
}
