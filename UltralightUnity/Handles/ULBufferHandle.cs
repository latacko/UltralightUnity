using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace UltralightUnity.Native;

internal sealed class ULBufferHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private ULBufferHandle() : base(false) { }

    protected override bool ReleaseHandle()
    {
        NativeBuffer.ulDestroyBuffer(handle);
        return true;
    }
}