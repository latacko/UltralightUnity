namespace UltralightSharedClasses.Structs
{
    public struct ChunksData
    {
        public const int MOUSE_EVENT_CHUNKS = 128;
        public const int KEY_EVENT_CHUNKS = 10;
        public const int TEXT_EVENT_CHUNKS = 10;
        public const int RESIZE_EVENT_CHUNKS = 128;

        public const int MAX_BYTES_PER_CHUNK = 50;
        public const int LOAD_EVENT_CHUNKS = 30;
        public const int SETUP_HTML_OR_URL = 3;
        public const int BASE_EVENT_CHUNKS = 3;
        public const int REQUEST_VIEW_EVENT_CHUNKS = 128;
        public const int DESTORY_VIEW_EVENT_CHUNKS = 128;
        public const int FILE_OPEN_CHUNKS = 20;
        public const int FILE_EXIST_CHUNKS = 20;
    }
}