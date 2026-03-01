using System;
using System.Runtime.InteropServices;
using UltralightUnity.Native;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct NativeFileSystem
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool ULFileExistsCallback(IntPtr pathPtr);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr ULGetMimeTypeCallback(IntPtr pathPtr);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr ULGetCharsetCallback(IntPtr pathPtr);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr ULOpenFileCallback(IntPtr pathPtr);

    internal ULFileExistsCallback file_exists;
    internal ULGetMimeTypeCallback get_file_mime_type;
    internal ULGetCharsetCallback get_file_charset;
    internal ULOpenFileCallback open_file;
}