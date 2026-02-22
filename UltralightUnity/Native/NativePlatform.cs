using System;
using System.Runtime.InteropServices;

namespace UltralightUnity.Native;

internal static class NativePlatform
{
    private const string LibUltralight = "AppCore";

    [DllImport(LibUltralight)]
    internal static extern void ulEnablePlatformFontLoader();

    [DllImport(LibUltralight)]
    internal static extern void ulEnablePlatformFileSystem(ULStringHandle baseDir);

    [DllImport(LibUltralight)]
    internal static extern void ulEnableDefaultLogger(ULStringHandle log_path);
}
