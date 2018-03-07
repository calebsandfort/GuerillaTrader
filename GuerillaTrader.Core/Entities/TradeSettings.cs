using System;

namespace GuerillaTrader.Entities
{
    public class TradeSettings : EntityBase
    {
        public String Name { get; set; }
        public Decimal TickValue { get; set; }
        public int Contracts { get; set; }
        public int RewardTicks { get; set; }
        public int RiskTicks { get; set; }
        public Decimal RoundTripCommissions { get; set; }
    }
}
