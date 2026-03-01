using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FileSystemHeader
{
    public uint existOffset;
    public uint fileOffset;

    /* fileExist */
    public uint fileExistWrite;
    public uint fileExistRead;

    /* open file */
    public uint openFileWrite;
    public uint openFileRead;
}