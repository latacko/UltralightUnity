using System;
using System.Runtime.InteropServices;
using UltralightUnity.HandlesJs;
using JSPropertyAttributes = System.UInt32;
namespace UltralightUnity.NativeJs
{
    public class NativeJSObjectRef
    {

        const string LibUltralight = NativeLib.LibUltralight;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr JSObjectCallAsFunctionCallback(IntPtr ctx, IntPtr function, IntPtr thisObject, nint argumentCount, IntPtr arguments, out IntPtr exception);

        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        internal static extern JSObjectRef JSObjectMakeFunctionWithCallback(JSContextRef ctx, JSStringRef name, JSObjectCallAsFunctionCallback callAsFunction);

        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        internal static extern JSValueRef JSObjectCallAsFunction(JSContextRef ctx, JSObjectRef objectRef, IntPtr thisObject, nuint argumentCount, IntPtr[]? arguments, out JSValueRef exception);

        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern void JSObjectSetProperty(JSContextRef ctx, JSObjectRef obj, JSStringRef propertyName, JSValueRef value, JSPropertyAttributes attributes, out JSValueRef exception);

        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern JSValueRef JSObjectGetProperty(JSContextRef ctx, JSObjectRef obj, JSStringRef propertyName, out JSValueRef exception);

        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern JSObjectRef JSObjectMake(JSContextRef ctx, JSClassRef jsClass, IntPtr data);

        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern JSPropertyNameArrayRef JSObjectCopyPropertyNames(JSContextRef ctx, JSObjectRef objectRef);

        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern nuint JSPropertyNameArrayGetCount(JSPropertyNameArrayRef array);

        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern JSStringRef JSPropertyNameArrayGetNameAtIndex(JSPropertyNameArrayRef array, nuint index);

        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern void JSPropertyNameArrayRelease(IntPtr array);

        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool JSObjectIsFunction(JSContextRef ctx, JSObjectRef objectRef);
    }
}