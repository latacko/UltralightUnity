using System.Runtime.InteropServices;

namespace UltralightSharedClasses.StringHeaders
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BaseInfoHeader
    {
        public byte ToDispose;
        public EventType type;
        public uint stringsCount;
        public uint DetailHeaderOffset;
    }
}