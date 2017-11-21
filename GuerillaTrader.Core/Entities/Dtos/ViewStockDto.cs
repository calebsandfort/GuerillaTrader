using Abp.AutoMapper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GuerillaTrader.Framework;

namespace GuerillaTrader.Entities.Dtos
{
    [AutoMap(typeof(Stock))]
    public class ViewStockDto : EntityDtoBase
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

        [DataType(DataType.Date)]
        public DateTime? NextEarningsDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExDividendDate { get; set; }

        [DataType(DataType.Currency)]
        public Decimal? TargetPrice { get; set; }

        public String Sector { get; set; }

        public Decimal RecentPerf { get; set; }
        public Decimal PastPerf { get; set; }
        public Decimal PastPositivePerf { get; set; }

        public int? AvgVolume { get; set; }
        public Decimal? ADV { get; set; }

        public Decimal PastProjected
        {
            get
            {
                return this.Price + (this.Price * this.PastPerf);
            }
        }

        public bool FailedToRetrieveBars { get; set; }

        public virtual ICollection<ViewStockReportDto> StockReports { get; set; }

    }

    [AutoMap(typeof(StockReport))]
    public class ViewStockReportDto : EntityDtoBase
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public Decimal Perf { get; set; }

        public override string ToString()
        {
            return String.Format("{0:M/d/y} - {1:M/d/y} ({2:P2})", this.StartDate, this.EndDate, this.Perf);
        }

        public string DatesOnlyToString()
        {
            return String.Format("{0:M/d/y} - {1:M/d/y}", this.StartDate, this.EndDate);
        }
    }
}
