using System;
using Microsoft.Win32.SafeHandles;

namespace UltralightUnity.HandlesJs
{
    public class JSClassRef : SafeHandleZeroOrMinusOneIsInvalid
    {
        public JSClassRef() : base(false)
        {

        }

        public JSClassRef(IntPtr handle) : base(false)
        {
            this.handle = handle;
        }

        public static JSClassRef Empty => new(IntPtr.Zero);

        protected override bool ReleaseHandle()
        {
            return true;
        }
    }
}
