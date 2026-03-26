namespace UltralightUnity.HandlesJs
{
    public static class JSHandleExtensions
    {
        public static JSValueRef AsJSValue(this JSObjectRef obj)
        {
            if (obj == null || obj.IsInvalid)
                return JSValueRef.Null;

            return new JSValueRef(obj.DangerousGetHandle());
        }
    }
}