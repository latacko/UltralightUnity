using System;
using System.Runtime.InteropServices;
using System.Text;
using UltralightUnity.Enums;
using UltralightUnity.Handles;
using UltralightUnity.Native;

namespace UltralightUnity;

public sealed class ULString : IDisposable
{
    internal ULStringHandle Handle { get; }

    public ULString(string str)
    {

        byte[] utf8 = Encoding.UTF8.GetBytes(str);

        IntPtr buffer = Marshal.AllocHGlobal(utf8.Length);
        Marshal.Copy(utf8, 0, buffer, utf8.Length);

        Handle = NativeString.ulCreateStringUTF8(buffer, (UIntPtr)utf8.Length);

        Marshal.FreeHGlobal(buffer);
    }

    internal ULString(ULStringHandle handle)
    {
        Handle = handle;
    }

    public string ToManagedString()
    {
        IntPtr data = NativeString.ulStringGetData(Handle);

        if (data == IntPtr.Zero)
            return string.Empty;

        int length = (int)NativeString.ulStringGetLength(Handle);

        byte[] buffer = new byte[length];
        Marshal.Copy(data, buffer, 0, length);

        return Encoding.UTF8.GetString(buffer);
    }

    public override string ToString() => ToManagedString();

    public void Dispose()
    {
        Handle.Dispose();
    }
}