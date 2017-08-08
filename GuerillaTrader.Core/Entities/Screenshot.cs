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
    [Table("Screenshots")]
    public class Screenshot : EntityBase
    {
        public byte[] Data { get; set; }

        public virtual ICollection<MarketLogEntry> MarketLogEntries { get; set; }

        public virtual ICollection<Trade> EntryTrades { get; set; }

        public virtual ICollection<Trade> ExitTrades { get; set; }
    }
}
