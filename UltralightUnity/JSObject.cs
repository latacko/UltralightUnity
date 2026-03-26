using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UltralightUnity.HandlesJs;
using UltralightUnity.NativeJs;

namespace UltralightUnity
{
    public class JSObject : JSValue
    {
        internal readonly JSObjectRef objectRef;

        internal JSObject(JSObjectRef handle, JSContextRef contextRef) : base(handle.DangerousGetHandle(), contextRef)
        {
            this.objectRef = handle;
        }

        public JSObject SetProperty(string name, JSValue value, uint attributes, out JSValueRef exception)
        {
            using var _name = new JSContext(contextRef).CreateWithUTF8CString(name);
            NativeJSObjectRef.JSObjectSetProperty(contextRef, objectRef, _name.stringRef, value.valueRef, attributes, out exception);
            return this;
        }

        public JSValue GetProperty(string name, out JSValueRef exception)
        {
            using var _name = new JSContext(contextRef).CreateWithUTF8CString(name);
            var _valueRef = NativeJSObjectRef.JSObjectGetProperty(contextRef, objectRef, _name.stringRef, out exception);
            return new (_valueRef, contextRef);
        }

        public List<string> GetPropertyNames()
        {
            var _result = new List<string>();
            using var _array = NativeJSObjectRef.JSObjectCopyPropertyNames(contextRef, objectRef);
            var _count = NativeJSObjectRef.JSPropertyNameArrayGetCount(_array);
            Console.WriteLine("keys count: " + _count);
            for (nuint i = 0; i < _count; i++)
            {
                var _nameRef = NativeJSObjectRef.JSPropertyNameArrayGetNameAtIndex(_array, i);
                _result.Add(new JSString(_nameRef, contextRef).ToString());
            }

            return _result;
        }

        public bool IsFunction()
        {
            return NativeJSObjectRef.JSObjectIsFunction(contextRef, objectRef);
        }

        public JSValue CallAsFunction(JSObject? thisObject, JSValue[] arguments, out JSValue exception)
        {
            IntPtr[] _argumentsRef = new IntPtr[arguments.Length];
            for (int i = 0; i < arguments.Length; i++)
            {
                _argumentsRef[i] = arguments[i].valueRef.DangerousGetHandle();
            }
            var _valueRef = NativeJSObjectRef.JSObjectCallAsFunction(contextRef, objectRef, thisObject != null ? thisObject.objectRef.DangerousGetHandle() : IntPtr.Zero, (nuint)arguments.Length, arguments.Length > 0 ? _argumentsRef : null, out JSValueRef exceptionRef);
            exception = new(exceptionRef, contextRef);
            return new(_valueRef, contextRef);
        }


        public delegate JSValue JSObjectCallAsFunctionCallback(JSContext ctx, JSObject function, JSObject thisObject, nint argumentCount, [In] JSValue[] arguments, out JSValue? exception);
        internal static JSObjectRef JSObjectMakeFunctionWithCallback(JSContextRef ctx, JSString name, JSObjectCallAsFunctionCallback callAsFunction)
        {
            NativeJSObjectRef.JSObjectCallAsFunctionCallback raw = (rawCtx, rawFn, rawThis, argCount, rawArgs, out rawException) =>
            {
                var _context = new JSContext(new JSContextRef(rawCtx, false));

                var argPtrs = new IntPtr[(int)argCount];
                for (int i = 0; i < argCount; i++)
                    argPtrs[i] = Marshal.ReadIntPtr(rawArgs, i * IntPtr.Size);

                var wrappedArgs = argPtrs.Select(a => new JSValue(new JSValueRef(a, false), _context.contextRef)).ToArray();


                var result = callAsFunction(
                    _context,
                    new(new JSObjectRef(rawFn, false), _context.contextRef),
                    new(new JSObjectRef(rawThis, false), _context.contextRef),
                    argCount,
                    wrappedArgs,
                    out var exception);

                rawException = exception?.valueRef?.DangerousGetHandle() ?? IntPtr.Zero;
                return result?.valueRef?.DangerousGetHandle() ?? IntPtr.Zero;
            };

            GC.KeepAlive(raw);
            GC.KeepAlive(ctx);
            GC.KeepAlive(name);
            return NativeJSObjectRef.JSObjectMakeFunctionWithCallback(ctx, name.stringRef, raw);
        }
    }
}