using System.Runtime.InteropServices;

namespace UltralightSharedClasses.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct InputTextEvent
    {
        public uint character;
    }
}