using Microsoft.Win32.SafeHandles;
using UltralightUnity.Native;

namespace UltralightUnity.Handles;

internal sealed class ULViewHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private ULViewHandle() : base(false) { }

    protected override bool ReleaseHandle()
    {
        NativeView.ulDestroyView(handle);
        return true;
    }
}