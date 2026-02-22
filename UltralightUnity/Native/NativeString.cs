using System;
using System.Runtime.InteropServices;

namespace UltralightUnity.Native;

internal static class NativeString
{

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULStringHandle ulCreateString(IntPtr str);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULStringHandle ulCreateStringUTF8(IntPtr str, UIntPtr len);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULStringHandle ulCreateStringUTF16(IntPtr str, UIntPtr len);


    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ulDestroyString(IntPtr str);


    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ulStringGetData(ULStringHandle str);

    [DllImport(NativeLib.LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern UIntPtr ulStringGetLength(ULStringHandle str);

}