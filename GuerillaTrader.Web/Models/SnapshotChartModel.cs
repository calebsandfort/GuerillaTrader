using GuerillaTrader.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace GuerillaTrader.Web.Models
{
    public class SnapshotChartModel
    {
        public String Title { get; set; }
        public String ValueField { get; set; }
        public Decimal ValueComp { get; set; }
        public String Format { get; set; }
        public String CategoryField { get; set; }
        public List<TradingAccountSnapshotDto> Snapshots { get; set; }

        public SnapshotChartModel()
        {
            this.Snapshots = new List<TradingAccountSnapshotDto>();
        }
    }
}