using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Entities
{
    [Table("Sectors")]
    public class Sector : EntityBase
    {
        public String Name { get; set; }
        public Decimal RecentPerf { get; set; }
        public Decimal PastPerf { get; set; }
        public Decimal PastPositivePerf { get; set; }
        public Decimal EffectiveTaxRate { get; set; }
    }

    [Table("Stocks")]
    public class Stock : Security
    {
        public Decimal Yield { get; set; }

        public int DividendYieldScore { get; set; }
        public int CashFlowScore { get; set; }
        public int RelativeValueScore { get; set; }
        public int TotalScore { get; set; }

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

        [ForeignKey("StockId")]
        public virtual ICollection<StockReport> StockReports { get; set; }
    }

    [Table("StockReports")]
    public class StockReport : EntityBase
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public Decimal Perf { get; set; }

        public bool FailedToRetrieveBars { get; set; }

        [ForeignKey("StockId")]
        public virtual Stock Stock { get; set; }
        public virtual int StockId { get; set; }

        [ForeignKey("StockReportId")]
        public virtual ICollection<StockBar> StockBars { get; set; }
    }

    [Table("StockBars")]
    public class StockBar : EntityBase
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

        [ForeignKey("StockReportId")]
        public virtual StockReport StockReport { get; set; }
        public virtual int StockReportId { get; set; }
    }
}
