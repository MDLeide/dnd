using System;
using System.Globalization;
using System.Windows.Data;

namespace Cashew.UI.WPF.Converters
{
    public class StringConverter : IValueConverter
    {
        public bool ToLower { get; set; }
        public string StartingText { get; set; }
        public string EndingText { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string s))
                return value;

            if (ToLower)
                s = s.ToLower();

            return $"{StartingText}{s}{EndingText}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
