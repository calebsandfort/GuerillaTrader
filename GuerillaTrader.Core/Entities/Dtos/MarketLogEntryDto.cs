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
    public class MarketLogEntryDto : EntityDtoBase
    {
        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; }
        public String Text { get; set; }

        public String Screenshot { get; set; }
        public bool ShowScreenshot
        {
            get
            {
                return this.ScreenshotDbId > 0;
            }
        }

        public MarketLogEntryTypes MarketLogEntryType { get; set; }
        public String MarketLogEntryTypeDisplay { get { return this.MarketLogEntryType.GetDisplay(); } }
        public virtual int TradingDayId { get; set; }

        public virtual int TradingAccountId { get; set; }

        public String Market { get; set; }
        public virtual int? MarketId { get; set; }

        public String Stock { get; set; }
        public virtual int? StockId { get; set; }

        public virtual int? ScreenshotDbId { get; set; }

    }
}
