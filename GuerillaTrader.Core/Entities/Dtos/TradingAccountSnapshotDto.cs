using Abp.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuerillaTrader.Entities.Dtos
{
    [AutoMap(typeof(TradingAccountSnapshot))]
    public class TradingAccountSnapshotDto : EntityDtoBase
    {
        [DataType(DataType.Currency)]
        public Decimal CurrentCapital { get; set; }

        [DataType(DataType.Currency)]
        public Decimal Commissions { get; set; }

        [DataType(DataType.Currency)]
        public Decimal ProfitLoss { get; set; }

        public Decimal TotalReturn { get; set; }

        public Decimal CAGR { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        //[ForeignKey("TradingAccountId")]
        //public virtual TradingAccount TradingAccount { get; set; }
        //public virtual int TradingAccountId { get; set; }
    }
}
