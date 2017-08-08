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
    [Table("Markets")]
    public class Market : EntityBase
    {
        public String Name { get; set; }
        public String Symbol { get; set; }

        [DataType(DataType.Currency)]
        public Decimal TickValue { get; set; }
        public Decimal TickSize { get; set; }

        [DataType(DataType.Currency)]
        public Decimal InitialMargin { get; set; }

        public int MTT { get; set; }

        public bool Active { get; set; }

        [DataType(DataType.Currency)]
        [NotMapped]
        public Decimal PointValue
        {
            get
            {
                return 1.0m / this.TickSize * this.TickValue;
            }
        }

        [ForeignKey("MarketId")]
        public virtual ICollection<Trade> Trades { get; set; }
    }
}
