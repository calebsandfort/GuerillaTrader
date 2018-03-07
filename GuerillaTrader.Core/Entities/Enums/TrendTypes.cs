using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Framework;

namespace GuerillaTrader.Entities
{
    public enum TrendTypes
    {
        None,
        [EnumDisplay("Bearish")]
        Bearish,
        [EnumDisplay("Neutral")]
        Neutral,
        [EnumDisplay("Bullish")]
        Bullish
    }
}
