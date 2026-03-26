using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using UltralightSharedClasses.StringHeaders;
using UltralightSharedClasses.Structs;

namespace UltralightSharedClasses.Classes
{
    public struct StringHolder
    {
        public readonly MemoryMappedFile Mmf;
        public readonly MemoryMappedViewAccessor Accessor;

        public StringHolder(MemoryMappedFile mmf, MemoryMappedViewAccessor accessor)
        {
            Mmf = mmf;
            Accessor = accessor;
        }
    }

    public static class StringManager
    {
        static Dictionary<uint, StringHolder> generatedStrings = new();

        public static uint GenerateMMF<T>(EventType type, T? detailHeader, params string[] strings) where T : unmanaged
        {
            uint _id = (uint)Guid.NewGuid().GetHashCode();

            string _name = BASE_FILE_NAME.STRING + _id.ToString();

            int _stringHeaderSize = Marshal.SizeOf<StringHeader>();

            int _totalSize = Marshal.SizeOf<BaseInfoHeader>() + (detailHeader.HasValue ? Marshal.SizeOf<T>() : 0);
            byte[][] _buffers = new byte[strings.Length][];
            StringHeader[] headers = new StringHeader[strings.Length];

            for (int i = 0; i < strings.Length; i++)
            {
                if (strings[i].Trim().Length == 0)
                    continue;

                _buffers[i] = Encoding.UTF8.GetBytes(strings[i]);

                headers[i] = new()
                {
                    offset = _totalSize + _stringHeaderSize,
                    length = _buffers[i].Length,
                    isThereNextString = (byte)((i < strings.Length - i) ? 1 : 0)
                };
                _totalSize += _stringHeaderSize + _buffers[i].Length;
            }

            var _mmf = CreateMMF.CreateMemoryMappedFile(_name, _totalSize);
            var _accessor = _mmf.CreateViewAccessor();
            BaseInfoHeader _bHeader = new()
            {
                type = type,
                stringsCount = (uint)strings.Length,
                DetailHeaderOffset = (uint)Marshal.SizeOf<BaseInfoHeader>()
            };
            _accessor.Write(0, ref _bHeader);
            if (detailHeader.HasValue)
            {
                T _detailedHeader = detailHeader.Value;
                _accessor.Write(Marshal.SizeOf<BaseInfoHeader>(), ref _detailedHeader);
            }

            for (int i = 0; i < strings.Length; i++)
            {
                if (strings[i].Trim().Length == 0)
                    continue;
                _accessor.Write(headers[i].offset - _stringHeaderSize, ref headers[i]);
                _accessor.WriteArray(headers[i].offset, _buffers[i], 0, headers[i].length);
            }


            generatedStrings.Add(_id, new StringHolder(_mmf, _accessor));

            return _id;
        }

        public static (EventType, object?, List<string>) ReadString(uint id)
        {
            string _name = BASE_FILE_NAME.STRING + id;

            using var _mmf = CreateMMF.OpenMemoryMappedFile(_name);
            using var _accessor = _mmf.CreateViewAccessor();

            _accessor.Read(0, out BaseInfoHeader header);
            Type? _detailHeader = GetHeaderHelper.Get(header.type);
            header.ToDispose = 1;
            _accessor.Write(0, ref header);

            int _detailHeadersize = _detailHeader != null ? Marshal.SizeOf(_detailHeader) : 0;

            byte[] buffer = new byte[_detailHeadersize];

            _accessor.ReadArray(header.DetailHeaderOffset, buffer, 0, _detailHeadersize);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                object? detailHeader = null;
                if (_detailHeader != null)
                    detailHeader = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), _detailHeader)!;

                byte[][] _buffers = new byte[header.stringsCount][];
                StringHeader[] headers = new StringHeader[header.stringsCount];
                List<string> _strings = new();

                long _stringsOffset = header.DetailHeaderOffset + _detailHeadersize;

                for (int i = 0; i < header.stringsCount; i++)
                {
                    _accessor.Read(_stringsOffset, out headers[i]);
                    _buffers[i] = new byte[headers[i].length];
                    _accessor.ReadArray(headers[i].offset, _buffers[i], 0, headers[i].length);
                    _strings.Add(Encoding.UTF8.GetString(_buffers[i]));
                    _stringsOffset = headers[i].offset + headers[i].length;
                }

                return (header.type, detailHeader, _strings);
            }
            catch (Exception e)
            {
                throw new Exception("Failed paring detailed header\n" + e.Message);
            }
            finally
            {
                handle.Free();

                // DisposeString(id);
            }
        }

        static readonly List<uint> toDelete = new();
        public static void TestIfDelete()
        {
            toDelete.Clear();
            foreach (var item in generatedStrings)
            {
                item.Value.Accessor.Read(0, out BaseInfoHeader header);
                if (header.ToDispose == 0) continue;

                toDelete.Add(item.Key);
            }

            foreach (var id in toDelete)
            {
                DisposeString(id);
            }
        }

        public static void DeleteAll()
        {
            toDelete.Clear();
            foreach (var item in generatedStrings)
            {
                toDelete.Add(item.Key);
            }

            foreach (var id in toDelete)
            {
                DisposeString(id);
            }
        }

        private static void DisposeString(uint id)
        {
            generatedStrings[id].Accessor.Dispose();
            generatedStrings[id].Mmf.Dispose();
            generatedStrings.Remove(id);

#if UNITY_STANDALONE_WIN
#elif UNITY_STANDALONE_LINUX
                File.Delete(CreateMMF.LINUX_PATH + BASE_FILE_NAME.STRING + id.ToString());
#else
            if (Environment.OSVersion.Platform == PlatformID.Unix)
                File.Delete(CreateMMF.LINUX_PATH + BASE_FILE_NAME.STRING + id.ToString());
#endif
        }
    }
}