using System.Drawing;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using UltralightUnity;

public unsafe partial class UltralightViewManager : IDisposable
{
    void RegisterEvents()
    {
        View.OnBeginLoading += OnBeginLoading;
        View.OnFinishLoading += OnBeginLoading;
        View.OnDOMReady += OnBeginLoading;
        View.OnLoadFailed += OnLoadField;
        View.OnURLChanged += OnURLChanged;
    }

    void OnDOMReady()
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

    private void OnLoadField(string url, string desc, int errorCode)
    {
        LoadFieldHeader _lfh = new()
        {

            errorCode = (uint)errorCode,
        };
        uint _id = StringManager.GenerateMMF<LoadFieldHeader>(EventType.LoadFailed, _lfh, url, desc);
        WriteLoadAdvancedEvent(_id);
    }

    private void OnURLChanged(string newUrl)
    {
        uint _id = StringManager.GenerateMMF<EmptyHeader>(EventType.UrlChanged, null, newUrl);
        WriteLoadAdvancedEvent(_id);
    }
}