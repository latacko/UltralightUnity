using System;
using Microsoft.Win32.SafeHandles;
using UltralightUnity.Native;

namespace UltralightUnity.Handles;

internal sealed class ULSurfaceHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private ULSurfaceHandle() : base(false) { }
    internal ULSurfaceHandle(IntPtr intPtr, bool dispose) : base(dispose)
    {
        SetHandle(intPtr);
    }

    protected override bool ReleaseHandle()
    {
        return true;
    }
}