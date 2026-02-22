using System;
using System.Runtime.InteropServices;
using UltralightUnity.Enums;
using UltralightUnity.Handles;

namespace UltralightUnity.Native;

internal class NativeMouseEvent
{
    [DllImport(NativeLib.LibUltralight)]
    public static extern ULMouseEventHandle ulCreateMouseEvent(ULMouseEventType type, int x, int y, ULMouseButton button);

    [DllImport(NativeLib.LibUltralight)]
    public static extern void ulDestroyMouseEvent(IntPtr evt);
}