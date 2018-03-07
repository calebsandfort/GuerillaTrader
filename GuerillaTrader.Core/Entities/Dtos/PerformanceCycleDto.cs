using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Entities.Dtos
{
    [AutoMap(typeof(PerformanceCycle))]
    public class PerformanceCycleDto : EntityDtoBase
    {
        public int Position { get; set; }
        public PerformanceCycleTypes CycleType { get; set; }
        public int Display { get; set; }
        public Decimal R { get; set; }
        public Decimal MaxDrawdown { get; set; }
        public int TradingAccountId { get; set; }
    }
}
