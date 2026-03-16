using System.Runtime.InteropServices;

namespace UltralightSharedClasses.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MouseEvent
    {
        public uint type;
        public int x;
        public int y;
    }
}