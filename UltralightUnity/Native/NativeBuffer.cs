using System;
using System.Runtime.InteropServices;
using UltralightUnity.DataTypes;
using UltralightUnity.Handles;

namespace UltralightUnity.Native;

internal static class NativeBuffer
{
    const string LibUltralight = NativeLib.LibUltralight;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void ulDestroyBufferCallback(IntPtr userData, IntPtr data);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern ULBufferHandle ulCreateBuffer(IntPtr data, uint size, IntPtr user_data, ulDestroyBufferCallback destruction_callback);

    [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ulDestroyBuffer(IntPtr buffer);
}