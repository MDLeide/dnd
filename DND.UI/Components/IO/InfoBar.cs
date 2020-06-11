using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DND.UI.Components.IO
{
    static class InfoBar
    {
        //todo: set limit on all entries count

        static readonly List<InfoBarEntry> Entries = new List<InfoBarEntry>();

        public static event EventHandler<InfoBarMessageChanged> LastEntryChanged;

        public static InfoBarEntry LastEntry { get; private set; }
        public static IReadOnlyCollection<InfoBarEntry> AllEntries { get; } = new ReadOnlyCollection<InfoBarEntry>(Entries);

        public static void Warn(string message) =>
            AddMessage(message, InfoBarEntry.EntryType.Warning);

        public static void Error(string message) =>
            AddMessage(message, InfoBarEntry.EntryType.Error);

        public static void Info(string message) =>
            AddMessage(message, InfoBarEntry.EntryType.Info);

        public static void AddMessage(string message, InfoBarEntry.EntryType type)
        {
            var entry = new InfoBarEntry(message, type);
            var eventArgs = new InfoBarMessageChanged(LastEntry, entry);
            Entries.Add(entry);
            LastEntry = entry;
            LastEntryChanged?.Invoke(null, eventArgs);
        }
    }
}
