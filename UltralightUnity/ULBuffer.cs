using System;
using System.Runtime.InteropServices;
using UltralightUnity.Enums;
using UltralightUnity.Handles;
using UltralightUnity.Native;

namespace UltralightUnity;

public sealed class ULBuffer : IDisposable
{
    internal ULBufferHandle Handle { get; }
    public Action<IntPtr, IntPtr> OnDestroyBuffer;
    private NativeBuffer.ulDestroyBufferCallback? _destroyCb;

    public ULBuffer(IntPtr ptr, uint size)
    {
        _destroyCb = DestroyEvent;
        Handle = NativeBuffer.ulCreateBuffer(ptr, size, IntPtr.Zero, _destroyCb);
    }

    private void DestroyEvent(IntPtr userData, IntPtr data)
    {
        OnDestroyBuffer?.Invoke(userData, data);
    }

    public void Dispose()
    {
        Console.WriteLine("Disposing buffer ");
        Handle.Dispose();
    }
}
