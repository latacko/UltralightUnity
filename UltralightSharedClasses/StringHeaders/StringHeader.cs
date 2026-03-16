using System.Runtime.InteropServices;

namespace UltralightSharedClasses.StringHeaders
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StringHeader
    {
        public byte isThereNextString;
        public int length;
        public long offset;
    }
}