using System;
using Microsoft.Win32.SafeHandles;

namespace UltralightUnity.HandlesJs
{
    public class JSValueRef : SafeHandleZeroOrMinusOneIsInvalid
    {
        public JSValueRef() : base(false) { }
        public JSValueRef(IntPtr handle) : base(false) { this.handle = handle; }
        public JSValueRef(IntPtr handle, bool owns) : base(owns) { this.handle = handle; }
        public static readonly JSValueRef Null = new();
        protected override bool ReleaseHandle() => true;

        public override string ToString()
        {
            return base.ToString() + " handle " +handle; 
        }
    }
}