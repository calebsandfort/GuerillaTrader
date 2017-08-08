using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Framework;

namespace GuerillaTrader.Entities
{
    public enum TradeExitReasons
    {
        [EnumDisplay("None")]
        None,
        [EnumDisplay("Target Hit")]
        TargetHit,
        [EnumDisplay("Stop Loss Hit")]
        StopLossHit,
        [EnumDisplay("Reversal Signal")]
        ReversalSignal,
        [EnumDisplay("End of Day")]
        EndOfDay
    }
}
