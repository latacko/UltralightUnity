using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace UltralightUnity.Native;

internal sealed class ULStringHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private string _creationStack;
    public ULStringHandle() : base(true)
    {
        _creationStack = Environment.StackTrace;
    }

    internal ULStringHandle(IntPtr existingHandle, bool ownsHandle) : base(ownsHandle)
    {
        SetHandle(existingHandle);
    }


    protected override bool ReleaseHandle()
    {
        // Console.WriteLine("=== ulDestroyString called ===");
        // Console.WriteLine("Created at:");
        // Console.WriteLine(_creationStack);
        // Console.WriteLine("Destroyed at:");
        // Console.WriteLine(Environment.StackTrace);
        NativeString.ulDestroyString(handle);
        return true;
    }
}
