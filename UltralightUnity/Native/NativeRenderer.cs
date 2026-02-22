using System;
using System.Runtime.InteropServices;
using UltralightUnity.Handles;

namespace UltralightUnity.Native;

internal static class NativeRenderer
{
    [DllImport(NativeLib.LibUltralight)]
    internal static extern ULRendererHandle ulCreateRenderer(ULConfigHandle config);

    [DllImport(NativeLib.LibUltralight)]
    internal static extern void ulDestroyRenderer(IntPtr renderer);

    [DllImport(NativeLib.LibUltralight)]
    internal static extern void ulUpdate(ULRendererHandle renderer);

    [DllImport(NativeLib.LibUltralight)]
    internal static extern void ulRefreshDisplay(ULRendererHandle renderer, uint display_id);

    [DllImport(NativeLib.LibUltralight)]
    internal static extern void ulRender(ULRendererHandle renderer);

    [DllImport(NativeLib.LibUltralight)]
    internal static extern void ulPurgeMemory(ULRendererHandle renderer);

    [DllImport(NativeLib.LibUltralight)]
    internal static extern void ulLogMemoryUsage(ULRendererHandle renderer);

    /// <summary>
    /// Start the remote inspector server.
    ///
    /// While the remote inspector is active, Views that are loaded into this renderer
    /// will be able to be remotely inspected from another Ultralight instance either locally
    /// (another app on same machine) or remotely (over the network) by navigating a View to:
    ///
    /// \code
    ///   inspector://<ADDRESS>:<PORT>
    /// \endcode
    /// </summary>
    /// <param name="renderer">The active renderer instance.</param>
    /// <param name="address">The address for the server to listen on (eg, "127.0.0.1")</param>
    /// <param name="port">The port for the server to listen on (eg, 9222)</param>
    /// <returns>Returns whether the server started successfully or not.</returns>
    [DllImport(NativeLib.LibUltralight)]
    [return: MarshalAs(UnmanagedType.I1)]
    // ! Muszę naprawć string address powinno być const char* zrobić odpowiednik
    internal static extern bool ulStartRemoteInspectorServer(ULRendererHandle renderer, StringHandle address, ushort port);

    [DllImport(NativeLib.LibUltralight)]
    internal static extern void ulSetGamepadDetails(ULRendererHandle renderer, uint index, ULStringHandle id, uint axis_coun, uint button_count);

    // [DllImport(NativeLib.LibUltralight)]
    // internal static extern void ulFireGamepadEvent(IntPtr renderer, ULGamepadEvent evt);

    // [DllImport(NativeLib.LibUltralight)]
    // internal static extern void ulFireGamepadAxisEvent(IntPtr renderer, ULGamepadAxisEvent evt);

    // [DllImport(NativeLib.LibUltralight)]
    // internal static extern void ulFireGamepadButtonEvent(IntPtr renderer, ULGamepadButtonEvent evt);
}
