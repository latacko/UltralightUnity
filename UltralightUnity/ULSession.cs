using System;
using UltralightUnity.Native;

namespace UltralightUnity;

public sealed class ULSession : IDisposable
{
    internal ULSessionHandle Handle { get; }

    public ULSession(ULRenderer renderer, bool persistent, string name)
    {
        using var s = new ULString(name);
        Handle = NativeSession.ulCreateSession(renderer.Handle, persistent, s.Handle);
    }

    public static ULSession Default(ULRenderer renderer)
    {
        return new ULSession(NativeSession.ulDefaultSession(renderer.Handle));
    }

    private ULSession(ULSessionHandle handle)
    {
        Handle = handle;
    }

    public bool IsPersistent => NativeSession.ulSessionIsPersistent(Handle);
    public string Name => new ULString(NativeSession.ulSessionGetName(Handle)).ToManagedString();
    public string DiskPath => new ULString(NativeSession.ulSessionGetDiskPath(Handle)).ToManagedString();
    public ulong Id => NativeSession.ulSessionGetId(Handle);

    public void Dispose() => Handle.Dispose();
}