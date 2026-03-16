using System.Runtime.InteropServices;

namespace UltralightSharedClasses.StringHeaders
{
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
}