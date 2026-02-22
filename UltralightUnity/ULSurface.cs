using System;
using UltralightUnity.DataTypes;
using UltralightUnity.Handles;
using UltralightUnity.Native;

namespace UltralightUnity;

public sealed class ULSurface : IDisposable
{
    internal ULSurfaceHandle Handle { get; }

    internal ULSurface(ULSurfaceHandle handle)
    {
        Handle = handle;
    }

    public uint Width => NativeSurface.ulSurfaceGetWidth(Handle);

    public uint Height => NativeSurface.ulSurfaceGetHeight(Handle);

    public uint RowBytes => NativeSurface.ulSurfaceGetRowBytes(Handle);

    /// <summary>
    /// Size in bytes of pixel buffer
    /// </summary>
    public nuint Size => NativeSurface.ulSurfaceGetSize(Handle);

    /// <summary>
    /// Lock pixel buffer and return pointer (BGRA32 premultiplied)
    /// </summary>
    public IntPtr LockPixels()
    {
        return NativeSurface.ulSurfaceLockPixels(Handle);
    }

    public void UnlockPixels()
    {
        NativeSurface.ulSurfaceUnlockPixels(Handle);
    }

    public ULIntRect DirtyBounds
    {
        get => NativeSurface.ulSurfaceGetDirtyBounds(Handle);
        set => NativeSurface.ulSurfaceSetDirtyBounds(Handle, value);
    }

    public bool IsDirty => !DirtyBounds.IsEmpty;

    public ULBitmap GetBitmap() => new(NativeSurface.ulBitmapSurfaceGetBitmap(Handle), false);

    public void ClearDirty() => NativeSurface.ulSurfaceClearDirtyBounds(Handle);

    public void Dispose() => Handle.Dispose();
}