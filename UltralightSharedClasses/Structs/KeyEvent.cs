using System.Runtime.InteropServices;
using UltralightSharedClasses.Enums;

namespace UltralightSharedClasses.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyEvent
    {
        public uint type;   // 1 = down, 2 = up
        public KeyCode key;    // key code
        public uint is_keypad;
    }
}