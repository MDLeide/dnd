using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DND.UI.Converters
{
    class IndexCardControlDimensionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is bool vertical))
            {
                if (!(parameter is string boolString))
                    return DependencyProperty.UnsetValue;

                if (!bool.TryParse(boolString, out var r))
                    return DependencyProperty.UnsetValue;

                vertical = r;
            }

            if (values == null ||
                values.Length != 4 ||
                !(values[0] is double cardDimension) ||
                !(values[1] is double imageDiameter) ||
                !(values[2] is double additionalDimension) ||
                !(values[3] is double offset))
                return DependencyProperty.UnsetValue;

            if (vertical)
            {
                if (offset <= 0)
                    return cardDimension + additionalDimension + Math.Abs(offset);

                return cardDimension + additionalDimension +
                       Math.Max(offset - (cardDimension + additionalDimension), 0);
            }

            if (offset > 0)
                return cardDimension + additionalDimension + offset;

            return cardDimension + additionalDimension +
                   Math.Max(Math.Abs(offset) - (cardDimension + additionalDimension), 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
