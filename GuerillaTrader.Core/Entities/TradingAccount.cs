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
    public class TradingAccount : EntityBase
    {
        public String Name { get; set; }

        [DataType(DataType.Currency)]
        public Decimal InitialCapital { get; set; }

        [DataType(DataType.Currency)]
        public Decimal CurrentCapital { get; set; }

        [DataType(DataType.Currency)]
        public Decimal Commissions { get; set; }

        [DataType(DataType.Currency)]
        public Decimal ProfitLoss { get; set; }

        public Decimal TotalReturn { get; set; }

        public Decimal CAGR { get; set; }

        [DataType(DataType.Date)]
        public DateTime InceptionDate { get; set; }

        public bool Active { get; set; }

        [ForeignKey("TradingAccountId")]
        public virtual ICollection<Trade> Trades { get; set; }

        [ForeignKey("TradingAccountId")]
        public virtual ICollection<MarketLogEntry> MarketLogEntries { get; set; }

        [ForeignKey("TradingAccountId")]
        public virtual ICollection<MonteCarloSimulation> MonteCarloSimulations { get; set; }

        [ForeignKey("TradingAccountId")]
        public virtual ICollection<TradingAccountSnapshot> Snapshots { get; set; }
    }
}
