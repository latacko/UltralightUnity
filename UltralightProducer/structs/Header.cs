using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct Header
{
    public uint magic;

    /* offsets */
    public uint mouseOffset;
    public uint keyOffset;
    public uint textOffset;
    public uint frameOffset;


    /* images */
    public uint width;
    public uint height;
    public uint bufferSize;
    public uint writeIndex;
    public uint frameCounter;
    public uint requestCounter;
    public uint resizeCounter;

    /* mouse */
    public uint buttonEventWrite;
    public uint buttonEventRead;

    /* key */
    public uint keyEventWrite;
    public uint keyEventRead;

    /* text */
    public uint inputTextEventWrite;
    public uint inputTextEventRead;
}