using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using GuerillaTrader.Framework;
using Abp.Application.Services.Dto;

namespace GuerillaTrader.Entities.Dtos
{
    [AutoMap(typeof(Screenshot))]
    public class ScreenshotDto : EntityDtoBase
    {
        public byte[] Data { get; set; }
        public Decimal EntryPrice { get; set; }
        public Decimal StopLossPrice { get; set; }
        public Decimal ProfitTakerPrice { get; set; }
    }
}
