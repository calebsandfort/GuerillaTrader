using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Framework
{
    public static class Extensions
    {
        public static T Percentile<T>(this List<T> data, Decimal k)
        {
            data = data.OrderBy(x => x).ToList();
            int index = ((int)Math.Ceiling(k * data.Count)) - 1;
            if (index == 0) index = 1;
            return data.Take(index).Last();
        }

        public static int? GetNullableValue(int val)
        {
            if (val > 0) return val;
            else return null;
        }

        public static List<Decimal> Split(this Decimal val)
        {
            return val.ToString("N3").Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => Decimal.Parse(x)).ToList();
        }

        public static Decimal FromFraction(this Decimal val, Decimal demoninator)
        {
            List<Decimal> split = val.Split();
            return split[0] + (split[1] / demoninator);
        }

        /// <summary>
        /// Extension method for faster string to decimal conversion.
        /// </summary>
        /// <param name="str">String to be converted to positive decimal value</param>
        /// <remarks>
        /// Leading and trailing whitespace chars are ignored
        /// </remarks>
        /// <returns>Decimal value of the string</returns>
        public static decimal ToDecimal(this string str)
        {
            long value = 0;
            var decimalPlaces = 0;
            var hasDecimals = false;
            var index = 0;
            var length = str.Length;

            while (index < length && char.IsWhiteSpace(str[index]))
            {
                index++;
            }

            var isNegative = index < length && str[index] == '-';
            if (isNegative)
            {
                index++;
            }

            while (index < length)
            {
                var ch = str[index++];
                if (ch == '.')
                {
                    hasDecimals = true;
                    decimalPlaces = 0;
                }
                else if (char.IsWhiteSpace(ch))
                {
                    break;
                }
                else
                {
                    value = value * 10 + (ch - '0');
                    decimalPlaces++;
                }
            }

            var lo = (int)value;
            var mid = (int)(value >> 32);
            return new decimal(lo, mid, 0, isNegative, (byte)(hasDecimals ? decimalPlaces : 0));
        }

        /// <summary>
        /// Extension method for faster string to Int32 conversion.
        /// </summary>
        /// <param name="str">String to be converted to positive Int32 value</param>
        /// <remarks>Method makes some assuptions - always numbers, no "signs" +,- etc.</remarks>
        /// <returns>Int32 value of the string</returns>
        public static int ToInt32(this string str)
        {
            int value = 0;
            for (var i = 0; i < str.Length; i++)
            {
                value = value * 10 + (str[i] - '0');
            }
            return value;
        }

        public static Decimal Return(this Decimal start, Decimal end)
        {
            return (end - start) / start;
        }

        public static Decimal CAGR(this Decimal start, Decimal end, int period)
        {
            return (Decimal)(Math.Pow((Double)end / (Double)start, (1.0/((Double)period/365.0))) - 1.0);
        }
    }
}
