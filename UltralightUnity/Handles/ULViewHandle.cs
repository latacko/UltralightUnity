using System;
using Microsoft.Win32.SafeHandles;
using UltralightUnity.Native;

namespace UltralightUnity.Handles;

internal sealed class ULViewHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private ULViewHandle() : base(false) { }
    internal ULViewHandle(IntPtr handle) : base(false) { this.handle = handle; }

    protected override bool ReleaseHandle()
    {
        NativeView.ulDestroyView(handle);
        return true;
    }
}