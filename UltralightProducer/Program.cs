using UltralightUnity;

class Program
{
    public static ULRenderer Renderer {get; private set;}
    unsafe static void Main()
    {
        NativeLoader.Load("libUltralightCore.so");
        NativeLoader.Load("libWebCore.so");
        NativeLoader.Load("libUltralight.so");
        NativeLoader.Load("libAppCore.so");

        using var ultralightManager = new UltralightManager();


        UltralightPlatform.Initialize();
        var config = new ULConfig();
        Renderer = new ULRenderer(config);

        ultralightManager.Init();

        int frame = 0;
        var fps = new FpsCounter();
        Console.WriteLine("Starting ultralight");
        while (true)
        {
            ultralightManager.BeforeUpdate();

            Renderer.Update();
            Renderer.RefreshDisplay(0);
            Renderer.Render();

            ultralightManager.Update();

            frame++;
            fps.Frame();
        }
    }
}