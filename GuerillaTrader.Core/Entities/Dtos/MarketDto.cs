using Abp.AutoMapper;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Entities.Dtos;

namespace GuerillaTrader.Entities.Dtos
{
    [AutoMap(typeof(Market))]
    public class MarketDto : EntityDtoBase
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
        public Decimal PointValue
        {
            get
            {
                return (this.TickSize * this.TickValue) == 0 ? 0m : 1.0m / this.TickSize * this.TickValue;
            }
        }
    }
}
