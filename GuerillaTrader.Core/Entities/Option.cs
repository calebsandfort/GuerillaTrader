using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Entities
{
    [Table("Options")]
    public class Option : Security
    {
        [DataType(DataType.Date)]
        public DateTime Expiry { get; set; }

        [DataType(DataType.Currency)]
        public Decimal Strike { get; set; }

        public OptionTypes OptionType { get; set; }

        public virtual ICollection<Trade> CoveredCallTrades { get; set; }
        public virtual ICollection<Trade> BullPutSpreadShortTrades { get; set; }
        public virtual ICollection<Trade> BullPutSpreadLongTrades { get; set; }

        public Option()
        {
            this.TickSize = .01m;
            this.TickValue = 100m;
        }
    }
}
