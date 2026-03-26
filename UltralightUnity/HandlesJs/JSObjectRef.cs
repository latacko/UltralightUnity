using System;
using Microsoft.Win32.SafeHandles;

namespace UltralightUnity.HandlesJs
{
    public class JSObjectRef : SafeHandleZeroOrMinusOneIsInvalid
    {
        public JSObjectRef() : base(false) { }
        public JSObjectRef(IntPtr handle, bool owns) : base(owns) { this.handle = handle; }
        public static readonly JSObjectRef Null = new();
        protected override bool ReleaseHandle() => true;

        public override string ToString()
        {
            return base.ToString() + " handle: " + handle;
        }
    }
}