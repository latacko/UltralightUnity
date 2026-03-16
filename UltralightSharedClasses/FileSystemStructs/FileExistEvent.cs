using System.Runtime.InteropServices;
namespace UltralightSharedClasses.FileSystemStructs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FileExistEvent
    {
        public byte set;
        public uint id;
        public byte exist;
    }
}