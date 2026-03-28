using System.Diagnostics;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using UltralightUnity;
using UltralightUnity.HandlesJs;
using UltralightUnity.NativeJs;
using static UltralightUnity.JSObject;


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
        JSObjectCallAsFunctionCallback btnFunction;
        // var inspecotr_view = renderer.CreateView(800, 600, viewConfig);

        // inspecotr_view.LoadURL("https://google.com");
        // inspecotr_view.LoadURL("inspector:///127.0.0.1:7676");
        // inspecotr_view.LoadURL("file:///inspector/Main.html");
        // inspecotr_view.OpenInspector();
        bool success = renderer.StartRemoteInspector("127.0.0.1", 7676);
        view.LoadURL("file:///test.html");
        // inspecotr_view.OnMessageConsole += OnMessageConsole;
        // inspecotr_view.OnDOMReady += OnDOMReady;
        // inspecotr_view.OnInspectorRequest += OnInspectorRequest;

        btnFunction = OnButtonClick;

        view.OnMessageConsole += OnMessageConsole;
        view.OnDOMReady += OnDOMReady;
        view.OnInspectorRequest += OnInspectorRequest;

        view.MessageEmitted += MessageEmitted;

        // string error = "";
        // string result = view.EvaluateScript("1+1", out error);
        // Console.WriteLine("Result: " + result);
        // Console.WriteLine("Error: " + error);

        Window RunWindow(GameWindowSettings gws, NativeWindowSettings nws, ULView _view)
        {
            return new Window(gws, nws, renderer, _view);
        }


        var browser_window = RunWindow(GameWindowSettings.Default, new NativeWindowSettings
        {
            ClientSize = new Vector2i(800, 600),
            Title = "Texture Window"
        }, view);
        // var inspecotr_window = RunWindow(GameWindowSettings.Default, new NativeWindowSettings
        // {
        //     ClientSize = new Vector2i(800, 600),
        //     Title = "Inspector Window"
        // }, inspecotr_view);

        Stopwatch _watchUpdate = new();
        _watchUpdate.Start();

        view.OpenInspector();
        browser_window.MakeCurrent(); // <-- important
        browser_window.OnLoadP();
        // inspecotr_window.MakeCurrent(); // <-- important
        // inspecotr_window.OnLoadP();

        while (!browser_window.IsExiting)
        // while (!browser_window.IsExiting && !inspecotr_window.IsExiting)
        {
            double updatePeriod = browser_window.UpdateFrequency == 0
                ? 0 : 1.0 / browser_window.UpdateFrequency;
            double elapsed = _watchUpdate.Elapsed.TotalSeconds;

            if (elapsed > updatePeriod)
            {
                _watchUpdate.Restart();

                browser_window.NewInputFrame();
                // inspecotr_window.NewInputFrame();
                NativeWindow.ProcessWindowEvents(false);

                // UltraLight renders both views in one shot
                renderer.Update();
                renderer.Render();

                // Window 1
                browser_window.MakeCurrent();
                browser_window.OnUpdateFrameP(new FrameEventArgs(elapsed));
                browser_window.OnRenderFrameP(new FrameEventArgs(elapsed)); // SwapBuffers inside here

                // Window 2
                // inspecotr_window.MakeCurrent();
                // inspecotr_window.OnUpdateFrameP(new FrameEventArgs(elapsed));
                // inspecotr_window.OnRenderFrameP(new FrameEventArgs(elapsed));
            }

            double timeToNext = updatePeriod - _watchUpdate.Elapsed.TotalSeconds;
            if (timeToNext > 0)
                Thread.Sleep((int)(timeToNext * 1000)); // or Utils.AccurateSleep if accessible
        }
        browser_window.OnUnloadP();
        // inspecotr_window.OnUnloadP();

        browser_window.Close();
        // inspecotr_window.Close();
        browser_window.Dispose();
        // inspecotr_window.Dispose();

        void OnDOMReady(ULView view)
        {
            JSContext _context = view.LockJSContext();
            JSGlobalContext _globalContext = _context.GetGlobalContext();
            JSObject _func = _context.MakeFunctionWithCallback("OnButtonClick", btnFunction);
            _globalContext.SetProperty("OnButtonClick", _func, 0, out var exception);

            view.UnlockJSContext();
            view.OpenInspector();
        }

        JSValue OnButtonClick(JSContext ctx, JSObject function, JSObject thisObject, nint argumentCount, JSValue[] arguments, out JSValue? exception)
        {
            exception = null;
            ctx.EvaluateScript("document.getElementById('result').innerText = 'Ultralight rocks!'", null, null, 0, out JSValue script_exception);
            // ctx.EvaluateScript("console.log(w)");

            var _funcValue = ctx.EvaluateScript("Result2", null, null, 0, out _);
            if (_funcValue.IsObject())
            {
                var _funcObject = _funcValue.ToObject(out _);
                if (_funcObject != null && _funcObject.IsFunction())
                {
                    using var _msg = ctx.CreateWithUTF8CString("elo lamusy " + (++counter));
                    _funcObject.CallAsFunction(null,
                    [
                        _msg
                    ], out var exception2);
                }
            }
            else
            {
            }

            view.PostMessage(JsonConvert.SerializeObject(new{test=true, aaa="test2", num=2137}));

            return ctx.MakeNull();
        }
    }

    private static ULView OnInspectorRequest(bool arg1, string arg2)
    {
        Console.WriteLine("inspector");
        return null;
    }




    static int counter;

    private static void OnMessageConsole(ULMessageSource soruce, ULMessageLevel level, string message, uint line_number, uint column_number, string source_id)
    {
        Console.WriteLine(soruce + " " + level + " Message: " + message + " Line number " + line_number + " Column number " + column_number + " Source id " + source_id);
    }

    private static void MessageEmitted(string sender, string json)
    {
        Console.WriteLine("Post message from " + sender + " json: " + json);
    }
}