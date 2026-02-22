using System;
using System.Runtime.InteropServices;
using UltralightUnity.Enums;
using UltralightUnity.Handles;

namespace UltralightUnity.Native;

internal class NativeScrollEvent
{
    [DllImport(NativeLib.LibUltralight)]
    public static extern ULScrollEventHandle ulCreateScrollEvent(ULScrollEventType type, int delta_x, int delta_y);

    [DllImport(NativeLib.LibUltralight)]
    public static extern void ulDestroyScrollEvent(IntPtr evt);
}