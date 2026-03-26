using System;
using System.Runtime.InteropServices;
using UltralightUnity.HandlesJs;

namespace UltralightUnity.NativeJs
{
    internal static class NativeJSBase
    {
        const string WebCore = NativeLib.WebCore;
        /// <summary>
        /// Evaluates a string of JavaScript.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="script">The script to evaluate.</param>
        /// <param name="thisObject">The object to use as "this", or null.</param>
        /// <param name="sourceURL">Optional source URL for debugging.</param>
        /// <param name="startingLineNumber">Starting line number (1-based).</param>
        /// <param name="exception">A pointer in which to store an exception, if any.</param>
        /// <returns>
        /// The JSValue result, or null if an exception is thrown.
        /// </returns>
        [DllImport(WebCore, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr JSEvaluateScript(JSContextRef ctx, JSStringRef script, IntPtr thisObject, IntPtr sourceURL, int startingLineNumber, out IntPtr exception);


        /// <summary>
        /// Checks for syntax errors in a string of JavaScript.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="script">The script to check.</param>
        /// <param name="sourceURL">Optional source URL.</param>
        /// <param name="startingLineNumber">Starting line number (1-based).</param>
        /// <param name="exception">A pointer in which to store a syntax exception, if any.</param>
        /// <returns>true if the script is valid, otherwise false.</returns>
        [DllImport(WebCore, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool JSCheckScriptSyntax(JSContextRef ctx, JSStringRef script, JSStringRef sourceURL, int startingLineNumber, out JSValueRef exception);


        /// <summary>
        /// Performs a JavaScript garbage collection.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <remarks>
        /// Normally not required; the engine collects automatically.
        /// </remarks>
        [DllImport(WebCore, CallingConvention = CallingConvention.Cdecl)]
        public static extern void JSGarbageCollect(JSContextRef ctx);
    }
}