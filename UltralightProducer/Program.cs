using UltralightUnity;

class Program
{
    public static ULRenderer Renderer {get; private set;}
    public static UltralightManager Manager {get; private set;}
    unsafe static void Main()
    {
        NativeLoader.Load("libUltralightCore.so");
        NativeLoader.Load("libWebCore.so");
        NativeLoader.Load("libUltralight.so");
        NativeLoader.Load("libAppCore.so");

        AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);  

        Manager = new UltralightManager();


        UltralightPlatform.Initialize();
        var config = new ULConfig();
        Renderer = new ULRenderer(config);

        Manager.Init();

        int frame = 0;
        var fps = new FpsCounter();
        Console.WriteLine("Starting ultralight");
        while (Manager.TestIfRun())
        {
            if (Manager.TestIfSleep())
            {
                Thread.Sleep(1);
                continue;
            }
            Manager.BeforeUpdate();

            Renderer.Update();
            Renderer.RefreshDisplay(0);
            Renderer.Render();

            Manager.Update();

            frame++;
            fps.Frame();
        }
        CurrentDomain_ProcessExit(null, null);
    }

    private static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
    {
        Console.WriteLine("Invalid magic. Closing...");
        Manager.Dispose();
    }
}