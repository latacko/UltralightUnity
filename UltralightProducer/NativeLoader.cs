using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

static class NativeLoader
{
    public static void Load(string libName)
    {
        string resourceName = Assembly.GetExecutingAssembly()
            .GetName().Name + ".libs." + libName;

        string tempPath = Path.Combine(Path.GetTempPath(), libName);

        using Stream? stream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(resourceName);

        if (stream == null)
            throw new Exception($"Resource {resourceName} not found");

        using FileStream fs = new FileStream(tempPath, FileMode.Create);
        stream.CopyTo(fs);

        NativeLibrary.Load(tempPath);
    }
}