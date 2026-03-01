using System;
using UltralightUnity.Enums;
using UltralightUnity.Handles;
using UltralightUnity.Native;

namespace UltralightUnity;

public sealed class ULImageSource : IDisposable
{
    internal ULImageSourceHandle Handle { get; }
    public Action<IntPtr, IntPtr> OnDestroyBuffer;

    public ULImageSource(ULBitmap bitmap)
    {
        Handle  = NativeImageSource.ulCreateImageSourceFromBitmap(bitmap.Handle);
    }

    public void Invalidate()
    {
        NativeImageSource.ulImageSourceInvalidate(Handle);
    }

    


    public void Dispose() {
        Handle.Dispose();
    }
}
