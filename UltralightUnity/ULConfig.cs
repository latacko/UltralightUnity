using System;
using UltralightUnity.Enums;
using UltralightUnity.Handles;
using UltralightUnity.Native;

namespace UltralightUnity;

public sealed class ULConfig : IDisposable
{
    internal ULConfigHandle Handle { get; }

    public ULConfig()
    {
        Handle  = NativeConfig.ulCreateConfig();
    }

    public string CachePath
    {
        set
        {
            using var s = new ULString(value);
            NativeConfig.ulConfigSetCachePath(Handle, s.Handle);
        }
    }

    public string ResourcePathPrefix
    {
        set
        {
            using var s = new ULString(value);
            NativeConfig.ulConfigSetResourcePathPrefix(Handle, s.Handle);
        }
    }

    public ULFaceWinding FaceWinding
    {
        set => NativeConfig.ulConfigSetFaceWinding(Handle, (byte)value);
    }

    public ULFontHinting FontHinting
    {
        set => NativeConfig.ulConfigSetFontHinting(Handle, (byte)value);
    }

    public double FontGamma
    {
        set => NativeConfig.ulConfigSetFontGamma(Handle, value);
    }

    public string UserStylesheet
    {
        set
        {
            using var s = new ULString(value);
            NativeConfig.ulConfigSetUserStylesheet(Handle, s.Handle);
        }
    }

    public bool ForceRepaint
    {
        set => NativeConfig.ulConfigSetForceRepaint(Handle, value);
    }

    public double AnimationTimerDelay
    {
        set => NativeConfig.ulConfigSetAnimationTimerDelay(Handle, value);
    }

    public double ScrollTimerDelay
    {
        set => NativeConfig.ulConfigSetScrollTimerDelay(Handle, value);
    }

    public double RecycleDelay
    {
        set => NativeConfig.ulConfigSetRecycleDelay(Handle, value);
    }

    public uint MemoryCacheSize
    {
        set => NativeConfig.ulConfigSetMemoryCacheSize(Handle, value);
    }

    public uint PageCacheSize
    {
        set => NativeConfig.ulConfigSetPageCacheSize(Handle, value);
    }

    public uint OverrideRAMSize
    {
        set => NativeConfig.ulConfigSetOverrideRAMSize(Handle, value);
    }

    public uint MinLargeHeapSize
    {
        set => NativeConfig.ulConfigSetMinLargeHeapSize(Handle, value);
    }

    public uint MinSmallHeapSize
    {
        set => NativeConfig.ulConfigSetMinSmallHeapSize(Handle, value);
    }

    public uint NumRendererThreads
    {
        set => NativeConfig.ulConfigSetNumRendererThreads(Handle, value);
    }

    public double MaxUpdateTime
    {
        set => NativeConfig.ulConfigSetMaxUpdateTime(Handle, value);
    }

    public uint BitmapAlignment
    {
        set => NativeConfig.ulConfigSetBitmapAlignment(Handle, value);
    }

    public void Dispose() {
        Handle.Dispose();
    }
}
