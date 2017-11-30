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
    public class TradingAccountSnapshot : EntityBase
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

        [ForeignKey("TradingAccountId")]
        public virtual TradingAccount TradingAccount { get; set; }
        public virtual int TradingAccountId { get; set; }

        public TradingAccountSnapshot() { }

        public TradingAccountSnapshot(DateTime date, TradingAccountSnapshot snapshot)
        {
            this.Date = date;
            this.CurrentCapital = snapshot.CurrentCapital;
            this.Commissions = snapshot.Commissions;
            this.ProfitLoss = snapshot.ProfitLoss;
            this.TotalReturn = snapshot.TotalReturn;
            this.CAGR = snapshot.CAGR;
        }
    }
}
