
using UltralightUnity;

public static class UltralightPlatform
{
    public static string BaseDir;

    public static void Initialize()
    {
        ULPlatform.EnablePlatformFontLoader();

        Console.WriteLine(BaseDir);
        // ULPlatform.EnablePlatformFileSystem(BaseDir);

        ULPlatform.EnableDefaultLogger(BaseDir + "data.log");
    }
}
