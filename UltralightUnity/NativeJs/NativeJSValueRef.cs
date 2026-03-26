using System;
using System.Runtime.InteropServices;
using UltralightUnity.EnumsJs;
using UltralightUnity.HandlesJs;

namespace UltralightUnity.NativeJs
{
    public static class NativeJSValueRef
    {
        const string LibUltralight = NativeLib.LibUltralight;

        /// <summary>
        /// Returns a JavaScript value's type.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="value">The JSValue whose type you want to obtain.</param>
        /// <returns>A value of type JSType that identifies value's type.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern JSType JSValueGetType(JSContextRef ctx, JSValueRef value);


        /// <summary>
        /// Tests whether a JavaScript value's type is the undefined type.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="value">The JSValue to test.</param>
        /// <returns>true if value's type is the undefined type, otherwise false.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool JSValueIsUndefined(JSContextRef ctx, JSValueRef value);


        /// <summary>
        /// Tests whether a JavaScript value's type is the null type.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="value">The JSValue to test.</param>
        /// <returns>true if value's type is the null type, otherwise false.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool JSValueIsNull(JSContextRef ctx, JSValueRef value);


        /// <summary>
        /// Tests whether a JavaScript value's type is the boolean type.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="value">The JSValue to test.</param>
        /// <returns>true if value's type is the boolean type, otherwise false.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool JSValueIsBoolean(JSContextRef ctx, JSValueRef value);


        /// <summary>
        /// Tests whether a JavaScript value's type is the number type.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="value">The JSValue to test.</param>
        /// <returns>true if value's type is the number type, otherwise false.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool JSValueIsNumber(JSContextRef ctx, JSValueRef value);


        /// <summary>
        /// Tests whether a JavaScript value's type is the string type.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="value">The JSValue to test.</param>
        /// <returns>true if value's type is the string type, otherwise false.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool JSValueIsString(JSContextRef ctx, JSValueRef value);


        /// <summary>
        /// Tests whether a JavaScript value's type is the object type.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="value">The JSValue to test.</param>
        /// <returns>true if value's type is the object type, otherwise false.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool JSValueIsObject(JSContextRef ctx, JSValueRef value);

        /// <summary>
        /// Tests whether a JavaScript value is an object with a given class in its class chain.
        /// </summary>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool JSValueIsObjectOfClass(JSContextRef ctx, JSValueRef value, IntPtr jsClass);


        /// <summary>
        /// Tests whether two JavaScript values are equal, as compared by the JS == operator.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="a">The first value to test.</param>
        /// <param name="b">The second value to test.</param>
        /// <param name="exception">A pointer in which to store an exception, if any.</param>
        /// <returns>
        /// true if the two values are equal, false if they are not equal or an exception is thrown.
        /// </returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool JSValueIsEqual(JSContextRef ctx, JSValueRef a, JSValueRef b, out JSValueRef exception);


        /// <summary>
        /// Tests whether two JavaScript values are strict equal, as compared by the JS === operator.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="a">The first value to test.</param>
        /// <param name="b">The second value to test.</param>
        /// <returns>true if the two values are strict equal, otherwise false.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool JSValueIsStrictEqual(JSContextRef ctx, JSValueRef a, JSValueRef b);


        /// <summary>
        /// Tests whether a JavaScript value is an object constructed by a given constructor,
        /// as compared by the JS instanceof operator.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="value">The JSValue to test.</param>
        /// <param name="constructor">The constructor to test against.</param>
        /// <param name="exception">A pointer in which to store an exception, if any.</param>
        /// <returns>
        /// true if value is an object constructed by constructor, otherwise false.
        /// </returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool JSValueIsInstanceOfConstructor(JSContextRef ctx, JSValueRef value, JSObjectRef constructor, out JSValueRef exception);


        /// <summary>
        /// Creates a JavaScript value of the undefined type.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <returns>The unique undefined value.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern JSValueRef JSValueMakeUndefined(JSContextRef ctx);


        /// <summary>
        /// Creates a JavaScript value of the null type.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <returns>The unique null value.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern JSValueRef JSValueMakeNull(JSContextRef ctx);


        /// <summary>
        /// Creates a JavaScript value of the boolean type.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="boolean">The bool to assign to the newly created JSValue.</param>
        /// <returns>
        /// A JSValue of the boolean type, representing the value of boolean.
        /// </returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern JSValueRef JSValueMakeBoolean(JSContextRef ctx, bool boolean);


        /// <summary>
        /// Creates a JavaScript value of the number type.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="number">The double to assign to the newly created JSValue.</param>
        /// <returns>
        /// A JSValue of the number type, representing the value of number.
        /// </returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern JSValueRef JSValueMakeNumber(JSContextRef ctx, double number);


        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern JSValueRef JSValueMakeString(JSContextRef ctx, JSStringRef stringRef);

        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern JSValueRef JSValueMakeString(JSContextRef ctx, IntPtr stringRef);


        /// <summary>
        /// Converts a JavaScript value to boolean and returns the resulting boolean.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="value">The JSValue to convert.</param>
        /// <returns>The boolean result of conversion.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool JSValueToBoolean(JSContextRef ctx, JSValueRef value);


        /// <summary>
        /// Converts a JavaScript value to number and returns the resulting number.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="value">The JSValue to convert.</param>
        /// <param name="exception">A pointer in which to store an exception, if any.</param>
        /// <returns>
        /// The numeric result of conversion, or NaN if an exception is thrown.
        /// </returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern double JSValueToNumber(JSContextRef ctx, JSValueRef value, out JSValueRef exception);


        /// <summary>
        /// Converts a JavaScript value to string and copies the result into a JavaScript string.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="value">The JSValue to convert.</param>
        /// <param name="exception">A pointer in which to store an exception, if any.</param>
        /// <returns>
        /// A JSString with the result of conversion, or null if an exception is thrown.
        /// </returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern JSStringRef JSValueToStringCopy(JSContextRef ctx, JSValueRef value, out JSValueRef exception);


        /// <summary>
        /// Converts a JavaScript value to object and returns the resulting object.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="value">The JSValue to convert.</param>
        /// <param name="exception">A pointer in which to store an exception, if any.</param>
        /// <returns>
        /// The JSObject result of conversion, or null if an exception is thrown.
        /// </returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern JSObjectRef JSValueToObject(JSContextRef ctx, JSValueRef value, out JSValueRef exception);


        /// <summary>
        /// Protects a JavaScript value from garbage collection.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="value">The JSValue to protect.</param>
        /// <remarks>
        /// Use this when storing a JSValue outside of JavaScript (e.g., in native/global state).
        /// Must be balanced with JSValueUnprotect.
        /// </remarks>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern void JSValueProtect(JSContextRef ctx, JSValueRef value);


        /// <summary>
        /// Unprotects a JavaScript value from garbage collection.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="value">The JSValue to unprotect.</param>
        /// <remarks>
        /// Must be called the same number of times as JSValueProtect.
        /// </remarks>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern void JSValueUnprotect(JSContextRef ctx, JSValueRef value);
    }
}