using System;
using Microsoft.Win32.SafeHandles;

namespace UltralightUnity.HandlesJs
{
    public class JSContextRef : SafeHandleZeroOrMinusOneIsInvalid
    {
        public JSContextRef() : base(false) { }
        public JSContextRef(IntPtr handle, bool owns) : base(owns) { this.handle = handle; }
        public static readonly JSContextRef Null = new();
        protected override bool ReleaseHandle() => true;

        public override string ToString()
        {
            return base.ToString() + " handle " + handle;
        }
    }
}