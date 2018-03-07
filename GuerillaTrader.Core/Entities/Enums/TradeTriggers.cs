using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Framework;

namespace GuerillaTrader.Entities
{
    public enum TradeTriggers
    {
        None,
        [EnumDisplay("Signals")]
        Signals,
        [EnumDisplay("Support")]
        Support,
        [EnumDisplay("Resistance")]
        Resistance,
        [EnumDisplay("Bullish Breakout")]
        BullishBreakout,
        [EnumDisplay("Bearish Breakout")]
        BearishBreakout
    }
}
