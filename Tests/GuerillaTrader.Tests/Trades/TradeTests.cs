using GuerillaTrader.Entities.Dtos;
using GuerillaTrader.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using Abp.Domain.Repositories;
using GuerillaTrader.Entities;
using Abp.AutoMapper;

namespace GuerillaTrader.Tests.Trades
{
    public class TradeTests : GuerillaTraderTestBase
    {
        readonly ITradeAppService _TradeAppService;
        readonly IRepository<Trade> _tradeRepository;

        public TradeTests()
        {
            _TradeAppService = Resolve<ITradeAppService>();
            _tradeRepository = Resolve<IRepository<Trade>>();
        }

        [Fact(DisplayName = "Trades.SetAdjPL")]
        public void SetAdjPL_Test()
        {
            foreach (Trade trade in _tradeRepository.GetAll())
            {
                trade.AdjProfitLoss = trade.ProfitLoss - trade.Commissions;
                _TradeAppService.Save(trade.MapTo<TradeDto>());
            }
        }
    }
}
