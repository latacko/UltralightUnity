using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct InputTextEvent
{
    public uint character;
}