using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct MouseEvent
{
    public uint type;
    public int x;
    public int y;
}