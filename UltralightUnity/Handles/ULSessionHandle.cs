using System;
using Microsoft.Win32.SafeHandles;
using UltralightUnity.Native;

internal sealed class ULSessionHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal ULSessionHandle() : base(true) { }

    public static readonly ULSessionHandle Null = new ULSessionHandle();

    protected override bool ReleaseHandle()
    {
        NativeSession.ulDestroySession(handle);
        return true;
    }
}