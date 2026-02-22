using UltralightUnity;
using UltralightUnity.Enums;

public static unsafe class KeysManager
{
    public static ULKeyEventModifiers modifiers { get; private set; }
    public static ULView view;
    public static void ReadEvent(KeyEvent* ev)
    {
        switch (ev->type)
        {
            case 1:
                KeyDown(ev);
                break;
            case 2:
                KeyUp(ev);
                break;
        }
    }

    static void KeyDown(KeyEvent* keyevent)
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

    static void KeyUp(KeyEvent* keyevent)
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

    public static void InputText(InputTextEvent* keyevent)
    {
        view.FireKeyEvent(ULKeyEvent.Character((char)keyevent->character, modifiers));
    }

}

public static class UltralightKeyMap
{
    public static int ToVirtualKey(this UltralightUnity.KeyCode key)
    {
        return key switch
        {
            // letters (DOM uses uppercase 65-90)
            UltralightUnity.KeyCode.A => 65,
            UltralightUnity.KeyCode.B => 66,
            UltralightUnity.KeyCode.C => 67,
            UltralightUnity.KeyCode.D => 68,
            UltralightUnity.KeyCode.E => 69,
            UltralightUnity.KeyCode.F => 70,
            UltralightUnity.KeyCode.G => 71,
            UltralightUnity.KeyCode.H => 72,
            UltralightUnity.KeyCode.I => 73,
            UltralightUnity.KeyCode.J => 74,
            UltralightUnity.KeyCode.K => 75,
            UltralightUnity.KeyCode.L => 76,
            UltralightUnity.KeyCode.M => 77,
            UltralightUnity.KeyCode.N => 78,
            UltralightUnity.KeyCode.O => 79,
            UltralightUnity.KeyCode.P => 80,
            UltralightUnity.KeyCode.Q => 81,
            UltralightUnity.KeyCode.R => 82,
            UltralightUnity.KeyCode.S => 83,
            UltralightUnity.KeyCode.T => 84,
            UltralightUnity.KeyCode.U => 85,
            UltralightUnity.KeyCode.V => 86,
            UltralightUnity.KeyCode.W => 87,
            UltralightUnity.KeyCode.X => 88,
            UltralightUnity.KeyCode.Y => 89,
            UltralightUnity.KeyCode.Z => 90,

            // numbers (DOM keycodes)
            UltralightUnity.KeyCode.Alpha0 => 48,
            UltralightUnity.KeyCode.Alpha1 => 49,
            UltralightUnity.KeyCode.Alpha2 => 50,
            UltralightUnity.KeyCode.Alpha3 => 51,
            UltralightUnity.KeyCode.Alpha4 => 52,
            UltralightUnity.KeyCode.Alpha5 => 53,
            UltralightUnity.KeyCode.Alpha6 => 54,
            UltralightUnity.KeyCode.Alpha7 => 55,
            UltralightUnity.KeyCode.Alpha8 => 56,
            UltralightUnity.KeyCode.Alpha9 => 57,

            // specials
            UltralightUnity.KeyCode.Return => 13,
            UltralightUnity.KeyCode.Backspace => 8,
            UltralightUnity.KeyCode.Tab => 9,
            UltralightUnity.KeyCode.Escape => 27,
            UltralightUnity.KeyCode.Space => 32,

            UltralightUnity.KeyCode.LeftArrow => 37,
            UltralightUnity.KeyCode.UpArrow => 38,
            UltralightUnity.KeyCode.RightArrow => 39,
            UltralightUnity.KeyCode.DownArrow => 40,

            _ => (int)key
        };
    }
}