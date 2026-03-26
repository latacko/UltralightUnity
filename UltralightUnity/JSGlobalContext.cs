using UltralightUnity.HandlesJs;
using UltralightUnity.NativeJs;

namespace UltralightUnity
{
    public class JSGlobalContext : JSObject
    {

        internal JSGlobalContext(JSObjectRef handle, JSContextRef contextRef) : base(handle, contextRef)
        {
        }


        public new void SetProperty(string name, JSValue value, uint attributes, out JSValueRef exception)
        {
            using var _name = new JSContext(contextRef).CreateWithUTF8CString(name);
            NativeJSObjectRef.JSObjectSetProperty(contextRef, objectRef, _name.stringRef, value.valueRef, attributes, out exception);
        }
    }
}