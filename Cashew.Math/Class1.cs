using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Cashew
{
    public static class Math
    {
        public static double Mean(params double[] values) =>
            AverageImpl(values);

        public static double Mean(IEnumerable<double> values) =>
            AverageImpl(values.ToArray());

        public static double Average(params double[] values) =>
            AverageImpl(values);

        public static double Average(IEnumerable<double> values) =>
            AverageImpl(values.ToArray());

        static double AverageImpl(double[] values)
        {
            return values.Sum() / values.Length;
        }

        public static double Square(double value)
        {
            return value * value;
        }

        public static double SquareRoot(double value)
        {
            return System.Math.Sqrt(value);
        }

        public static double StandardDeviation(params double[] values)
            => StandardDeviationImpl(values);

        static double StandardDeviationImpl(double[] values)
        {
            var avg = AverageImpl(values);
            var summedDifference = 0d;
            for (int i = 0; i < values.Length; i++)
                summedDifference += Square(values[i] - avg);
            return SquareRoot(1 / (values.Length - 1d) * summedDifference);
        }
    }
}
