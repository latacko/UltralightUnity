using System;
using UltralightUnity.Native;

namespace UltralightUnity;

public sealed class ULRenderer : IDisposable
{
    internal ULRendererHandle Handle { get; }

    public ULRenderer(ULConfig config)
    {
        Handle = NativeRenderer.ulCreateRenderer(config.Handle);

        if (Handle.IsInvalid)
            throw new InvalidOperationException("ulCreateRenderer failed.");
    }

    public void Update() => NativeRenderer.ulUpdate(Handle);

    public void RefreshDisplay(uint displayId = 0) => NativeRenderer.ulRefreshDisplay(Handle, displayId);

    public void Render() => NativeRenderer.ulRender(Handle);

    public void PurgeMemory() => NativeRenderer.ulPurgeMemory(Handle);

    public void LogMemoryUsage() => NativeRenderer.ulLogMemoryUsage(Handle);

    public bool StartRemoteInspector(string address, ushort port)
    {
        using var _addressStr = new String(address);
        return NativeRenderer.ulStartRemoteInspectorServer(Handle, _addressStr.Handle, port);
    }


    public ULView CreateView(uint width, uint height, ULViewConfig config)
    {
        return new ULView(this, width, height, config);
    }

    public ULView CreateView(uint width, uint height, ULViewConfig config, ULSession session)
    {
        return new ULView(this, width, height, config, session);
    }

    public void Dispose(){
        Handle.Dispose();
    }
}
