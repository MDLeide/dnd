using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MVVM.Converters
{
    public class CollectionStateToVisibilityConverter : IValueConverter
    {
        public Visibility CollectionIsNull { get; set; } = Visibility.Hidden;
        public Visibility IsEmpty { get; set; } = Visibility.Hidden;
        public Visibility IsNotEmpty { get; set; } = Visibility.Visible;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return CollectionIsNull;

            if (!(value is IEnumerable e))
                return null;

            foreach (var item in e)
                return IsNotEmpty;

            return IsEmpty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}