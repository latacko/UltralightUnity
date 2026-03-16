using System.Runtime.InteropServices;
using UltralightSharedClasses.Enums;

namespace UltralightSharedClasses.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BaseEvent
    {
        /// <summary>
        /// 1- begin loading
        /// 2- finish
        /// </summary>
        public BaseEventType type;
    }
}