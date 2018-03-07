using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Entities
{
    public class PerformanceCycle : EntityBase
    {
        public int Position { get; set; }
        public PerformanceCycleTypes CycleType { get; set; }
        public String Display { get; set; }
        public Decimal R { get; set; }
        public Decimal MaxDrawdown { get; set; }
        public int WinningTrades { get; set; }
        public int LosingTrades { get; set; }
        public int TotalTrades { get; set; }
        public Decimal SuccessRate { get; set; }
        public Decimal PPC { get; set; }

        public Decimal StartCapital { get; set; }
        public Decimal EndCapital { get; set; }
        public Decimal ProfitLoss { get; set; }

        [ForeignKey("TradingAccountId")]
        public virtual TradingAccount TradingAccount { get; set; }
        public virtual int TradingAccountId { get; set; }

        //Weekly,
        //Block,
        //Security,
        //Day,
        //Hour

        public static List<PerformanceCycle> BuildLists(TradingAccount tradingAccount)
        {
            List<PerformanceCycle> cycles = new List<PerformanceCycle>();
            cycles.AddRange(BuildWeeklyList(tradingAccount));
            cycles.AddRange(BuildSecurityList(tradingAccount));
            cycles.AddRange(BuildMonthList(tradingAccount));
            cycles.AddRange(BuildDayOfWeekList(tradingAccount));
            cycles.AddRange(BuildHourList(tradingAccount));
            cycles.AddRange(BuildDayList(tradingAccount));
            cycles.AddRange(BuildAllList(tradingAccount));

            return cycles;
        }

        private static List<PerformanceCycle> BuildWeeklyList(TradingAccount tradingAccount)
        {
            DateTime startOfWeek = tradingAccount.InceptionDate.Date;
            DateTime endOfWeek = startOfWeek.AddDays(6);
            Dictionary<String, List<Trade>> roughCycles = new Dictionary<string, List<Trade>>();

            while (tradingAccount.Trades.Any(x => x.ExitDate > startOfWeek && x.ExitDate < endOfWeek))
            {
                roughCycles.Add($"{startOfWeek:M/d} - {endOfWeek:M/d}", tradingAccount.Trades.Where(x => x.ExitDate > startOfWeek && x.ExitDate < endOfWeek).OrderBy(x => x.ExitDate).ToList());
                startOfWeek = startOfWeek.AddDays(7);
                endOfWeek = endOfWeek.AddDays(7);
            }

            return BuildList(tradingAccount.InitialCapital, roughCycles, PerformanceCycleTypes.Weekly);
        }

        private static List<PerformanceCycle> BuildSecurityList(TradingAccount tradingAccount)
        {
            Dictionary<String, List<Trade>> roughCycles = (from t in tradingAccount.Trades
                                                           group t by t.Market.Symbol into g
                                                           select new { key = g.Key, trades = g.OrderBy(x => x.EntryDate).ToList() })
                                                           .OrderBy(x => x.key).ToDictionary(x => x.key, x => x.trades);

            return BuildList(tradingAccount.InitialCapital, roughCycles, PerformanceCycleTypes.Security);
        }

        private static List<PerformanceCycle> BuildMonthList(TradingAccount tradingAccount)
        {
            Dictionary<String, List<Trade>> roughCycles = (from t in tradingAccount.Trades
                                                           group t by new { t.EntryDate.Year, t.EntryDate.Month } into g
                                                           select new { year = g.Key.Year, month = g.Key.Month, trades = g.OrderBy(x => x.EntryDate).ToList() })
                                                           .OrderBy(x => x.year).ThenBy(x => x.month).ToDictionary(x => $"{x.month}/{x.year}", x => x.trades);

            return BuildList(tradingAccount.InitialCapital, roughCycles, PerformanceCycleTypes.Month);
        }

        private static List<PerformanceCycle> BuildDayList(TradingAccount tradingAccount)
        {
            Dictionary<String, List<Trade>> roughCycles = new Dictionary<string, List<Trade>>();
            //roughCycles.Add($"{tradingAccount.InceptionDate:M/d/yyyy}", new List<Trade>());

            foreach(var pair in (from t in tradingAccount.Trades
                                 group t by new { t.EntryDate.Year, t.EntryDate.Month, t.EntryDate.Day } into g
                                 select new { year = g.Key.Year, month = g.Key.Month, day = g.Key.Day, trades = g.OrderBy(x => x.EntryDate).ToList() })
                                                           .OrderBy(x => x.year).ThenBy(x => x.month).ThenBy(x => x.day).ToDictionary(x => $"{x.month}/{x.day}/{x.year}", x => x.trades))
            {
                roughCycles.Add(pair.Key, pair.Value);
            }

            return BuildList(tradingAccount.InitialCapital, roughCycles, PerformanceCycleTypes.Day);
        }

        private static List<PerformanceCycle> BuildDayOfWeekList(TradingAccount tradingAccount)
        {
            Dictionary<String, List<Trade>> roughCycles = (from t in tradingAccount.Trades
                                                           group t by t.EntryDate.DayOfWeek into g
                                                           select new { key = g.Key, trades = g.OrderBy(x => x.EntryDate).ToList() })
                                                           .OrderBy(x => x.key).ToDictionary(x => x.key.ToString(), x => x.trades);

            return BuildList(tradingAccount.InitialCapital, roughCycles, PerformanceCycleTypes.DayOfWeek);
        }

        private static List<PerformanceCycle> BuildHourList(TradingAccount tradingAccount)
        {
            Dictionary<String, List<Trade>> roughCycles = (from t in tradingAccount.Trades
                                                           group t by t.EntryDate.Hour into g
                                                           select new { key = g.Key, trades = g.OrderBy(x => x.EntryDate).ToList() })
                                                           .OrderBy(x => x.key).ToDictionary(x => $"{x.key}:00 - {x.key + 1}:00", x => x.trades);

            return BuildList(tradingAccount.InitialCapital, roughCycles, PerformanceCycleTypes.Hour);
        }

        private static List<PerformanceCycle> BuildAllList(TradingAccount tradingAccount)
        {
            Dictionary<String, List<Trade>> roughCycles = new Dictionary<string, List<Trade>> { { "All", tradingAccount.Trades.ToList() } };

            return BuildList(tradingAccount.InitialCapital, roughCycles, PerformanceCycleTypes.All);
        }

        private static List<PerformanceCycle> BuildList(Decimal startingBalance, Dictionary<String, List<Trade>> roughCycles, PerformanceCycleTypes performanceCycleType)
        {
            List<PerformanceCycle> cycles = new List<PerformanceCycle>();
            Decimal rollingBalance = startingBalance;

            int position = 1;
            foreach(String key in roughCycles.Keys)
            {
                Decimal currentBalance = rollingBalance;
                List<Trade> trades = roughCycles[key];

                PerformanceCycle cycle = new PerformanceCycle();
                cycle.Display = key;
                cycle.Position = position;
                cycle.CycleType = performanceCycleType;
                cycle.WinningTrades = trades.Count(x => x.AdjProfitLoss > 0);
                cycle.LosingTrades = trades.Count(x => x.AdjProfitLoss < 0);
                cycle.TotalTrades = trades.Count();
                cycle.SuccessRate = cycle.TotalTrades == 0 ? 0m : (Decimal)cycle.WinningTrades / (Decimal)cycle.TotalTrades;
                cycle.StartCapital = currentBalance;

                PerformanceCycle.CalculateRAndMaxDD(currentBalance, trades, (RAndMaxDDResult result) =>
                {
                    cycle.R = result.R;
                    cycle.MaxDrawdown = result.MaxDrawdown;
                }, out rollingBalance);

                cycle.EndCapital = rollingBalance;
                cycle.ProfitLoss = cycle.EndCapital - cycle.StartCapital;
                cycle.PPC = trades.Count == 0 ? 0m : (trades.Sum(x => x.AdjProfitLoss)/ trades.Sum(x => x.Size));
                cycles.Add(cycle);
                //rollingBalance = currentBalance;
                position += 1;
            }

            return cycles;
        }

        public static void CalculateRAndMaxDD(Decimal startingBalance, List<Trade> trades, Action<RAndMaxDDResult> setResults, out Decimal rollingBalance)
        {
            Decimal winningTotal = 0m;
            Decimal losingTotal = 0m;

            Decimal maxDrawdown = 0m;
            Decimal maxBalance = startingBalance;

            foreach (Trade trade in trades.OrderBy(x => x.ExitDate))
            {
                if (trade.AdjProfitLoss > 0m) winningTotal += trade.AdjProfitLoss;
                else losingTotal += Math.Abs(trade.AdjProfitLoss);

                startingBalance += trade.AdjProfitLoss;

                if (startingBalance > maxBalance) maxBalance = startingBalance;

                Decimal drawdown = maxBalance - startingBalance;
                if (drawdown > maxDrawdown) maxDrawdown = drawdown;
            }

            rollingBalance = startingBalance;

            setResults(new RAndMaxDDResult { R = losingTotal == 0m ? trades.Count(x => x.AdjProfitLoss > 0) : (winningTotal / losingTotal), MaxDrawdown = maxDrawdown / maxBalance });
        }
    }



    public class RAndMaxDDResult
    {
        public Decimal R { get; set; }
        public Decimal MaxDrawdown { get; set; }
    }
}
