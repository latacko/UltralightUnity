using System.Runtime.InteropServices;

namespace UltralightSharedClasses.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ResizeEvent
    {
        public uint width;
        public uint height;
    }
}