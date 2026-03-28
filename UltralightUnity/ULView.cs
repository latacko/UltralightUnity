using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using UltralightUnity.Handles;
using UltralightUnity.HandlesJs;
using UltralightUnity.Native;

namespace UltralightUnity;

public sealed class ULView : IDisposable
{
    public event Action<string>? OnTitleChanged;
    public event Action<string>? OnURLChanged;
    public event Action? OnBeginLoading;
    public event Action? OnFinishLoading;
    public event Action<ULView>? OnDOMReady;
    public event Action<ulong, bool, string, string, string, int>? OnLoadFailed;
    public event Action<ULMessageSource, ULMessageLevel, string, uint, uint, string> OnMessageConsole;

    public delegate void MessageEmittedEvent(string sender, string json);
    public event MessageEmittedEvent MessageEmitted;
    public event Func<bool, string, ULView> OnInspectorRequest;

    private NativeView.ChangeTitleCallback? _titleCb;
    private NativeView.ChangeURLCallback? _urlCb;
    private NativeView.SimpleViewCallback? _beginCb;
    private NativeView.SimpleViewCallback? _finishCb;
    private NativeView.SimpleViewCallback? _domReadyCb;
    private NativeView.FailLoadingCallback? _failCb;
    private NativeView.AddConsoleMessageCallback? _messageCB;
    private NativeView.ULCreateInspectorViewCallback? _inspectorCB;

    private JSObject.JSObjectCallAsFunctionCallback? _receivedPostMessageFunction;

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
        _inspectorCB = CreateInspectorViewEvent;

        NativeView.ulViewSetChangeTitleCallback(Handle, _titleCb, IntPtr.Zero);
        NativeView.ulViewSetChangeURLCallback(Handle, _urlCb, IntPtr.Zero);
        NativeView.ulViewSetBeginLoadingCallback(Handle, _beginCb, IntPtr.Zero);
        NativeView.ulViewSetFinishLoadingCallback(Handle, _finishCb, IntPtr.Zero);
        NativeView.ulViewSetDOMReadyCallback(Handle, _domReadyCb, IntPtr.Zero);
        NativeView.ulViewSetFailLoadingCallback(Handle, _failCb, IntPtr.Zero);
        NativeView.ulViewSetAddConsoleMessageCallback(Handle, _messageCB, IntPtr.Zero);
        NativeView.ulViewSetCreateInspectorViewCallback(Handle, _inspectorCB, IntPtr.Zero);

        _receivedPostMessageFunction = ReceivedPostMessage;
        OnDOMReady += AttachUltralightObject;
    }

    internal ULView(ULViewHandle handle)
    {
        Handle = handle;
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

    public JSContext LockJSContext()
    {
        return new(NativeView.ulViewLockJSContext(Handle));
    }

    public void UnlockJSContext()
    {
        NativeView.ulViewUnlockJSContext(Handle);
    }

    public void OpenInspector()
    {
        Console.WriteLine("Sending request for inspector");
        NativeView.ulViewCreateLocalInspectorView(Handle.DangerousGetHandle());
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

    public void AttachUltralightObject(ULView view)
    {
        JSContext _context = view.LockJSContext();
        JSGlobalContext _globalContext = _context.GetGlobalContext();

        var _bridgeObject = _context.ObjectMake(JSClassRef.Empty, IntPtr.Zero);
        JSObject _postMassageFunction = _context.MakeFunctionWithCallback("postMessage", _receivedPostMessageFunction!);
        _bridgeObject.SetProperty("postMessage", _postMassageFunction, 0, out _);

        _globalContext.SetProperty("ultralight", _bridgeObject, 0, out _);

        _context.EvaluateScript("window.dispatchEvent(new Event('ultralightready'));", null, null, 0, out _);

        view.UnlockJSContext();
    }

    private JSValue ReceivedPostMessage(JSContext ctx, JSObject function, JSObject thisObject, nint argumentCount, JSValue[] arguments, out JSValue? exception)
    {
        exception = null;
        Dictionary<string, object> _data = [];
        
        JSObject _obj = arguments[1].ToObject(out _);
        var _keys = _obj.GetPropertyNames();

        foreach (var key in _keys)
        {
            var _object = _obj.GetProperty(key, out _);
            if (_object.IsNumber())
                _data[key] = _object.ToNumber(out _);
            else if (_object.IsString())
                _data[key] = _object.ToString();
            else if (_object.IsBoolean())
                _data[key] = _object.ToBoolean();
            else
                Console.WriteLine("Unsuported data type for property: " + key);
        }
        string _json = Newtonsoft.Json.JsonConvert.SerializeObject(_data);
        MessageEmitted?.Invoke(arguments[0].ToString(), _json);
        return ctx.MakeNull();
    }

    public void PostMessage(string json)
    {
        JSContext _context = LockJSContext();
        _context.EvaluateScript($"window.dispatchEvent(new MessageEvent('ultralightmessage', {{ data: {json} }}));", null, null, 0, out _);
        UnlockJSContext();
    }

    #region Events manager
    private void TitleChangedEvent(IntPtr userData, IntPtr view, IntPtr title)
    {
        Console.WriteLine("Title changed: " + new ULString(new ULStringHandle(title, false)).ToManagedString());
        OnTitleChanged?.Invoke(new ULString(new ULStringHandle(title, false)).ToManagedString());
    }

    private void URLChangedEvent(IntPtr userData, IntPtr view, IntPtr url)
    {
        Console.WriteLine("URL changed: " + new ULString(new ULStringHandle(url, false)).ToManagedString());
        OnURLChanged?.Invoke(new ULString(new ULStringHandle(url, false)).ToManagedString());
    }

    private void BeginLoadingEvent(IntPtr userData, IntPtr view)
    {
        Console.WriteLine("Begin loading");
        OnBeginLoading?.Invoke();
    }

    private void FinishLoadingEvent(IntPtr userData, IntPtr view)
    {
        Console.WriteLine("Finish loading");
        OnFinishLoading?.Invoke();
    }

    private void DOMReadyEvent(IntPtr userData, IntPtr view)
    {
        Console.WriteLine("DOM ready");
        OnDOMReady?.Invoke(new ULView(new ULViewHandle(view)));
    }

    private void LoadFailedEvent(IntPtr userData, IntPtr caller, ulong frameId, bool isMainFrame, IntPtr url, IntPtr description, IntPtr errorDomain, int errorCode)
    {
        Console.WriteLine("Failed event");
        Console.WriteLine("Url pointer " + url);
        Console.WriteLine("Desc pointer " + description);
        OnLoadFailed?.Invoke(frameId, isMainFrame, new ULString(new ULStringHandle(url, false)).ToManagedString(), new ULString(new ULStringHandle(description, false)).ToManagedString(), new ULString(new ULStringHandle(errorDomain, false)).ToManagedString(), errorCode);
    }

    private void AddConsoleMessageEvent(IntPtr userData, IntPtr view, ULMessageSource source, ULMessageLevel level, IntPtr message, uint line_number, uint column_number, IntPtr source_id)
    {
        Console.WriteLine("Console event");
        OnMessageConsole?.Invoke(source, level, new ULString(new ULStringHandle(message, false)).ToManagedString(), line_number, column_number, new ULString(new ULStringHandle(source_id, false)).ToManagedString());
    }

    private ULViewHandle CreateInspectorViewEvent(IntPtr userData, IntPtr callerView, bool isLocal, IntPtr inspected_url)
    {
        Console.WriteLine("create inspector event");
        var _ulView = OnInspectorRequest?.Invoke(isLocal, new ULString(new ULStringHandle(inspected_url, false)).ToManagedString());
        if (_ulView == null)
            throw new Exception("Inspector view wasn't created");
        return _ulView.Handle;
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
        NativeView.ulViewSetCreateInspectorViewCallback(Handle, null, IntPtr.Zero);
        Handle.Dispose();
    }
}