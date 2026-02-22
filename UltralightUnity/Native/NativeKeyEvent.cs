using System;
using System.Runtime.InteropServices;
using UltralightUnity.Enums;
using UltralightUnity.Handles;

namespace UltralightUnity.Native;

internal class NativeKeyEvent
{
    [DllImport(NativeLib.LibUltralight)]
    public static extern ULKeyEventHandle ulCreateKeyEvent(ULKeyEventType type, uint modifiers, int virtual_key_code, int native_key_code, ULStringHandle text, ULStringHandle unmodified_text, [MarshalAs(UnmanagedType.I1)] bool is_keypad, [MarshalAs(UnmanagedType.I1)] bool is_auto_repeat, [MarshalAs(UnmanagedType.I1)] bool is_system_key);
    [DllImport(NativeLib.LibUltralight)]
    public static extern ULKeyEventHandle ulCreateKeyEvent(ULKeyEventType type, uint modifiers, int virtual_key_code, int native_key_code, IntPtr text, IntPtr unmodified_text, [MarshalAs(UnmanagedType.I1)] bool is_keypad, [MarshalAs(UnmanagedType.I1)] bool is_auto_repeat, [MarshalAs(UnmanagedType.I1)] bool is_system_key);

    // ------------------------------------------------------------
    // Windows-specific key event
    // ------------------------------------------------------------
    // [DllImport(NativeLib.LibUltralight)]
    // public static extern ULKeyEventHandle ulCreateKeyEventWindows(ULKeyEventType type, UIntPtr wparam, IntPtr lparam, [MarshalAs(UnmanagedType.I1)] bool is_system_key);

    // ------------------------------------------------------------
    // Destroy key event
    // ------------------------------------------------------------
    [DllImport(NativeLib.LibUltralight)]
    public static extern void ulDestroyKeyEvent(IntPtr evt);
}