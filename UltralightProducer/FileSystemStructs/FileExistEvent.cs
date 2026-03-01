using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct FileExistEvent
{
    public byte set;
    public uint id;
    public byte exist;
}