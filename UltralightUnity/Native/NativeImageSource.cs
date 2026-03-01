using System;
using System.Runtime.InteropServices;
using UltralightUnity.Handles;
using UltralightUnity.Native;

internal static class NativeImageSource {
    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULImageSourceHandle ulCreateImageSourceFromBitmap(ULBitmapHandle bitmapHandle);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulDestroyImageSource(IntPtr imageSourceHandle);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulImageSourceInvalidate(ULImageSourceHandle imageSourceHandle);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulImageSourceProviderAddImageSource(ULStringHandle id, ULImageSourceHandle imageSourceHandle);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulImageSourceProviderRemoveImageSource(ULStringHandle id);
}