using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct ResizeEvent
{
    public uint width;
    public uint height;
}