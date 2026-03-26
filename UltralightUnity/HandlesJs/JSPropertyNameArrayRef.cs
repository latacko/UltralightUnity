using System;
using Microsoft.Win32.SafeHandles;
using UltralightUnity.NativeJs;

namespace UltralightUnity.HandlesJs
{
    public class JSPropertyNameArrayRef : SafeHandleZeroOrMinusOneIsInvalid
    {
        public JSPropertyNameArrayRef() : base(true) { }
        public static readonly JSContextRef Null = new();
        protected override bool ReleaseHandle()
        {
            NativeJSObjectRef.JSPropertyNameArrayRelease(handle);
            return true;
        }
    }
}