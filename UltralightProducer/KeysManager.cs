using UltralightSharedClasses.Enums;
using UltralightSharedClasses.Structs;
using UltralightUnity;
using UltralightUnity.Enums;

public static unsafe class KeysManager
{
    public static ULKeyEventModifiers modifiers { get; private set; }
    public static void ReadEvent(KeyEvent* ev, ULView view)
    {
        switch (ev->type)
        {
            case 1:
                KeyDown(ev, view);
                break;
            case 2:
                KeyUp(ev, view);
                break;
        }
    }

    static void KeyDown(KeyEvent* keyevent, ULView view)
    {

        if (keyevent->key == KeyCode.LeftControl)
        {
            modifiers |= ULKeyEventModifiers.CtrlKey;
        }
        else if (keyevent->key == KeyCode.LeftAlt)
        {
            modifiers |= ULKeyEventModifiers.AltKey;
        }
        else if (keyevent->key == KeyCode.LeftShift)
        {
            modifiers |= ULKeyEventModifiers.ShiftKey;
        }
        int key = UltralightKeyMap.ToVirtualKey(keyevent->key);
        view.FireKeyEvent(ULKeyEvent.KeyDown(key, key, false, modifiers));
    }

    static void KeyUp(KeyEvent* keyevent, ULView view)
    {

        if (keyevent->key == KeyCode.LeftControl)
        {
            modifiers &= ~ULKeyEventModifiers.CtrlKey;
        }
        else if (keyevent->key == KeyCode.LeftAlt)
        {
            modifiers &= ~ULKeyEventModifiers.AltKey;
        }
        else if (keyevent->key == KeyCode.LeftShift)
        {
            modifiers &= ~ULKeyEventModifiers.ShiftKey;
        }
        int key = UltralightKeyMap.ToVirtualKey(keyevent->key);
        view.FireKeyEvent(ULKeyEvent.KeyUp(key, key, false, modifiers));
    }

    public static void InputText(InputTextEvent* keyevent, ULView view)
    {
        view.FireKeyEvent(ULKeyEvent.Character((char)keyevent->character, modifiers));
    }

}

public static class UltralightKeyMap
{
    public static int ToVirtualKey(this KeyCode key)
    {
        return key switch
        {
            // letters (DOM uses uppercase 65-90)
            KeyCode.A => 65,
            KeyCode.B => 66,
            KeyCode.C => 67,
            KeyCode.D => 68,
            KeyCode.E => 69,
            KeyCode.F => 70,
            KeyCode.G => 71,
            KeyCode.H => 72,
            KeyCode.I => 73,
            KeyCode.J => 74,
            KeyCode.K => 75,
            KeyCode.L => 76,
            KeyCode.M => 77,
            KeyCode.N => 78,
            KeyCode.O => 79,
            KeyCode.P => 80,
            KeyCode.Q => 81,
            KeyCode.R => 82,
            KeyCode.S => 83,
            KeyCode.T => 84,
            KeyCode.U => 85,
            KeyCode.V => 86,
            KeyCode.W => 87,
            KeyCode.X => 88,
            KeyCode.Y => 89,
            KeyCode.Z => 90,

            // numbers (DOM keycodes)
            KeyCode.Alpha0 => 48,
            KeyCode.Alpha1 => 49,
            KeyCode.Alpha2 => 50,
            KeyCode.Alpha3 => 51,
            KeyCode.Alpha4 => 52,
            KeyCode.Alpha5 => 53,
            KeyCode.Alpha6 => 54,
            KeyCode.Alpha7 => 55,
            KeyCode.Alpha8 => 56,
            KeyCode.Alpha9 => 57,

            // specials
            KeyCode.Return => 13,
            KeyCode.Backspace => 8,
            KeyCode.Tab => 9,
            KeyCode.Escape => 27,
            KeyCode.Space => 32,

            KeyCode.LeftArrow => 37,
            KeyCode.UpArrow => 38,
            KeyCode.RightArrow => 39,
            KeyCode.DownArrow => 40,

            _ => (int)key
        };
    }
}