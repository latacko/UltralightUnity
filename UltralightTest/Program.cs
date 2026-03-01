using System.Runtime.InteropServices;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;


internal class Program
{
    private static void Main(string[] args)
    {

        UltralightPlatform.Initialize();
        var config = new UltralightUnity.ULConfig();
        config.FaceWinding = UltralightUnity.Enums.ULFaceWinding.CounterClockwise;
        var renderer = new UltralightUnity.ULRenderer(config);
        var viewConfig = new UltralightUnity.ULViewConfig();
        viewConfig.SetIsAccelerated(false);
        var view = renderer.CreateView(800, 600, viewConfig);
        
        view.LoadURL("https://github.com/SupinePandora43/UltralightNet/blob/b596b10aa60e0d7d8b216121e546434572a2df52/src/UltralightNet.Test/View.cs#L81");
        view.OnMessageConsole += OnMessageConsole;
        string error = "";
        string result = view.EvaluateScript("1+1", out error);
        Console.WriteLine("Result: " + result);
        Console.WriteLine("Error: " + error);
        var game = new Window(
            GameWindowSettings.Default,
            new NativeWindowSettings
            {
                ClientSize = new Vector2i(800, 600),
                Title = "Texture Window"
            }, renderer, view);

        game.Run();

        game.Close();

        // Console.WriteLine("Test1");


        // Console.WriteLine("Test2");

        // config.ResourcePathPrefix = "resources/";
        // // config.UserStylesheet = "body { background: purple; }";
        // Console.WriteLine("Test3");

        // Console.WriteLine("Test4");


        // bool loaded = false;

        // var view = renderer.CreateView(1920, 1080, viewConfig);
        // view.LoadURL("https://css-loaders.com/growing/#l7");
        // view.OnFinishLoading += () =>
        // {
        //     Console.WriteLine("Loaded");
        //     loaded = true;
        // };

        // uint frame = 0;
        // while (true)
        // {
        //     frame++;
        //     Console.WriteLine("loding frame: " + frame);
        //     renderer.Update();
        //     // give time to process network etc.
        //     Thread.Sleep(10);
        //     RenderOneFrame();

        //     if (frame > 1)
        //     {
        //         frame = 0;
        //         WriteToPng();
        //     }
        // }

        // void RenderOneFrame()
        // {
        //     renderer.RefreshDisplay(0);
        //     renderer.Render();

        //     if (view.GetSurface().IsDirty)
        //         WriteToPng();
        // }

        // unsafe void WriteToPng()
        // {
        //     var surface = view.GetSurface();
        //     var bitmap = surface.GetBitmap();
        //     nint pixels = bitmap.LockPixels();
        //     int size = (int)bitmap.Size;
        //     int width = (int)bitmap.Width;
        //     int height = (int)bitmap.Height;

        //     // copy to managed array
        //     byte[] managed = new byte[size];
        //     Marshal.Copy(pixels, managed, 0, size);

        //     bitmap.UnlockPixels();

        //     // create image (assuming BGRA)
        //     using var image = Image.LoadPixelData<Bgra32>(managed, width, height);

        //     // save PNG
        //     image.Save("OUTPUT.png");
        // }
    }

    private static void OnMessageConsole(ULMessageSource soruce, ULMessageLevel level, string message, uint line_number, uint column_number, string source_id)
    {
        Console.WriteLine(soruce + " " + level + " Message: " + message + " Line number " + line_number + " Column number " + column_number + " Source id " + source_id);
    }
}