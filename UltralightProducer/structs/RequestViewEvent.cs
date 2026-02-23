using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct RequestViewEvent
{
    public uint width;
    public uint height;
    public byte isTransparent;
    public uint id;
}