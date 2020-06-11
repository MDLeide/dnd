using System;
using System.Collections.Generic;
using System.Windows;

namespace Cashew.UI.WPF
{
    public static class Interaction
    {
        public static int DoubleClickMilliseconds { get; } = 350;

        public static bool DiscardUnsavedChangesQuery()
            => Query("Unsaved Changes", "There are unsaved changes. Do you wish to discard them?");

        public static bool QueryWarn(string title, string message)
            => Query(title, message, MessageBoxImage.Exclamation);

        public static bool Query(string title, string message, MessageBoxImage image = MessageBoxImage.Question) =>
            MessageBox.Show(message, title, MessageBoxButton.YesNo, image) == MessageBoxResult.Yes;


        public static bool Confirm(string title, string message, MessageBoxImage image = MessageBoxImage.Question) =>
            MessageBox.Show(message, title, MessageBoxButton.OKCancel, image) == MessageBoxResult.OK;


        public static void DisplayError(string title, string message)
            => Display(title, message, MessageBoxImage.Error);

        public static void Display(string title, string message, MessageBoxImage image = MessageBoxImage.Information) =>
            MessageBox.Show(message, title, MessageBoxButton.OK, image);


        static readonly Dictionary<Guid, DateTime> LastClicked = new Dictionary<Guid, DateTime>();
        public static bool IsDoubleClick(Guid guid)
        {
            var now = DateTime.Now;
            if (LastClicked.ContainsKey(guid))
                LastClicked.Add(guid, DateTime.MinValue);
            
            var last = LastClicked[guid];
            LastClicked[guid] = now;
            return (now - last).TotalMilliseconds <= DoubleClickMilliseconds;
        }
    }
}