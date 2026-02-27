using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct SetUpHTMLORURLHeader
{
    public SetUpType type;
}

public enum SetUpType: byte
{
    url,
    html
}