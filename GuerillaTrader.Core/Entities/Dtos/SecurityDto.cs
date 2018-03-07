using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Entities.Dtos
{
    public abstract class SecurityDto : EntityDtoBase
    {
        public String Name { get; set; }
        public String Symbol { get; set; }
        public String CnbcSymbol { get; set; }

        [DataType(DataType.Currency)]
        public Decimal TickValue { get; set; }
        public Decimal TickSize { get; set; }

        [DataType(DataType.Currency)]
        public Decimal Price { get; set; }
    }
}
