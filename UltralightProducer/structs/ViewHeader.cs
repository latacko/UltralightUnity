using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct ViewHeader
{
    public uint magic;

    /* offsets */
    public uint mouseOffset;
    public uint keyOffset;
    public uint textOffset;
    public uint resizeOffset;
    public uint frameOffset;
    public uint loadEventOffset;


    /* images */
    public uint width;
    public uint height;

    public uint bufferSize;
    public uint writeIndex;
    public uint resizeCounter;


    /* resize */
    public uint resizeEventWrite;
    public uint resizeEventRead;

    /* mouse */
    public uint buttonEventWrite;
    public uint buttonEventRead;

    /* key */
    public uint keyEventWrite;
    public uint keyEventRead;

    /* Load Url or html */
    public uint LoadEventWrite;
    public uint LoadViewEventRead;


    /* text */
    public uint inputTextEventWrite;
    public uint inputTextEventRead;
}