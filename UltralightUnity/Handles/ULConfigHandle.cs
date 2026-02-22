using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using UltralightUnity.Native;

namespace UltralightUnity.Handles;

internal sealed class ULConfigHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    
    public ULConfigHandle() : base(true)
    {
    }
    
    protected override bool ReleaseHandle()
    {
        NativeConfig.ulDestroyConfig(handle);
        return true;
    }
}
