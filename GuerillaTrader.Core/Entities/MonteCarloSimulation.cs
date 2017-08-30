using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Entities
{
    [Table("MonteCarloSimulations")]
    public class MonteCarloSimulation : EntityBase
    {
        public DateTime TimeStamp { get; set; }
        public int NumberOfTradesInSample { get; set; }
        public int NumberOfTradesPerIteration { get; set; }
        public int NumberOfIterations { get; set; }
        public Decimal CumulativeProfitK { get; set; }
        public Decimal CumulativeProfit { get; set; }
        public bool TradingEdge { get; set; }
        public Decimal ConsecutiveLossesK { get; set; }
        public int ConsecutiveLosses { get; set; }
        public Decimal MaxDrawdownK { get; set; }
        public Decimal MaxDrawdown { get; set; }
        public Decimal AccountSize { get; set; }
        public Decimal RuinPoint { get; set; }
        public Decimal MaxDrawdownMultiple { get; set; }
        public Decimal OneContractFunds { get; set; }
        public int MaxContracts { get; set; }
        public String MarketMaxContractsJson { get; set; }

        [ForeignKey("TradingAccountId")]
        public virtual TradingAccount TradingAccount { get; set; }
        public virtual int TradingAccountId { get; set; }

        public static Double GetTradesPerDay(DateTime start, DateTime end, int totalTrades)
        {
            TimeSpan range = end.Date - start.Date;
            int tradingDays = 0;
            DateTime currentDate;

            for(int i = 0; i <= range.Days; i++)
            {
                currentDate = start.AddDays(i);
                if(currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    tradingDays += 1;
                }
            }

            return (Double)totalTrades / (Double)tradingDays;
        }
    }
}
