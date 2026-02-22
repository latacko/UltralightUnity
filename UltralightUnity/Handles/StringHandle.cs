using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace UltralightUnity.Native;

internal sealed class StringHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    /// <summary>
    /// Length in bytes (without null terminator)
    /// </summary>
    public UIntPtr Length { get; }

    /// <summary>
    /// Pointer to UTF-8 const char* (null terminated)
    /// </summary>
    public IntPtr Pointer => handle;

    public StringHandle() : base(true) { }

    public StringHandle(string text) : base(true)
    {
        if (text == null)
        {
            SetHandle(IntPtr.Zero);
            Length = UIntPtr.Zero;
            return;
        }

        byte[] utf8 = System.Text.Encoding.UTF8.GetBytes(text);

        IntPtr buffer = Marshal.AllocHGlobal(utf8.Length + 1);
        Marshal.Copy(utf8, 0, buffer, utf8.Length);
        Marshal.WriteByte(buffer + utf8.Length, 0);

        SetHandle(buffer);
        Length = (UIntPtr)utf8.Length;
    }

    protected override bool ReleaseHandle()
    {
        if (handle != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(handle);
        }
        return true;
    }
}
