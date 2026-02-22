using System;
using System.Runtime.InteropServices;
using UltralightUnity.Handles;

namespace UltralightUnity.Native;

internal static class NativeViewConfig
{
    const string LibUltralight = NativeLib.LibUltralight;

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULViewConfigHandle ulCreateViewConfig();

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulDestroyViewConfig(IntPtr config);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewConfigSetDisplayId(
        ULViewConfigHandle config,
        uint display_id);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewConfigSetIsAccelerated(
        ULViewConfigHandle config,
        bool is_accelerated);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewConfigSetIsTransparent(
        ULViewConfigHandle config,
        bool is_transparent);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewConfigSetInitialDeviceScale(
        ULViewConfigHandle config,
        double initial_device_scale);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewConfigSetInitialFocus(
        ULViewConfigHandle config,
        bool is_focused);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewConfigSetEnableImages(
        ULViewConfigHandle config,
        bool enabled);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewConfigSetEnableJavaScript(
        ULViewConfigHandle config,
        bool enabled);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewConfigSetFontFamilyStandard(
        ULViewConfigHandle config,
        ULStringHandle font_name);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewConfigSetFontFamilyFixed(
        ULViewConfigHandle config,
        ULStringHandle font_name);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulViewConfigSetFontFamilySerif(
        ULViewConfigHandle config,
        ULStringHandle font_name);
}