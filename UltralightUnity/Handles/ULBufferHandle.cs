using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace UltralightUnity.Native;

internal sealed class ULBufferHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private ULBufferHandle() : base(false) { }

    protected override bool ReleaseHandle()
    {
        Console.WriteLine("buffer_test1");
        NativeBuffer.ulDestroyBuffer(handle);
        Console.WriteLine("buffer_test2");
        return true;
    }
}