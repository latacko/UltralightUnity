using System;
using UltralightUnity.Enums;
using UltralightUnity.Handles;
using UltralightUnity.Native;

namespace UltralightUnity;

public sealed class ULBitmap : IDisposable
{
    internal ULBitmapHandle Handle { get; }

    public ULBitmap(uint width, uint height, ULBitmapFormat format)
    {
        Handle = NativeBitmap.ulCreateBitmap(width, height, format);
    }

    public static ULBitmap Empty()
    {
        return new ULBitmap(NativeBitmap.ulCreateEmptyBitmap());
    }

    internal ULBitmap(ULBitmapHandle handle)
    {
        Handle = handle;
    }

    internal ULBitmap(IntPtr handle, bool dispose = true)
    {
        Handle = new(handle, dispose);
    }

    public uint Width => NativeBitmap.ulBitmapGetWidth(Handle);

    public uint Height => NativeBitmap.ulBitmapGetHeight(Handle);

    public ULBitmapFormat Format => NativeBitmap.ulBitmapGetFormat(Handle);

    public uint BytesPerPixel => NativeBitmap.ulBitmapGetBpp(Handle);

    public uint RowBytes => NativeBitmap.ulBitmapGetRowBytes(Handle);

    public UIntPtr Size => NativeBitmap.ulBitmapGetSize(Handle);

    public bool IsEmpty => NativeBitmap.ulBitmapIsEmpty(Handle);

    public bool OwnsPixels => NativeBitmap.ulBitmapOwnsPixels(Handle);

    public IntPtr LockPixels() => NativeBitmap.ulBitmapLockPixels(Handle);

    public void UnlockPixels() => NativeBitmap.ulBitmapUnlockPixels(Handle);

    public IntPtr RawPixels() => NativeBitmap.ulBitmapRawPixels(Handle);

    public void Erase() => NativeBitmap.ulBitmapErase(Handle);

    public void SwapRedBlue() => NativeBitmap.ulBitmapSwapRedBlueChannels(Handle);

    public bool SavePng(string path)
    {
        using var _pathString = new String(path);
        return NativeBitmap.ulBitmapWritePNG(Handle, _pathString.Handle);
    }

    public void Dispose() => Handle.Dispose();
}