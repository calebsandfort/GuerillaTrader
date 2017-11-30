using GuerillaTrader.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuerillaTrader.Web.Models
{
    public class TradingAccountDetailsModel
    {
        public TradingAccountDto TradingAccount { get; set; }
        public List<TradingAccountSnapshotDto> Snapshots { get; set; }
    }
}