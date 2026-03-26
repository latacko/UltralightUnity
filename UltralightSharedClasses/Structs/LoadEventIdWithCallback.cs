using System.Runtime.InteropServices;

namespace UltralightSharedClasses.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LoadEventIdWithCallback
    {
        public uint id;
        public uint idCallback;
    }
}