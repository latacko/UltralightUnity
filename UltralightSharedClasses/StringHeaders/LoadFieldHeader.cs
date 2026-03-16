using System.Runtime.InteropServices;

namespace UltralightSharedClasses.StringHeaders
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LoadFieldHeader
    {
        public ulong frameId;
        public byte isMainFrame;
        public uint errorCode;
    }
}