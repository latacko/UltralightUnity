using System;
using System.Runtime.InteropServices;
using UltralightUnity.DataTypes;
using UltralightUnity.Handles;

namespace UltralightUnity.Native;

internal static class NativeSurface
{
    const string LibUltralight = NativeLib.LibUltralight;

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ulSurfaceGetWidth(ULSurfaceHandle surface);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ulSurfaceGetHeight(ULSurfaceHandle surface);


    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ulSurfaceGetRowBytes(ULSurfaceHandle surface);


    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern nuint ulSurfaceGetSize(ULSurfaceHandle surface);


    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ulSurfaceLockPixels(ULSurfaceHandle surface);


    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulSurfaceUnlockPixels(ULSurfaceHandle surface);


    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULIntRect ulSurfaceGetDirtyBounds(ULSurfaceHandle surface);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULIntRect ulSurfaceSetDirtyBounds(ULSurfaceHandle surface, ULIntRect bounds);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulSurfaceClearDirtyBounds(ULSurfaceHandle surface);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ulBitmapSurfaceGetBitmap(ULSurfaceHandle surface);
}