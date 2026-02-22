using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace UltralightUnity.Native;

internal sealed class ULRendererHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    public ULRendererHandle() : base(true)
    {
    }

    protected override bool ReleaseHandle()
    {
        NativeRenderer.ulDestroyRenderer(handle);
        return true;
    }
}
