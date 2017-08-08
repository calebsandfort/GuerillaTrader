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
    [Table("TradingDirectives")]
    public class TradingDirective : Entity<int>
    {
        public String Text { get; set; }
        public TradingDirectiveTypes TradingDirectiveType { get; set; }
    }
}
