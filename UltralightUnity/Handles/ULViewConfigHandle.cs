using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace UltralightUnity.Native;

internal sealed class ULViewConfigHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private ULViewConfigHandle() : base(true) { }

    protected override bool ReleaseHandle()
    {
        NativeViewConfig.ulDestroyViewConfig(handle);
        return true;
    }
}