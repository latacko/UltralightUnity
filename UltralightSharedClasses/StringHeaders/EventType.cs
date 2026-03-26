using System;

namespace UltralightSharedClasses.StringHeaders
{
    public enum EventType : byte
    {
        TitleChanged,
        UrlChanged,
        LoadFailed,
        Message,
        Set_HTML_OR_URL,
        FileExist,
        FileOpen,
        EvaluateScript
    }

    public static class GetHeaderHelper
    {
        public static Type? Get(EventType type)
        {
            switch (type)
            {
                case EventType.LoadFailed:
                    return typeof(LoadFieldHeader);
                case EventType.UrlChanged:
                    return null;
                case EventType.Set_HTML_OR_URL:
                    return typeof(SetUpHTMLORURLHeader);
                case EventType.FileExist:
                    return null;
                case EventType.FileOpen:
                    return null;
                case EventType.EvaluateScript:
                    return null;
                default:
                    throw new System.Exception("Tried loading string file with unknown detailHeader type. Declared type: " + type);
            }
        }
    }
}