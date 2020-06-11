namespace DND.UI.Components.IO
{
    class InfoBarEntry
    {
        public InfoBarEntry(string message, EntryType type)
        {
            Message = message;
            Type = type;
        }

        public string Message { get; }
        public EntryType Type { get; }

        public enum EntryType
        {
            Info,
            Warning,
            Error
        }
    }
}