using System;
using System.Runtime.InteropServices;
using UltralightUnity;
using UltralightUnity.HandlesJs;
using UltralightUnity.NativeJs;

public class JSString : JSValue, IDisposable
{
    internal readonly JSStringRef stringRef;
    public int Length => (int)NativeJSStringRef.JSStringGetLength(stringRef);

    public JSString(JSStringRef handle, JSContextRef contextRef) : base(NativeJSValueRef.JSValueMakeString(contextRef, handle).DangerousGetHandle())
    {
        stringRef = handle;
    }

    public JSString(IntPtr handle, JSContextRef contextRef) : base(NativeJSValueRef.JSValueMakeString(contextRef, handle).DangerousGetHandle())
    {
        stringRef = new(handle, false);
    }

    public JSString(IntPtr handle, bool owns, JSContextRef contextRef) : base(NativeJSValueRef.JSValueMakeString(contextRef, handle).DangerousGetHandle())
    {
        stringRef = new(handle, owns);
    }

    public void Retain()
    {
        NativeJSStringRef.JSStringRetain(stringRef);
    }

    public void Release()
    {
        NativeJSStringRef.JSStringRelease(stringRef.DangerousGetHandle());
    }

    public new string ToString()
    {
        var maxSize = NativeJSStringRef.JSStringGetMaximumUTF8CStringSize(stringRef);
        IntPtr buffer = Marshal.AllocHGlobal((int)maxSize);
        try
        {
            // 3. Fill buffer
            var actualSize = NativeJSStringRef.JSStringGetUTF8CString(stringRef, buffer, maxSize);

            if (actualSize == UIntPtr.Zero)
                return string.Empty;

            // 4. Convert to C# string
            return Marshal.PtrToStringUTF8(buffer);
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is JSString _obj)
        {
            return NativeJSStringRef.JSStringIsEqual(stringRef, _obj.stringRef);
        }
        else if (obj is string _str)
        {
            using var _ulStr = new UltralightUnity.String(_str);
            return NativeJSStringRef.JSStringIsEqualToUTF8CString(stringRef, _ulStr.Handle);
        }
        return false;
    }

    public void Dispose()
    {
        stringRef.Dispose();
    }
}