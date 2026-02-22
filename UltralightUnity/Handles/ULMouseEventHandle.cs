using System;
using System.Diagnostics.Contracts;
using Microsoft.Win32.SafeHandles;
using UltralightUnity.Native;

namespace UltralightUnity.Handles;

internal sealed class ULMouseEventHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private ULMouseEventHandle() : base(true) { }

    protected override bool ReleaseHandle()
    {
        NativeMouseEvent.ulDestroyMouseEvent(handle);
        return true;
    }
}