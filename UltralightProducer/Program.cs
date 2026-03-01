using UltralightUnity;

class Program
{
    public static ULRenderer Renderer { get; private set; }
    public static UltralightManager Manager { get; private set; }
    // public static ULFileSystem unityFs { get; private set; }
    public static UnityFileSystem unityFs { get; private set; }
    static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            UltralightPlatform.BaseDir = args[0];
        } else
        {
            UltralightPlatform.BaseDir = AppDomain.CurrentDomain.BaseDirectory;
        }

        NativeLoader.Load("UltralightCore");
        NativeLoader.Load("WebCore");
        NativeLoader.Load("Ultralight");
        NativeLoader.Load("AppCore");

        AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

        Manager = new UltralightManager();
        Manager.Init();

        // unityFs = new ULFileSystem(UltralightPlatform.BaseDir);
        unityFs = new UnityFileSystem();

        UltralightPlatform.Initialize();
        var config = new ULConfig();
        Renderer = new ULRenderer(config);


        int frame = 0;
        var fps = new FpsCounter();
        Console.WriteLine("Starting ultralight");

        // Thread workingThread = new Thread(new ParameterizedThreadStart(DoJob))
        // { IsBackground = true };


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
        unityFs.Dispose();
        Manager.Dispose();
    }
}