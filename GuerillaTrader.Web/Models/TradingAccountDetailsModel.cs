using GuerillaTrader.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuerillaTrader.Web.Models
{
    public class TradingAccountDetailsModel
    {
        public TradingAccountDto TradingAccount { get; set; }
        public List<TradeDto> Trades { get; set; }

        public List<TradingAccountChartItem> RChartItems { get; set; }
        public List<TradingAccountChartItem> DrawdownChartItems { get; set; }

        public Decimal R { get; set; }
        public Decimal MaxDrawdown { get; set; }

        public TradingAccountDetailsModel()
        {
            this.RChartItems = new List<TradingAccountChartItem>();
            this.DrawdownChartItems = new List<TradingAccountChartItem>();
        }

        public void SetPerfFields()
        {
            Decimal currentBalance = this.TradingAccount.InitialCapital;
            Decimal winningTotal = 0m;
            Decimal losingTotal = 0m;

            Decimal maxDrawdown = 0m;
            Decimal maxBalance = currentBalance;

            foreach (TradeDto trade in this.Trades.OrderBy(x => x.ExitDate))
            {
                if (trade.AdjProfitLoss > 0m) winningTotal += trade.AdjProfitLoss;
                else losingTotal += Math.Abs(trade.AdjProfitLoss);

                currentBalance += trade.AdjProfitLoss;

                if (currentBalance > maxBalance) maxBalance = currentBalance;

                Decimal drawdown = maxBalance - currentBalance;
                if (drawdown > maxDrawdown) maxDrawdown = drawdown;
            }

            this.R = winningTotal / losingTotal;
            this.MaxDrawdown = maxDrawdown / maxBalance;
        }

        public void FillChartItems()
        {
            DateTime startOfWeek = this.TradingAccount.InceptionDate.Date;
            DateTime endOfWeek = startOfWeek.AddDays(6);

            Decimal rollingBalance = this.TradingAccount.InitialCapital;

            while(this.Trades.Any(x => x.ExitDate > startOfWeek && x.ExitDate < endOfWeek))
            {
                Decimal currentBalance = rollingBalance;
                Decimal winningTotal = 0m;
                Decimal losingTotal = 0m;

                Decimal maxDrawdown = 0m;
                Decimal maxBalance = currentBalance;

                foreach (TradeDto trade in this.Trades.Where(x => x.ExitDate > startOfWeek && x.ExitDate < endOfWeek).OrderBy(x => x.ExitDate))
                {
                    if (trade.AdjProfitLoss > 0m) winningTotal += trade.AdjProfitLoss;
                    else losingTotal += Math.Abs(trade.AdjProfitLoss);

                    currentBalance += trade.AdjProfitLoss;

                    if (currentBalance > maxBalance) maxBalance = currentBalance;

                    Decimal drawdown = maxBalance - currentBalance;
                    if (drawdown > maxDrawdown) maxDrawdown = drawdown;
                }

                this.RChartItems.Add(new TradingAccountChartItem { Display = $"{startOfWeek:M/d} - {endOfWeek:M/d}", Value = winningTotal / losingTotal });
                this.DrawdownChartItems.Add(new TradingAccountChartItem { Display = $"{startOfWeek:M/d} - {endOfWeek:M/d}", Value = Math.Abs(maxDrawdown / maxBalance) });

                startOfWeek = startOfWeek.AddDays(7);
                endOfWeek = endOfWeek.AddDays(7);
                rollingBalance = currentBalance;
            }
        }
    }
}