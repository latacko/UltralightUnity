using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct KeyEvent
{
    public uint type;   // 1 = down, 2 = up
    public UltralightUnity.KeyCode key;    // key code
    public uint is_keypad;
}