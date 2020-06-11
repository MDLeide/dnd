using System.Windows;
using System.Windows.Media;

namespace DND.UI.Util
{
    static class Formatting
    {
        public static Format SoftLinkFormat { get; set; } =
            new Format() {Underline = true, Foreground = new SolidColorBrush(Colors.DarkSlateBlue)};

        public static Format BrokenLinkFormat { get; set; } =
            new Format() {Underline = true, Foreground = new SolidColorBrush(Colors.Red)};

        public static Format OpenTokenFormat { get; set; } =
            new Format() {Foreground = new SolidColorBrush(Colors.Red)};

        public static Format TokenFormat { get; set; } =
            new Format() {FontWeight = FontWeights.Bold};

        public static Format StringFormat { get; set; }
    }
}