using System.Runtime.InteropServices;

namespace UltralightSharedClasses.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RequestViewEvent
    {
        public uint width;
        public uint height;
        public byte isTransparent;
        public uint id;
    }
}