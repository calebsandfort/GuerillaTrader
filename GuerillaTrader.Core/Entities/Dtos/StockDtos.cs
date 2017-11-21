using Abp.AutoMapper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GuerillaTrader.Framework;

namespace GuerillaTrader.Entities.Dtos
{
    [AutoMap(typeof(Sector))]
    public class SectorDto : EntityDtoBase
    {
        public String Name { get; set; }
        public Decimal RecentPerf { get; set; }
        public Decimal PastPerf { get; set; }
        public Decimal PastPositivePerf { get; set; }
        public Decimal EffectiveTaxRate { get; set; }
    }

    [AutoMap(typeof(Stock))]
    public class StockDto : EntityDtoBase
    {
        public String Name { get; set; }
        public String Symbol { get; set; }
        public Decimal Yield { get; set; }

        public int DividendYieldScore { get; set; }
        public int CashFlowScore { get; set; }
        public int RelativeValueScore { get; set; }
        public int TotalScore { get; set; }

        [DataType(DataType.Currency)]
        public Decimal Price { get; set; }
        [DataType(DataType.Currency)]
        public Decimal IdealValue { get; set; }

        public String Sector { get; set; }

        public Decimal RecentPerf { get; set; }
        public Decimal PastPerf { get; set; }
        public Decimal PastPositivePerf { get; set; }
        public bool FailedToRetrieveBars { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NextEarningsDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExDividendDate { get; set; }

        [DataType(DataType.Currency)]
        public Decimal? TargetPrice { get; set; }

        public int? AvgVolume { get; set; }
        public Decimal? ADV { get; set; }

        public virtual ICollection<StockReportDto> StockReports { get; set; }

        public void SetStats(int period)
        {
            List<StockReportDto> temp = new List<StockReportDto>();

            foreach (StockReportDto report in this.StockReports)
            {
                report.SetStats(period);
                if (!report.FailedToRetrieveBars)
                {
                    temp.Add(report);
                }
            }

            this.StockReports = temp.OrderByDescending(x => x.StartDate).ToList();

            if (this.StockReports.Count == 0)
            {
                this.RecentPerf = 0m;
                this.PastPerf = 0m;
                this.PastPositivePerf = 0m;
                this.FailedToRetrieveBars = true;
            }
            else
            {
                StockReportDto recentReport = this.StockReports.First();
                List<StockReportDto> pastReports = this.StockReports.Skip(1).ToList();
                this.RecentPerf = recentReport.Perf;
                this.PastPerf = pastReports.Average(x => x.Perf);
                this.PastPositivePerf = (Decimal)pastReports.Count(x => x.Perf > 0m) / (Decimal)pastReports.Count();
                this.FailedToRetrieveBars = false;
            }
        }
    }

    [AutoMap(typeof(StockReport))]
    public class StockReportDto : EntityDtoBase
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public virtual int StockId { get; set; }

        public virtual ICollection<StockBarDto> StockBars { get; set; }

        public Decimal Perf { get; set; }

        public bool FailedToRetrieveBars { get; set; }

        public void SetStats(int period)
        {
            this.StockBars = this.StockBars.OrderBy(x => x.Date).ToList();
            if (this.StockBars.Count > 0)
            {
                Decimal startPrice = this.StockBars.First().Close;
                Decimal endPrice = this.StockBars.Last().Close;

                this.Perf = startPrice.Return(endPrice);
            }
        }
    }

    [AutoMap(typeof(StockBar))]
    public class StockBarDto : EntityDtoBase
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Currency)]
        public Decimal Open { get; set; }

        [DataType(DataType.Currency)]
        public Decimal High { get; set; }

        [DataType(DataType.Currency)]
        public Decimal Low { get; set; }

        [DataType(DataType.Currency)]
        public Decimal Close { get; set; }

    }
}
