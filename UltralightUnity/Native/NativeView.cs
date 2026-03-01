using System;
using System.Runtime.InteropServices;
using UltralightUnity.Handles;
using UltralightUnity.Native;

namespace UltralightUnity.Native;

internal static class NativeView
{
    const string LibUltralight = NativeLib.LibUltralight;

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULViewConfigHandle ulCreateViewConfig();

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulDestroyViewConfig(IntPtr config);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULViewHandle ulCreateView(ULRendererHandle renderer, uint width, uint height, ULViewConfigHandle config, ULSessionHandle session);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulDestroyView(IntPtr view);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulViewLoadURL(ULViewHandle view, ULStringHandle url);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulViewLoadHTML(ULViewHandle view, ULStringHandle html);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULStringHandle ulViewGetURL(ULViewHandle view);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULStringHandle ulViewGetTitle(ULViewHandle view);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulViewSetNeedsPaint(ULViewHandle view, bool needs_paint);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool ulViewGetNeedsPaint(ULViewHandle view);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ulViewGetSurface(ULViewHandle view);

    [DllImport(LibUltralight)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ulViewIsTransparent(ULViewHandle view);

    [DllImport(LibUltralight)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ulViewIsLoading(ULViewHandle view);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulViewReload(ULViewHandle view);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulViewStop(ULViewHandle view);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool ulViewHasInputFocus(ULViewHandle view);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulViewFireKeyEvent(ULViewHandle view, ULKeyEventHandle keyEventHandle);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulViewFireMouseEvent(ULViewHandle view, ULMouseEventHandle mouseEventHandle);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulViewFireScrollEvent(ULViewHandle view, ULScrollEventHandle scrollEventHandle);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulViewResize(ULViewHandle view, uint width, uint height);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulViewCreateLocalInspectorView(ULViewHandle view);

    [DllImport(LibUltralight)]
    public static extern IntPtr ulViewEvaluateScript(ULViewHandle view, ULStringHandle js_string, out IntPtr exception);

    #region Events

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void ChangeTitleCallback(IntPtr userData, IntPtr view, IntPtr title);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void ChangeURLCallback(IntPtr userData, IntPtr view, IntPtr url);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void SimpleViewCallback(IntPtr userData, IntPtr view);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void AddConsoleMessageCallback(IntPtr userData, IntPtr view, ULMessageSource source, ULMessageLevel level, IntPtr message, uint line_number, uint column_number, IntPtr source_id);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void FailLoadingCallback(IntPtr userData, IntPtr caller, ulong frameId, [MarshalAs(UnmanagedType.I1)] bool isMainFrame, IntPtr url, IntPtr description, IntPtr errorDomain, int errorCode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate ULViewHandle ULCreateInspectorViewCallback(IntPtr userData, ULViewHandle callerView, [MarshalAs(UnmanagedType.I1)] bool isLocal, IntPtr inspected_url);


    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewSetChangeTitleCallback(ULViewHandle view, ChangeTitleCallback? callback, IntPtr userData);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewSetChangeURLCallback(ULViewHandle view, ChangeURLCallback? callback, IntPtr userData);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewSetBeginLoadingCallback(ULViewHandle view, SimpleViewCallback? callback, IntPtr userData);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewSetFinishLoadingCallback(ULViewHandle view, SimpleViewCallback? callback, IntPtr userData);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewSetDOMReadyCallback(ULViewHandle view, SimpleViewCallback? callback, IntPtr userData);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewSetFailLoadingCallback(ULViewHandle view, FailLoadingCallback? callback, IntPtr userData);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewSetAddConsoleMessageCallback(ULViewHandle view, AddConsoleMessageCallback? callback, IntPtr userData);


    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewSetCreateInspectorViewCallback(ULViewHandle view, ULCreateInspectorViewCallback? callback, IntPtr userData);
    #endregion
}