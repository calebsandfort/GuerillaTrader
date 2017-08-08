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
    public class TradingDay : EntityBase
    {
        [DataType(DataType.DateTime)]
        public DateTime Day { get; set; }

        [ForeignKey("TradingDayId")]
        public virtual ICollection<MarketLogEntry> MarketLogEntries { get; set; }
    }
}
