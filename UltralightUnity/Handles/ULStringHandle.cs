using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace UltralightUnity.Native;

internal sealed class ULStringHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    public ULStringHandle() : base(true)
    {
    }

    internal ULStringHandle(IntPtr existingHandle, bool ownsHandle): base(ownsHandle)
    {
        SetHandle(existingHandle);
    }


    protected override bool ReleaseHandle()
    {
        NativeString.ulDestroyString(handle);
        return true;
    }
}
