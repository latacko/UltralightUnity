using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct LoadFieldHeader
{
    public byte toDispose;
    public uint length;
    public long offset;
    public long offset2;
    public uint errorCode;
}