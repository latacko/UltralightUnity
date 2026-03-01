using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using UltralightUnity;
using UltralightUnity.Native;

public class ULFileSystem
{
    public delegate bool ULFileExistsCallback(string path);
    public delegate string ULGetMimeTypeCallback(string path);
    public delegate string ULGetCharsetCallback(string path);

    public delegate ULBuffer ULOpenFileCallback(string path);

    private NativeFileSystem.ULFileExistsCallback fileExistsCallback;
    private NativeFileSystem.ULGetMimeTypeCallback getMimeTypeCallback;
    private NativeFileSystem.ULGetCharsetCallback getCharsetCallback;
    private NativeFileSystem.ULOpenFileCallback openFileCallback;

    private NativeFileSystem nativeFileSystem;
    private readonly string baseDir;

    public ULFileSystem():this("")
    {
        
    }

    public ULFileSystem(string baseDir)
    {
        this.baseDir = baseDir;
        fileExistsCallback = FileExistsEvent;
        getMimeTypeCallback = GetMimeTypeEvent;
        getCharsetCallback = GetCharacterEvent;
        openFileCallback = OpenFileEvent;

        nativeFileSystem = new()
        {
            file_exists = fileExistsCallback,
            get_file_charset = getCharsetCallback,
            get_file_mime_type = getMimeTypeCallback,
            open_file = openFileCallback
        };

        NativePlatform.ulPlatformSetFileSystem(nativeFileSystem);
        Console.WriteLine(Marshal.SizeOf<NativeFileSystem>());
    }
    private bool FileExistsEvent(IntPtr pathPtr)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        string _path = new ULString(new ULStringHandle(pathPtr, false)).ToManagedString();
        bool _exist = FileExists(_path);
        stopwatch.Stop();
        return _exist;
    }

    private IntPtr GetMimeTypeEvent(IntPtr pathPtr)
    {
        string _path = new ULString(new ULStringHandle(pathPtr, false)).ToManagedString();
        var _mime = new ULString(GetMimeType(_path));
        return _mime.Handle.DangerousGetHandle();
    }

    private IntPtr GetCharacterEvent(IntPtr pathPtr)
    {
        string _path = new ULString(new ULStringHandle(pathPtr, false)).ToManagedString();
        var _charset = new ULString(GetCharacter(_path));
        return _charset.Handle.DangerousGetHandle();
    }

    private IntPtr OpenFileEvent(IntPtr pathPtr)
    {
        string _path = new ULString(new ULStringHandle(pathPtr, false)).ToManagedString();
        var _bufferHandle = OpenFile(_path).Handle;
        return _bufferHandle.DangerousGetHandle();
    }

    public virtual bool FileExists(string path)
    {
        return File.Exists(baseDir + path);
    }

    public virtual string GetMimeType(string path)
    {
        return MimeTypes.Get(Path.GetExtension(baseDir + path)[1..]);
    }

    public virtual string GetCharacter(string path)
    {
        return "utf-8";
    }

    public virtual ULBuffer OpenFile(string path)
    {
        byte[] data = File.ReadAllBytes(baseDir + path);
        int size = data.Length;

        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.Copy(data, 0, ptr, size);

        var buffer = new ULBuffer(ptr, (uint)size)
        {
            OnDestroyBuffer = (userData, dataPtr) =>
            {
                // dataPtr == ptr we allocated
                Debug.WriteLine("Destroying buffer " + path);
                Marshal.FreeHGlobal(dataPtr);
            }
        };

        return buffer;
    }
}