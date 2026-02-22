using System;
using UltralightUnity;
using UltralightUnity.Enums;
using UltralightUnity.Handles;
using UltralightUnity.Native;

public class ULScrollEvent: IDisposable
{
    internal ULScrollEventHandle Handle { get; }

    public ULScrollEvent(ULScrollEventType type, int delta_x, int delta_y)
    {
        Handle = NativeScrollEvent.ulCreateScrollEvent(type, delta_x, delta_y);
    }

    public void Dispose()
    {
        Handle.Dispose();
    }
}