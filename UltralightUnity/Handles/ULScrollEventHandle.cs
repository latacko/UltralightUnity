using System;
using System.Diagnostics.Contracts;
using Microsoft.Win32.SafeHandles;
using UltralightUnity.Native;

namespace UltralightUnity.Handles;

internal sealed class ULScrollEventHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private ULScrollEventHandle() : base(true) { }

    protected override bool ReleaseHandle()
    {
        NativeScrollEvent.ulDestroyScrollEvent(handle);
        return true;
    }
}