using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct Header
{
    public uint magic;

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