using System;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using UltralightUnity.Handles;
using UltralightUnity.Native;

namespace UltralightUnity;

public sealed class ULView : IDisposable
{
    public event Action<string>? OnTitleChanged;
    public event Action<string>? OnURLChanged;
    public event Action? OnBeginLoading;
    public event Action? OnFinishLoading;
    public event Action? OnDOMReady;
    public event Action<string, string, int>? OnLoadFailed;
    public event Action<ULMessageSource, ULMessageLevel, string, uint, uint, string> OnMessageConsole;

    private NativeView.ChangeTitleCallback? _titleCb;
    private NativeView.ChangeURLCallback? _urlCb;
    private NativeView.SimpleViewCallback? _beginCb;
    private NativeView.SimpleViewCallback? _finishCb;
    private NativeView.SimpleViewCallback? _domReadyCb;
    private NativeView.FailLoadingCallback? _failCb;
    private NativeView.AddConsoleMessageCallback? _messageCB;

    internal ULViewHandle Handle { get; }

    public ULView(ULRenderer renderer, uint width, uint height, ULViewConfig config) : this(renderer, width, height, config, null)
    {
    }

    public ULView(ULRenderer renderer, uint width, uint height, ULViewConfig config, ULSession? session)
    {
        Handle = NativeView.ulCreateView(renderer.Handle, width, height, config.Handle, session == null ? ULSessionHandle.Null : session.Handle);


        _titleCb = TitleChangedEvent;
        _urlCb = URLChangedEvent;
        _beginCb = BeginLoadingEvent;
        _finishCb = FinishLoadingEvent;
        _domReadyCb = DOMReadyEvent;
        _failCb = LoadFailedEvent;
        _messageCB = AddConsoleMessageEvent;

        NativeView.ulViewSetChangeTitleCallback(Handle, _titleCb, IntPtr.Zero);
        NativeView.ulViewSetChangeURLCallback(Handle, _urlCb, IntPtr.Zero);
        NativeView.ulViewSetBeginLoadingCallback(Handle, _beginCb, IntPtr.Zero);
        NativeView.ulViewSetFinishLoadingCallback(Handle, _finishCb, IntPtr.Zero);
        NativeView.ulViewSetDOMReadyCallback(Handle, _domReadyCb, IntPtr.Zero);
        NativeView.ulViewSetFailLoadingCallback(Handle, _failCb, IntPtr.Zero);
        NativeView.ulViewSetAddConsoleMessageCallback(Handle, _messageCB, IntPtr.Zero);
    }

    public void LoadURL(string url)
    {
        using var s = new ULString(url);
        NativeView.ulViewLoadURL(Handle, s.Handle);
    }

    public void LoadHTML(string html)
    {
        using var s = new ULString(html);
        NativeView.ulViewLoadHTML(Handle, s.Handle);
    }

    public string URL => new ULString(NativeView.ulViewGetURL(Handle)).ToManagedString();

    public string Title => new ULString(NativeView.ulViewGetTitle(Handle)).ToManagedString();
    public bool IsLoading => NativeView.ulViewIsLoading(Handle);
    public bool HasInputFocus => NativeView.ulViewHasInputFocus(Handle);

    public bool NeedsPaint
    {
        get => NativeView.ulViewGetNeedsPaint(Handle);
        set => NativeView.ulViewSetNeedsPaint(Handle, value);
    }

    public ULSurface GetSurface()
    {
        return new ULSurface(new ULSurfaceHandle(NativeView.ulViewGetSurface(Handle), false));
    }

    public void Reload()
    {
        NativeView.ulViewReload(Handle);
    }

    public void Stop()
    {
        NativeView.ulViewStop(Handle);
    }

    public void FireKeyEvent(ULKeyEvent keyEvent)
    {
        NativeView.ulViewFireKeyEvent(Handle, keyEvent.Handle);
    }

    public void FireMouseEvent(ULMouseEvent mouseEvent)
    {
        NativeView.ulViewFireMouseEvent(Handle, mouseEvent.Handle);
    }

    public void FireScrollEvent(ULScrollEvent scrollEvent)
    {
        NativeView.ulViewFireScrollEvent(Handle, scrollEvent.Handle);
    }

    public void Resize(uint width, uint height)
    {
        NativeView.ulViewResize(Handle, width, height);
    }

    public string EvaluateScript(string js, out string exception)
    {
        using var script = new ULString(js);

        IntPtr _exception;
        var resultHandle = NativeView.ulViewEvaluateScript(Handle, script.Handle, out _exception);

        string _result = new ULString(new ULStringHandle(resultHandle, false)).ToManagedString();

        if (_exception != IntPtr.Zero)
        {
            exception = new ULString(new ULStringHandle(_exception, false)).ToManagedString();
        }
        else
        {
            exception = "";
        }

        return _result;
    }


    #region Events manager
    private void TitleChangedEvent(IntPtr userData, IntPtr view, IntPtr title)
    {
        OnTitleChanged?.Invoke(new ULString(new ULStringHandle(title, false)).ToManagedString());
    }

    private void URLChangedEvent(IntPtr userData, IntPtr view, IntPtr url)
    {
        OnURLChanged?.Invoke(new ULString(new ULStringHandle(url, false)).ToManagedString());
    }

    private void BeginLoadingEvent(IntPtr userData, IntPtr view)
    {
        OnBeginLoading?.Invoke();
    }

    private void FinishLoadingEvent(IntPtr userData, IntPtr view)
    {
        OnFinishLoading?.Invoke();
    }

    private void DOMReadyEvent(IntPtr userData, IntPtr view)
    {
        OnDOMReady?.Invoke();
    }

    private void LoadFailedEvent(IntPtr userData, IntPtr view, IntPtr url, IntPtr description, int errorCode)
    {
        OnLoadFailed?.Invoke(new ULString(new ULStringHandle(url, false)).ToManagedString(), new ULString(new ULStringHandle(description, false)).ToManagedString(), errorCode);
    }

    private void AddConsoleMessageEvent(IntPtr userData, IntPtr view, ULMessageSource source, ULMessageLevel level, IntPtr message, uint line_number, uint column_number, IntPtr source_id)
    {
        OnMessageConsole?.Invoke(source, level, new ULString(new ULStringHandle(message, false)).ToManagedString(), line_number, column_number, new ULString(new ULStringHandle(source_id, false)).ToManagedString());
    }
    #endregion

    public void Dispose()
    {
        if (Handle.IsClosed)
            return;
        NativeView.ulViewSetChangeTitleCallback(Handle, null, IntPtr.Zero);
        NativeView.ulViewSetChangeURLCallback(Handle, null, IntPtr.Zero);
        NativeView.ulViewSetBeginLoadingCallback(Handle, null, IntPtr.Zero);
        NativeView.ulViewSetFinishLoadingCallback(Handle, null, IntPtr.Zero);
        NativeView.ulViewSetDOMReadyCallback(Handle, null, IntPtr.Zero);
        NativeView.ulViewSetFailLoadingCallback(Handle, null, IntPtr.Zero);
        NativeView.ulViewSetAddConsoleMessageCallback(Handle, null, IntPtr.Zero);
        Handle.Dispose();
    }
}