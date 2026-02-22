using System;
using System.Runtime.InteropServices;

namespace UltralightUnity.Native;

internal static class NativeSession
{
    const string LibUltralight = NativeLib.LibUltralight;

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULSessionHandle ulCreateSession(ULRendererHandle renderer, bool is_persistent, ULStringHandle name);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulDestroySession(IntPtr session);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULSessionHandle ulDefaultSession(ULRendererHandle renderer);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool ulSessionIsPersistent(ULSessionHandle session);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULStringHandle ulSessionGetName( ULSessionHandle session);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ulSessionGetId(ULSessionHandle session);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULStringHandle ulSessionGetDiskPath(ULSessionHandle session);
}