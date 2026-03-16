using System.Runtime.InteropServices;

namespace UltralightSharedClasses.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DestroyViewEvent
    {
        public uint id;
    }
}