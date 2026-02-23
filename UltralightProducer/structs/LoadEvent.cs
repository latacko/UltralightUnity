using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct LoadEvent
{
    public uint id;
    public byte type; // 0-url | 1-html
    public byte thereIsNextChunk;
    public uint length;
    public fixed byte data[ChunksData.MAX_BYTES_PER_CHUNK]; 
}