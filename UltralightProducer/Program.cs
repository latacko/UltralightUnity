using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Runtime.InteropServices;
using UltralightUnity;

class Program
{
    unsafe static void Main()
    {
        NativeLoader.Load("libUltralightCore.so");
        NativeLoader.Load("libWebCore.so");
        NativeLoader.Load("libUltralight.so");
        NativeLoader.Load("libAppCore.so");

        using var ultralightManager = new UltralightManager();


        UltralightPlatform.Initialize();
        var config = new ULConfig();
        var renderer = new ULRenderer(config);

        var viewConfig = new ULViewConfig();
        viewConfig.SetIsAccelerated(false);
        
        var view = renderer.CreateView(UltralightManager.WIDTH, UltralightManager.HEIGHT, viewConfig);
        MouseManager.view = view;
        KeysManager.view = view;

        view.LoadURL("https://google.com");

        ultralightManager.Init();

        int frame = 0;
        var fps = new FpsCounter();
        while (true)
        {
            
            ultralightManager.Update(view.GetSurface().GetBitmap());

            renderer.Update();
            renderer.RefreshDisplay(0);
            renderer.Render();
            frame++;
            fps.Frame();
        }
    }
}