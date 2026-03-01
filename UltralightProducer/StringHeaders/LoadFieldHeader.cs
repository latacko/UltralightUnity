using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct LoadFieldHeader
{
    public ulong frameId;
    public byte isMainFrame;
    public uint errorCode;
}