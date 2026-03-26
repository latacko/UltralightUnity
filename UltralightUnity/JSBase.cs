using System;
using UltralightUnity.HandlesJs;
using UltralightUnity.NativeJs;

namespace UltralightUnity
{
    public static class JSBase
    {
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
        // public static IntPtr JSEvaluateScript(JSContextRef ctx, JSString script, JSObjectRef thisObject, IntPtr sourceURL, int startingLineNumber, out IntPtr exception)
        // {
        //     return NativeJSBase.JSEvaluateScript(ctx, script.string_handle, thisObject, IntPtr.Zero, startingLineNumber, out exception);
        // }


        /// <summary>
        /// Checks for syntax errors in a string of JavaScript.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <param name="script">The script to check.</param>
        /// <param name="sourceURL">Optional source URL.</param>
        /// <param name="startingLineNumber">Starting line number (1-based).</param>
        /// <param name="exception">A pointer in which to store a syntax exception, if any.</param>
        /// <returns>true if the script is valid, otherwise false.</returns>
        public static bool JSCheckScriptSyntax(JSContextRef ctx, JSString script, JSString sourceURL, int startingLineNumber, out JSValueRef exception)
        {
            return NativeJSBase.JSCheckScriptSyntax(ctx, script.stringRef, sourceURL == null ? JSStringRef.Null : sourceURL.stringRef, startingLineNumber, out exception);
        }


        /// <summary>
        /// Performs a JavaScript garbage collection.
        /// </summary>
        /// <param name="ctx">The execution context to use.</param>
        /// <remarks>
        /// Normally not required; the engine collects automatically.
        /// </remarks>
        public static void JSGarbageCollect(JSContextRef ctx)
        {
            NativeJSBase.JSGarbageCollect(ctx);
        }
    }
}