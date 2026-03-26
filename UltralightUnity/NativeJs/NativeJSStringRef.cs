using System;
using System.Runtime.InteropServices;
using UltralightUnity.HandlesJs;
using UltralightUnity.Native;

namespace UltralightUnity.NativeJs
{
    internal static class NativeJSStringRef
    {
        const string LibUltralight = NativeLib.LibUltralight;
        /// <summary>
        /// Creates a JavaScript string from a buffer of Unicode characters.
        /// </summary>
        /// <param name="chars">The buffer of Unicode characters.</param>
        /// <param name="numChars">The number of characters to copy.</param>
        /// <returns>A JSString containing chars.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr JSStringCreateWithCharacters([In] char[] chars, UIntPtr numChars);


        /// <summary>
        /// Creates a JavaScript string from a null-terminated UTF8 string.
        /// </summary>
        /// <param name="str">The null-terminated UTF8 string.</param>
        /// <returns>A JSString containing the string.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr JSStringCreateWithUTF8CString(StringHandle str);


        /// <summary>
        /// Retains a JavaScript string.
        /// </summary>
        /// <param name="string">The JSString to retain.</param>
        /// <returns>The same JSString.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern JSStringRef JSStringRetain(JSStringRef str);


        /// <summary>
        /// Releases a JavaScript string.
        /// </summary>
        /// <param name="string">The JSString to release.</param>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern void JSStringRelease(IntPtr str);


        /// <summary>
        /// Returns the number of Unicode characters in a JavaScript string.
        /// </summary>
        /// <param name="string">The JSString.</param>
        /// <returns>The number of Unicode characters.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr JSStringGetLength(JSStringRef str);


        /// <summary>
        /// Returns a pointer to the Unicode character buffer.
        /// </summary>
        /// <param name="string">The JSString.</param>
        /// <returns>Pointer to UTF-16 character buffer.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr JSStringGetCharactersPtr(JSStringRef str);


        /// <summary>
        /// Returns the maximum number of bytes needed for UTF8 conversion.
        /// </summary>
        /// <param name="string">The JSString.</param>
        /// <returns>Maximum number of bytes.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr JSStringGetMaximumUTF8CStringSize(JSStringRef str);


        /// <summary>
        /// Converts a JavaScript string into a UTF8 string.
        /// </summary>
        /// <param name="string">The source JSString.</param>
        /// <param name="buffer">Destination buffer.</param>
        /// <param name="bufferSize">Size of buffer.</param>
        /// <returns>Number of bytes written (including null terminator).</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr JSStringGetUTF8CString(JSStringRef str, IntPtr buffer, UIntPtr bufferSize);


        /// <summary>
        /// Tests whether two JavaScript strings match.
        /// </summary>
        /// <param name="a">First string.</param>
        /// <param name="b">Second string.</param>
        /// <returns>true if equal, otherwise false.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool JSStringIsEqual(JSStringRef a, JSStringRef b);


        /// <summary>
        /// Tests whether a JavaScript string matches a UTF8 string.
        /// </summary>
        /// <param name="a">JSString to test.</param>
        /// <param name="b">UTF8 string.</param>
        /// <returns>true if equal, otherwise false.</returns>
        [DllImport(LibUltralight, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool JSStringIsEqualToUTF8CString(JSStringRef a, StringHandle b);
    }
}