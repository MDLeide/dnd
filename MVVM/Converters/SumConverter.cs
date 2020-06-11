using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MVVM.Converters
{
    public class DoubleToThicknessConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 1 || values.Length == 3 || values.Length > 4)
                return DependencyProperty.UnsetValue;

            if (values.Length == 1)
            {
                if (values[0] is double d)
                    return new Thickness(d);
            }
            else if (values.Length == 2)
            {
                if (values[0] is double leftRight && 
                    values[1] is double topBottom)
                    return new Thickness(leftRight, topBottom, leftRight, topBottom);
            }
            else if (values.Length == 4)
            {
                if (values[0] is double left && 
                    values[1] is double top && 
                    values[2] is double right &&
                    values[3] is double bottom)
                    return new Thickness(left, top, right, bottom);
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MinConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || !values.Any())
                return DependencyProperty.UnsetValue;

            var min = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] is IComparable c)
                    if (c.CompareTo(min) < 0)
                        min = values[i];
            }

            return min;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MaxConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || !values.Any())
                return DependencyProperty.UnsetValue;

            var max = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] is IComparable c)
                    if (c.CompareTo(max) > 0)
                        max = values[i];
            }

            return max;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SumConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return Convert(values, targetType);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"MVVM.Converters.SumConverter: Error when trying to sum to {targetType?.Name}: {e.Message}");
                return DependencyProperty.UnsetValue;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        static object Convert(object[] values, Type targetType)
        {
            if (targetType == null || targetType == typeof(double))
                return SumToDouble(values);

            if (targetType == typeof(int))
                return SumToInt(values);

            return DependencyProperty.UnsetValue;
        }

        static double SumToDouble(object[] values)
        {
            var sum = 0d;
            foreach (var val in values)
                sum += System.Convert.ToDouble(val);
            return sum;
        }

        static int SumToInt(object[] values)
        {
            var sum = 0;
            foreach (var val in values)
                sum += System.Convert.ToInt32(val);
            return sum;
        }
    }
}
