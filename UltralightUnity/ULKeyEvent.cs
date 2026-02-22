using System;
using UltralightUnity;
using UltralightUnity.Enums;
using UltralightUnity.Handles;
using UltralightUnity.Native;

public class ULKeyEvent : IDisposable
{
    internal ULKeyEventHandle Handle { get; }

    public ULKeyEvent(ULKeyEventType type, ULKeyEventModifiers modifiers, int virtual_key_code, int native_key_code, string text, string unmodified_text, bool is_keypad, bool is_auto_repeat, bool is_system_key)
    {
        using var _text = new ULString(text);
        using var _unmodified_text = new ULString(unmodified_text);
        Handle = NativeKeyEvent.ulCreateKeyEvent(type, (uint)modifiers, virtual_key_code, native_key_code, _text.Handle, _unmodified_text.Handle, is_keypad, is_auto_repeat, is_system_key);
    }

    public void Dispose()
    {
        Handle.Dispose();
    }

    public static ULKeyEvent KeyDown(int virtualKey, int nativeKey, bool isAutoRepeat = false, ULKeyEventModifiers modifiers = 0)
    {
        return new ULKeyEvent(
            ULKeyEventType.RawKeyDown,
            modifiers,
            virtualKey,
            nativeKey,
            string.Empty,
            string.Empty,
            false,
            isAutoRepeat,
            false);
    }

    public static ULKeyEvent KeyUp(int virtualKey, int nativeKey, bool isAutoRepeat = false, ULKeyEventModifiers modifiers = 0)
    {
        return new ULKeyEvent(
            ULKeyEventType.KeyUp,
            modifiers,
            virtualKey,
            nativeKey,
            string.Empty,
            string.Empty,
            false,
            isAutoRepeat,
            false);
    }

    public static ULKeyEvent Character(char c, ULKeyEventModifiers modifiers = 0)
    {
        return new ULKeyEvent(
            ULKeyEventType.Char,
            modifiers,
            0,
            0,
            c.ToString(),
            c.ToString(),
            false,
            false,
            false);
    }
}