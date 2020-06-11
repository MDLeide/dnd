using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace DND.UI.Util
{
    class Format
    {
        public string Prepend { get; set; }
        public string Append { get; set; }

        public FontFamily FontFamily { get; set; }
        public double? Size { get; set; }

        public FontWeight? FontWeight { get; set; }
        public bool Underline { get; set; }

        public Brush Foreground { get; set; }
        public Brush Background { get; set; }

        public Inline FormatRun(string val)
        {
            Inline inline = new Run($"{Prepend ?? string.Empty}{val}{Append ?? string.Empty}");
            if (Underline)
                inline = new Underline(inline);

            if (FontFamily != null)
                inline.FontFamily = FontFamily;
            if (Size.HasValue)
                inline.FontSize = Size.Value;
            if (FontWeight.HasValue)
                inline.FontWeight = FontWeight.Value;
            if (Foreground != null)
                inline.Foreground = Foreground;
            if (Background != null)
                inline.Background = Background;

            return inline;
        }
    }
}