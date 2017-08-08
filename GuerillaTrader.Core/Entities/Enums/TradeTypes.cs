using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Framework;

namespace GuerillaTrader.Entities
{
    public enum TradeTypes
    {
        None,
        [EnumDisplay("Long")]
        Long,
        [EnumDisplay("Short")]
        Short
    }
}
