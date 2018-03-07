using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Framework;

namespace GuerillaTrader.Entities
{
    public enum PerformanceCycleTypes
    {
        None = 0,
        [EnumDisplay("Weekly")]
        Weekly,
        [EnumDisplay("Block")]
        Block,
        [EnumDisplay("Hour")]
        Hour,
        [EnumDisplay("Day of Week")]
        DayOfWeek,
        [EnumDisplay("Month")]
        Month,
        [EnumDisplay("Day")]
        Day,
        [EnumDisplay("Security")]
        Security,
        [EnumDisplay("All")]
        All
    }
}
