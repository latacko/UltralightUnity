using System.Runtime.InteropServices;

namespace UltralightSharedClasses.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Header
    {
        public uint magic;

        /* Screen size */
        public uint ScreenWidth;
        public uint ScreenHeight;

        /* offsets */
        public uint requestViewOffset;
        public uint destroyViewOffset;

        /* Request View */
        public uint RequestViewEventWrite;
        public uint RequestViewEventRead;

        /* Destroy View */
        public uint DestroyViewEventWrite;
        public uint DestroyViewEventRead;

        /* Frames */
        public uint frameCounter;
        public uint requestCounter;
    }
}