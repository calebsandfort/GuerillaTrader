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
    }
}
