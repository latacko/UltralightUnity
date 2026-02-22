using System;
using UltralightUnity;
using UltralightUnity.Enums;
using UltralightUnity.Handles;
using UltralightUnity.Native;

public class ULMouseEvent: IDisposable
{
    internal ULMouseEventHandle Handle { get; }

    public ULMouseEvent(ULMouseEventType type, int x, int y, ULMouseButton button)
    {
        Handle = NativeMouseEvent.ulCreateMouseEvent(type, x, y, button);
    }

    public void Dispose()
    {
        Handle.Dispose();
    }
}