using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FileHeader
{
    public int length;
    public long offset;
}