using System;
using Microsoft.Win32.SafeHandles;
using UltralightUnity.NativeJs;

namespace UltralightUnity.HandlesJs
{
    public class JSStringRef : SafeHandleZeroOrMinusOneIsInvalid
    {
        public JSStringRef() : base(false) { }
        public JSStringRef(IntPtr handle) : base(true)
        {
            this.handle = handle;
        }

        public JSStringRef(IntPtr handle, bool owns) : base(owns)
        {
            this.handle = handle;
        }

        public static readonly JSStringRef Null = new();
        protected override bool ReleaseHandle()
        {
            NativeJSStringRef.JSStringRelease(handle);
            return true;
        }
    }
}