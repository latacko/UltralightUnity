using System.Drawing;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using UltralightSharedClasses.Classes;
using UltralightSharedClasses.Enums;
using UltralightSharedClasses.StringHeaders;
using UltralightUnity;

public unsafe partial class UltralightViewManager : IDisposable
{
    ULView inspectorView;
    void RegisterEvents()
    {
        View.OnBeginLoading += OnBeginLoading;
        View.OnFinishLoading += OnBeginLoading;
        View.OnDOMReady += OnDOMReady;
        View.OnLoadFailed += OnLoadField;
        View.OnURLChanged += OnURLChanged;
        View.OnInspectorRequest += OnInspectorRequest;
        View.OnMessageConsole += OnMessageConsole;

        View.MessageEmitted += MessageEmitted;
    }

    void OnDOMReady(ULView view)
    {
        WriteBaseEvent(BaseEventType.OnDOMReady);
    }
    void OnBeginLoading()
    {
        WriteBaseEvent(BaseEventType.BeginLoading);
    }

    void OnFinishLoading()
    {
        WriteBaseEvent(BaseEventType.FinishLoading);
    }

    private void OnLoadField(ulong frameid, bool isMainFrame, string url, string desc, string errorDomain, int errorCode)
    {
        LoadFieldHeader _lfh = new()
        {
            frameId = frameid,
            isMainFrame = (byte)(isMainFrame ? 1 : 0),
            errorCode = (uint)errorCode,
        };
        uint _id = StringManager.GenerateMMF<LoadFieldHeader>(EventType.LoadFailed, _lfh, url, desc, errorDomain);
        WriteLoadAdvancedEvent(_id);
    }

    private void OnURLChanged(string newUrl)
    {
        uint _id = StringManager.GenerateMMF<EmptyHeader>(EventType.UrlChanged, null, newUrl);
        WriteLoadAdvancedEvent(_id);
    }

    private ULView OnInspectorRequest(bool isLocal, string inspector_url)
    {
        if (inspectorView != null)
            return inspectorView;

        (var width, var height) = Program.Manager.GetScreenSize();
        using var _viewConfig = new ULViewConfig();
        _viewConfig.SetIsAccelerated(false);
        inspectorView = Program.Renderer.CreateView(width / 2, height / 2, _viewConfig);
        Console.WriteLine("Created inspector");
        return inspectorView;
    }

    private void OnMessageConsole(ULMessageSource source, ULMessageLevel level, string message, uint line_number, uint column_number, string source_id)
    {
        uint _id = StringManager.GenerateMMF<MessageConsoleHeader>(EventType.MessageConsole, new()
        {
            source = (uint)source,
            level = (uint)level,
            line_number = line_number,
            column_number = column_number
        }, message, source_id);
        WriteMessageConsole(_id);
    }

    private void MessageEmitted(string sender, string json)
    {
        uint _id = StringManager.GenerateMMF<EmptyHeader>(EventType.MessageEmitted, null, sender, json);
        WriteMessageEmitted(_id);
    }
}