using System;
using UltralightUnity.HandlesJs;
using UltralightUnity.NativeJs;

namespace UltralightUnity
{
    public class JSValue
    {
        internal JSValueRef valueRef;
        internal JSContextRef contextRef;

        public JSValue(JSValueRef handle, JSContextRef contextRef)
        {
            this.valueRef = handle;
            this.contextRef = contextRef;
        }

        public JSValue(IntPtr handle, JSContextRef contextRef)
        {
            this.valueRef = new(handle);
            this.contextRef = contextRef;
        }

        public JSValue(IntPtr handle)
        {
            this.valueRef = new(handle);
        }

        public bool IsString()
        {
            return NativeJSValueRef.JSValueIsString(contextRef, valueRef);
        }

        public bool IsObject()
        {
            return NativeJSValueRef.JSValueIsObject(contextRef, valueRef);
        }

        public bool IsNumber()
        {
            return NativeJSValueRef.JSValueIsNumber(contextRef, valueRef);
        }

        public bool IsNull()
        {
            return NativeJSValueRef.JSValueIsNull(contextRef, valueRef);
        }

        public new string ToString()
        {
            return new JSString(valueRef.DangerousGetHandle(), contextRef).ToString();
        }

        public double ToNumber(out JSValue exception)
        {
            double _number = NativeJSValueRef.JSValueToNumber(contextRef, valueRef, out JSValueRef exceptionRef);
            exception = new(exceptionRef, contextRef);
            return _number;
        }

        public JSObject ToObject(out JSValue exception)
        {
            var _objectRef = NativeJSValueRef.JSValueToObject(contextRef, valueRef, out JSValueRef exceptionRef);
            exception = new(exceptionRef, contextRef);
            JSObject _object = new(_objectRef, contextRef);
            return _object;
        }
    }
}