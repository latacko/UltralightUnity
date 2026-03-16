using System.Runtime.InteropServices;

namespace UltralightSharedClasses.FileSystemStructs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FileOpenId
    {
        public uint file_id;
        public uint path_id;
    }
}