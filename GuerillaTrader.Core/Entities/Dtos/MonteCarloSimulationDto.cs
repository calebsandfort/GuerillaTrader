using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using GuerillaTrader.Framework;
using Abp.Timing;
using GuerillaTrader.Shared;
using GuerillaTrader.Shared.Dtos;

namespace GuerillaTrader.Entities.Dtos
{
    public class MonteCarloSimulationDto : EntityDtoBase
    {
        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; }

        [UIHint("MyIntStatic")]
        [Display(Name = "Sample Size")]
        public int NumberOfTradesInSample { get; set; }

        [UIHint("MyInt")]
        [Display(Name = "Trades/Iteration")]
        public int NumberOfTradesPerIteration { get; set; }

        [UIHint("MyInt")]
        [Display(Name = "Iterations")]
        public int NumberOfIterations { get; set; }

        [UIHint("MyPercentage")]
        [Display(Name = "Cume Profit K")]
        public Decimal CumulativeProfitK { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Cume Profit")]
        public Decimal CumulativeProfit { get; set; }

        [Display(Name = "Trading Edge")]
        public bool TradingEdge { get; set; }

        [UIHint("MyPercentage")]
        [Display(Name = "Cons Losses K")]
        public Decimal ConsecutiveLossesK { get; set; }

        [Display(Name = "Cons Losses")]
        public int ConsecutiveLosses { get; set; }

        [UIHint("MyPercentage")]
        [Display(Name = "Max DD K")]
        public Decimal MaxDrawdownK { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Max DD")]
        public Decimal MaxDrawdown { get; set; }

        [DataType(DataType.Currency)]
        [UIHint("MyCurrency")]
        [Display(Name = "Account Size")]
        public Decimal AccountSize { get; set; }

        [DataType(DataType.Currency)]
        [UIHint("MyCurrency")]
        [Display(Name = "Ruin Point")]
        public Decimal RuinPoint { get; set; }

        [UIHint("MyDecimal")]
        [Display(Name = "Max DD Mult")]
        public Decimal MaxDrawdownMultiple { get; set; }

        [DataType(DataType.Currency)]
        [UIHint("MyCurrency")]
        [Display(Name = "One Contract Funds")]
        public Decimal OneContractFunds { get; set; }

        [Display(Name = "Max Contracts")]
        public int MaxContracts { get; set; }

        public List<MarketMaxContracts> MarketMaxContractsList { get; set; }

        [Display(Name = "Account")]
        public String TradingAccount { get; set; }
        [UIHint("TradingAccount")]
        [Display(Name = "Account")]
        public virtual int TradingAccountId { get; set; }

        public MonteCarloSimulationDto()
        {
            this.MarketMaxContractsList = new List<MarketMaxContracts>();
        }

        public void Simulate(List<Trade> sample, List<Market> markets, IConsoleHubProxy consoleHubProxy)
        {
            Random random = new Random(Clock.Now.Millisecond);
            int sampleSize = sample.Count;
            List<MonteCarloSimulationIteration> iterations = new List<MonteCarloSimulationIteration>();

            for(int i = 0; i < this.NumberOfIterations; i++)
            {
                consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Simulation iteration {i+1} of {this.NumberOfIterations}"));

                MonteCarloSimulationIteration iteration = new MonteCarloSimulationIteration();
                for(int j = 0; j < this.NumberOfTradesPerIteration; j++)
                {
                    MonteCarloSimulationTrade trade = new MonteCarloSimulationTrade { NetProfit = sample[random.Next(sampleSize)].ProfitLossPerContract };
                    iteration.Trades.Add(trade);
                    trade.CumulativeProfit = iteration.Trades.Sum(x => x.NetProfit);
                    trade.Drawdown = trade.CumulativeProfit - iteration.Trades.Max(x => x.CumulativeProfit);

                    if(trade.NetProfit < 0)
                    {
                        if(iteration.Trades.Count == 1)
                        {
                            trade.ConsecutiveLosses = 1;
                        }
                        else
                        {
                            trade.ConsecutiveLosses = iteration.Trades[iteration.Trades.Count - 2].ConsecutiveLosses + 1;
                        }
                    }
                }

                iteration.CumulativeProfit = iteration.Trades.Last().CumulativeProfit;
                iteration.MaxDrawdown = iteration.Trades.Min(x => x.Drawdown);
                iteration.ConsecutiveLosses = iteration.Trades.Max(x => x.ConsecutiveLosses);
                iterations.Add(iteration);
            }

            this.CumulativeProfit = Extensions.Percentile<Decimal>(iterations.Select(x => x.CumulativeProfit).ToList(), 1.0m - this.CumulativeProfitK);
            this.TradingEdge = this.CumulativeProfit > 0;

            this.ConsecutiveLosses = Extensions.Percentile<int>(iterations.Select(x => x.ConsecutiveLosses).ToList(), this.ConsecutiveLossesK);
            this.MaxDrawdown = Extensions.Percentile<Decimal>(iterations.Select(x => x.MaxDrawdown).ToList(), 1.0m - this.MaxDrawdownK);
            this.OneContractFunds = this.RuinPoint + (this.MaxDrawdownMultiple * Math.Abs(this.MaxDrawdown));
            this.MaxContracts = (int)Math.Floor(this.AccountSize/this.OneContractFunds);

            foreach(Market market in markets)
            {
                this.MarketMaxContractsList.Add(new MarketMaxContracts { Symbol = market.Symbol, Size = (int)Math.Floor(this.AccountSize / (market.InitialMargin + 10m + (this.MaxDrawdownMultiple * Math.Abs(this.MaxDrawdown)))) });
            }
        }
    }

    public class MonteCarloSimulationIteration
    {
        public Decimal CumulativeProfit { get; set; }
        public Decimal MaxDrawdown { get; set; }
        public int ConsecutiveLosses { get; set; }
        public List<MonteCarloSimulationTrade> Trades { get; set; }

        public MonteCarloSimulationIteration()
        {
            this.Trades = new List<MonteCarloSimulationTrade>();
        }
    }

    public class MonteCarloSimulationTrade
    {
        public Decimal NetProfit { get; set; }
        public Decimal CumulativeProfit { get; set; }
        public Decimal Drawdown { get; set; }
        public int ConsecutiveLosses { get; set; }

    }

    public class MarketMaxContracts
    {
        public String Symbol { get; set; }
        public int Size { get; set; }
    }
}
