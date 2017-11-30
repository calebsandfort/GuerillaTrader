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
    [AutoMap(typeof(Option))]
    public class OptionDto : SecurityDto
    {
        [DataType(DataType.Date)]
        public DateTime Expiry { get; set; }

        [DataType(DataType.Currency)]
        public Decimal Strike { get; set; }

        public OptionTypes OptionType { get; set; }

        public OptionDto()
        {
            this.TickSize = .01m;
            this.TickValue = 100m;
        }
    }
}
