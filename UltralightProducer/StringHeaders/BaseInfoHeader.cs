using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct BaseInfoHeader
{
    public EventType type;
    public uint stringsCount;
    public uint DetailHeaderOffset;
}