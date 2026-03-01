using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace UltralightUnity.Native;

internal sealed class ULImageSourceHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private ULImageSourceHandle() : base(true) { }

    protected override bool ReleaseHandle()
    {
        NativeImageSource.ulDestroyImageSource(handle);
        return true;
    }
}