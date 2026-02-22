using System;
using System.Runtime.InteropServices;
using UltralightUnity.Handles;

namespace UltralightUnity.Native;

internal static class NativeConfig
{

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern ULConfigHandle ulCreateConfig();

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulDestroyConfig(IntPtr config);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulConfigSetCachePath(ULConfigHandle config, ULStringHandle cache_path);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulConfigSetResourcePathPrefix(ULConfigHandle config, ULStringHandle resource_path_prefix);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulConfigSetFaceWinding(ULConfigHandle config, byte winding); // ULFaceWinding

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulConfigSetFontHinting(ULConfigHandle config, byte font_hinting); // ULFontHinting

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulConfigSetFontGamma(ULConfigHandle config, double font_gamma);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulConfigSetUserStylesheet(ULConfigHandle config, ULStringHandle css_string);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static extern void ulConfigSetForceRepaint(ULConfigHandle config, bool enabled);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulConfigSetAnimationTimerDelay(ULConfigHandle config, double delay);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulConfigSetScrollTimerDelay(ULConfigHandle config, double delay);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulConfigSetRecycleDelay(ULConfigHandle config, double delay);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulConfigSetMemoryCacheSize(ULConfigHandle config, uint size);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulConfigSetPageCacheSize(ULConfigHandle config, uint size);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulConfigSetOverrideRAMSize(ULConfigHandle config, uint size);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulConfigSetMinLargeHeapSize(ULConfigHandle config, uint size);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulConfigSetMinSmallHeapSize(ULConfigHandle config, uint size);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulConfigSetNumRendererThreads(ULConfigHandle config, uint num_renderer_threads);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulConfigSetMaxUpdateTime(ULConfigHandle config, double max_update_time);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void ulConfigSetBitmapAlignment(ULConfigHandle config, uint bitmap_alignment);

    // Renderer creation from C API:
    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr ulCreateRenderer(ULConfigHandle config);
}