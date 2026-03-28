using System.Runtime.InteropServices;

namespace UltralightSharedClasses.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ViewHeader
    {
        public uint magic;

        /* offsets */
        public uint mouseOffset;
        public uint keyOffset;
        public uint textOffset;
        public uint resizeOffset;
        public uint frameOffset;
        public uint loadEventsOffset;
        public uint setupHTML_OR_URL_Offset;
        public uint executeJSOffset;
        public uint messageConsoleOffset;
        public uint messageEmittedOffset;
        public uint postMessageOffset;
        public uint baseEventsOffset;

        public byte openInspector;


        /* images */
        public uint width;
        public uint height;

        public uint bufferSize;
        public uint writeIndex;
        public uint resizeCounter;


        /* resize */
        public uint resizeEventWrite;
        public uint resizeEventRead;

        /* mouse */
        public uint buttonEventWrite;
        public uint buttonEventRead;

        /* key */
        public uint keyEventWrite;
        public uint keyEventRead;

        /* text */
        public uint inputTextEventWrite;
        public uint inputTextEventRead;

        /* Events */
        public byte loadEventsWrite;
        public byte loadEventsRead;

        /* Events like begin, finish loading And On DOM Ready */
        public byte baseEventsWrite;
        public byte baseEventsRead;

        /* Load Url or html */
        public uint setUpEventWrite;
        public uint setUpEventRead;

        /* Execute js */
        public uint executeJSEventWrite;
        public uint executeJSEventRead;

        /* Message Console */
        public uint messageConsoleEventWrite;
        public uint messageConsoleEventRead;

        /* Message Emitted */
        public uint messageEmittedEventWrite;
        public uint messageEmittedEventRead;

        /* Post message */
        public uint postMessageEventWrite;
        public uint postMessageEventRead;
    }
}