using System;
using System.Globalization;
using System.Windows.Data;

namespace MVVM.Converters
{
    public class NullToBoolConverter : IValueConverter
    {
        public bool IsNullValue { get; set; } = true;
        public bool IsNotNullValue { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return IsNullValue;
            return IsNotNullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}