using System;
using System.Runtime.InteropServices;
using UltralightUnity.Enums;
using UltralightUnity.Handles;

namespace UltralightUnity.Native;

internal static class NativeBitmap
{
    const string LibUltralight = NativeLib.LibUltralight;

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULBitmapHandle ulCreateEmptyBitmap();

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULBitmapHandle ulCreateBitmap(uint width, uint height, ULBitmapFormat format);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULBitmapHandle ulCreateBitmapFromPixels(uint width, uint height, ULBitmapFormat format, uint row_bytes, IntPtr pixels, UIntPtr size, bool should_copy);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULBitmapHandle ulCreateBitmapFromCopy(ULBitmapHandle existing);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulDestroyBitmap(IntPtr bitmap);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ulBitmapGetWidth(ULBitmapHandle bitmap);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ulBitmapGetHeight(ULBitmapHandle bitmap);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULBitmapFormat ulBitmapGetFormat(ULBitmapHandle bitmap);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ulBitmapGetBpp(ULBitmapHandle bitmap);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ulBitmapGetRowBytes(ULBitmapHandle bitmap);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern UIntPtr ulBitmapGetSize(ULBitmapHandle bitmap);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool ulBitmapOwnsPixels(ULBitmapHandle bitmap);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ulBitmapLockPixels(ULBitmapHandle bitmap);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulBitmapUnlockPixels(ULBitmapHandle bitmap);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ulBitmapRawPixels(ULBitmapHandle bitmap);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool ulBitmapIsEmpty(ULBitmapHandle bitmap);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulBitmapErase(ULBitmapHandle bitmap);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool ulBitmapWritePNG(ULBitmapHandle bitmap, StringHandle path);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulBitmapSwapRedBlueChannels(ULBitmapHandle bitmap);
}