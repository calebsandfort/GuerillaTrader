using GuerillaTrader.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace GuerillaTrader.Web.Models
{
    public class TradingAccountChartModel
    {
        public String Title { get; set; }
        public String Format { get; set; }
        public String Color { get; set; }
        public List<TradingAccountChartItem> Items { get; set; }

        public TradingAccountChartModel()
        {
            this.Items = new List<TradingAccountChartItem>();
        }
    }

    public class TradingAccountChartItem
    {
        public String Display { get; set; }
        public Decimal Value { get; set; }
    }
}