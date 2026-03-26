using System.Runtime.InteropServices;
using UltralightUnity.HandlesJs;

namespace UltralightUnity.NativeJs
{
    public class NativeJSContextRef
    {
        const string LibUltralight = NativeLib.LibUltralight;
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern JSObjectRef JSContextGetGlobalObject(JSContextRef ctx);
    }
}