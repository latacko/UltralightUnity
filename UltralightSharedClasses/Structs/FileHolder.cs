using System.IO.MemoryMappedFiles;

namespace UltralightSharedClasses.Structs
{
    public struct FileHolder
    {
        public readonly MemoryMappedFile Mmf;
        public readonly MemoryMappedViewAccessor Accessor;

        public FileHolder(MemoryMappedFile mmf, MemoryMappedViewAccessor accessor)
        {
            Mmf = mmf;
            Accessor = accessor;
        }
    }
}
