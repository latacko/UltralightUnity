using System;
using System.Diagnostics.Contracts;
using Microsoft.Win32.SafeHandles;
using UltralightUnity.Native;

namespace UltralightUnity.Handles;

internal sealed class ULKeyEventHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal ULKeyEventHandle() : base(true) { }

    protected override bool ReleaseHandle()
    {
        NativeKeyEvent.ulDestroyKeyEvent(handle);
        return true;
    }
}