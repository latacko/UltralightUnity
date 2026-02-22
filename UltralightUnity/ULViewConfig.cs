using System;
using UltralightUnity.Native;

namespace UltralightUnity;

public sealed class ULViewConfig : IDisposable
{
    internal ULViewConfigHandle Handle { get; }

    public ULViewConfig()
    {
        Handle = NativeViewConfig.ulCreateViewConfig();
    }

    public void SetDisplayId(uint id) =>
        NativeViewConfig.ulViewConfigSetDisplayId(Handle, id);

    public void SetIsAccelerated(bool value) =>
        NativeViewConfig.ulViewConfigSetIsAccelerated(Handle, value);

    public void SetIsTransparent(bool value) =>
        NativeViewConfig.ulViewConfigSetIsTransparent(Handle, value);

    public void SetInitialDeviceScale(double scale) =>
        NativeViewConfig.ulViewConfigSetInitialDeviceScale(Handle, scale);

    public void SetInitialFocus(bool focused) =>
        NativeViewConfig.ulViewConfigSetInitialFocus(Handle, focused);

    public void SetEnableImages(bool enabled) =>
        NativeViewConfig.ulViewConfigSetEnableImages(Handle, enabled);

    public void SetEnableJavaScript(bool enabled) =>
        NativeViewConfig.ulViewConfigSetEnableJavaScript(Handle, enabled);

    public void SetFontFamilyStandard(string name) =>
        SetFont(name, NativeViewConfig.ulViewConfigSetFontFamilyStandard);

    public void SetFontFamilyFixed(string name) =>
        SetFont(name, NativeViewConfig.ulViewConfigSetFontFamilyFixed);

    public void SetFontFamilySerif(string name) =>
        SetFont(name, NativeViewConfig.ulViewConfigSetFontFamilySerif);

    private void SetFont(string name, Action<ULViewConfigHandle, ULStringHandle> setter)
    {
        using var str = new ULString(name);
        setter(Handle, str.Handle);
    }

    public void Dispose(){
        Handle.Dispose();
    }
}