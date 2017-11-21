using Abp.AutoMapper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GuerillaTrader.Framework;

namespace GuerillaTrader.Entities.Dtos
{
    [AutoMap(typeof(Stock))]
    public class StockUpdatePriceAndDatesDto : EntityDtoBase
    {
        [DataType(DataType.Currency)]
        public Decimal Price { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NextEarningsDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExDividendDate { get; set; }

        [DataType(DataType.Currency)]
        public Decimal? TargetPrice { get; set; }

        public int? AvgVolume { get; set; }
        public Decimal? ADV { get; set; }
    }
}
