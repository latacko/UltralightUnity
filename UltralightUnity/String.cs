using System;
using System.Runtime.InteropServices;
using System.Text;
using UltralightUnity.Native;

namespace UltralightUnity;

public sealed class String : IDisposable
{
    internal StringHandle Handle { get; }
    private bool _disposed;

    public String(string text)
    {
        Handle = new StringHandle(text);
    }

    public string ToManagedString()
    {
        if (Handle.IsInvalid || Handle.Length == UIntPtr.Zero)
            return string.Empty;

        int length = (int)Handle.Length;

        byte[] buffer = new byte[length];
        Marshal.Copy(Handle.Pointer, buffer, 0, length);

        return Encoding.UTF8.GetString(buffer);
    }

    public override string ToString()
    {
        return ToManagedString();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        Handle.Dispose();
        GC.SuppressFinalize(this);
    }
}