using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Entities
{
    [Table("MarketLogEntries")]
    public class MarketLogEntry : EntityBase
    {
        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; }

        public String Text { get; set; }

        public MarketLogEntryTypes MarketLogEntryType { get; set; }

        [ForeignKey("TradingDayId")]
        public virtual TradingDay TradingDay { get; set; }
        public virtual int TradingDayId { get; set; }

        [ForeignKey("TradingAccountId")]
        public virtual TradingAccount TradingAccount { get; set; }
        public virtual int TradingAccountId { get; set; }

        [ForeignKey("MarketId")]
        public virtual Market Market { get; set; }
        public virtual int? MarketId { get; set; }

        [ForeignKey("StockId")]
        public virtual Stock Stock { get; set; }
        public virtual int? StockId { get; set; }

        [ForeignKey("ScreenshotDbId")]
        [InverseProperty("MarketLogEntries")]
        public virtual Screenshot ScreenshotDb { get; set; }
        public virtual int? ScreenshotDbId { get; set; }

        public class MarketLogEntryMapping : EntityTypeConfiguration<MarketLogEntry>
        {
            public MarketLogEntryMapping()
            {
                //HasRequired(m => m.ScreenshotDb).WithMany(m => m.MarketLogEntries);
            }
        }
    }
}
