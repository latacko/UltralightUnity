using System;
using UltralightUnity.HandlesJs;
using UltralightUnity.NativeJs;

namespace UltralightUnity;

public class JSContext
{
    internal readonly JSContextRef contextRef;

    internal JSContext(JSContextRef handle)
    {
        this.contextRef = handle;
    }
    public JSGlobalContext GetGlobalContext()
    {
        return new(NativeJSContextRef.JSContextGetGlobalObject(contextRef), contextRef);
    }

    public JSObject MakeFunctionWithCallback(string name, JSObject.JSObjectCallAsFunctionCallback callAsFunction)
    {
        using var _name = CreateWithUTF8CString(name);
        return new(JSObject.JSObjectMakeFunctionWithCallback(contextRef, _name, callAsFunction), contextRef);
    }

    public JSObject ObjectMake(JSClassRef jsClass, IntPtr data)
    {
        return new(NativeJSObjectRef.JSObjectMake(contextRef, jsClass, data), contextRef);
    }

    public JSValue MakeNull()
    {
        return new(NativeJSValueRef.JSValueMakeNull(contextRef), contextRef);
    }

    public JSValue EvaluateScript(string script, JSObject? thisObject, string? sourceURL, int startingLineNumber, out JSValue exception)
    {
        using var _name = CreateWithUTF8CString(script);
        IntPtr _sourceUrlPtr = IntPtr.Zero;
        if (sourceURL != null && sourceURL.Trim().Length > 0)
        {
            using var _sourceURL = CreateWithUTF8CString(sourceURL);
            _sourceUrlPtr = _sourceURL.stringRef.DangerousGetHandle();
        }
        var value = NativeJSBase.JSEvaluateScript(contextRef, _name.stringRef, thisObject != null ? thisObject.objectRef.DangerousGetHandle() : IntPtr.Zero, _sourceUrlPtr, startingLineNumber, out IntPtr exceptionPtr);
        exception = new(exceptionPtr);
        return new(value, contextRef);
    }

    public JSString CreateWithUTF8CString(string str)
    {
        using var _str = new UltralightUnity.String(str);

        var _handle = NativeJSStringRef.JSStringCreateWithUTF8CString(_str.Handle);
        JSString _stringRef = new(_handle, true, contextRef);

        return _stringRef;
    }
}