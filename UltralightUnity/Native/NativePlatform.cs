using System;
using System.Runtime.InteropServices;

namespace UltralightUnity.Native;

internal static class NativePlatform
{
    private const string LibAppCore = "AppCore";

    [DllImport(LibAppCore)]
    internal static extern void ulEnablePlatformFontLoader();

    [DllImport(LibAppCore)]
    internal static extern void ulEnablePlatformFileSystem(ULStringHandle baseDir);

    [DllImport(NativeLib.LibUltralight)]
    internal static extern void ulPlatformSetFileSystem(NativeFileSystem fileSystem);

    [DllImport(LibAppCore)]
    internal static extern void ulEnableDefaultLogger(ULStringHandle log_path);
}
