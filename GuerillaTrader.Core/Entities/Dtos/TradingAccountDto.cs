using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Entities.Dtos
{
    [AutoMap(typeof(TradingAccount))]
    public class TradingAccountDto : EntityDtoBase
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

        [DataType(DataType.Currency)]
        public Decimal AdjProfitLoss { get; set; }

        public Decimal TotalReturn { get; set; }

        public Decimal CAGR { get; set; }

        [DataType(DataType.Date)]
        [UIHint("MyDate")]
        public DateTime InceptionDate { get; set; }

        public bool Active { get; set; }
    }
}
