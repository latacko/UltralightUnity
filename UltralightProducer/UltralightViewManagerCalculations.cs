using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using UltralightSharedClasses.Structs;

public unsafe partial class UltralightViewManager : IDisposable
{
    private int ComputeFildSizes()
    {
        var type = GetType();
        var bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        var fields = type.GetFields(bindingFlags);

        int _totalSize = 0;
        foreach (var field in fields)
        {
            var attr = field.GetCustomAttribute<FieldSizeAttribute>();
            if (attr == null) continue;
            int _size = Convert.ToInt32(field.GetValue(this));
            _totalSize += _size;
        }
        return _totalSize;
    }

    private void ComputeOffsets()
    {
        var type = GetType();
        var bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        var fields = type.GetFields(bindingFlags);

        // first offset
        SetField(nameof(mouseOffset), headerSize);
        SetHeaderOffset(nameof(ViewHeader.mouseOffset), headerSize);

        uint current = (uint)headerSize;

        foreach (var field in fields)
        {
            var attr = field.GetCustomAttribute<OffsetAfterAttribute>();
            if (attr == null) continue;

            var sizeField = type.GetField(attr.SizeField, bindingFlags)
                ?? throw new Exception($"Size field '{attr.SizeField}' not found.");

            uint size = Convert.ToUInt32(sizeField.GetValue(this));
            current += size;

            // set local readonly field
            SetField(field.Name, (int)current);

            // set matching header field if it exists
            var headerField = typeof(ViewHeader).GetField(field.Name + "Offset") // e.g. keyOffset -> keyOffsetOffset won't match
                ?? typeof(ViewHeader).GetField(field.Name.Replace("Offset", "") + "Offset") // normalize naming
                ?? typeof(ViewHeader).GetField(field.Name); // exact match fallback

            if (headerField != null)
                SetHeaderOffset(headerField.Name, (int)current);
            else
                Console.WriteLine("Missing " + field.Name + " in the header file");
        }
    }

    private void SetField(string name, int value)
    {
        var field = GetType().GetField(name,
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)!;
        field.SetValue(this, value);
    }

    private void SetHeaderOffset(string headerFieldName, int value)
    {
        var field = typeof(ViewHeader).GetField(headerFieldName);
        if (field == null) return;

        int byteOffset = (int)Marshal.OffsetOf<ViewHeader>(headerFieldName);
        unsafe { *(uint*)((byte*)header + byteOffset) = (uint)value; }
    }
}