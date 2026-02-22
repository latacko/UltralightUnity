using System;
using System.Diagnostics.Contracts;
using Microsoft.Win32.SafeHandles;
using UltralightUnity.Native;

namespace UltralightUnity.Handles;

internal sealed class ULBitmapHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private ULBitmapHandle() : base(true) { }
    internal ULBitmapHandle(IntPtr intPtr, bool dispose) : base(dispose)
    {
        SetHandle(intPtr);
    }
    
    // public void SetOwnsInternal(bool owns)
    // {
    //     owns = 
    // }
    protected override bool ReleaseHandle()
    {
        Console.WriteLine("Destroying bitmap");
        NativeBitmap.ulDestroyBitmap(handle);
        return true;
    }
}