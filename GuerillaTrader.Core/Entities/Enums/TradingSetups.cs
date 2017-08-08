using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Framework;

namespace GuerillaTrader.Entities
{
    [Flags]
    public enum TradingSetups
    {
        None = 0,
        [EnumDisplay("Congestion Zone")]
        CongestionZone = 1,
        [EnumDisplay("Congestion Breakout Failure")]
        CongestionBreakoutFailure = 2,
        [EnumDisplay("Trend Bar Failure")]
        TrendBarFailure = 4,
        [EnumDisplay("Anti-Climax")]
        AntiClimax = 8,
        [EnumDisplay("Deceleration")]
        Deceleration = 16,
        [EnumDisplay("Anxiety Zone")]
        AnxietyZone = 32,
        [EnumDisplay("Surge")]
        Surge = 64,
        [EnumDisplay("Pressure Zone")]
        PressureZone = 128
    }
}
