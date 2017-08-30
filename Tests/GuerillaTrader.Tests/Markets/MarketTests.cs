using GuerillaTrader.Entities.Dtos;
using GuerillaTrader.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace GuerillaTrader.Tests.Markets
{
    public class MarketTests : GuerillaTraderTestBase
    {
        readonly IMarketAppService _MarketAppService;

        public MarketTests()
        {
            _MarketAppService = Resolve<IMarketAppService>();
        }

        [Fact]
        public async Task LoadAndScrape_Test()
        {
            List<TosMarketDto> markets = await this._MarketAppService.LoadAndScrape();
            markets.Count(x => x.InitialMargin == 0m).ShouldBe(0, String.Format("Markets with no initial margin: {0}",
                markets.Count(x => x.InitialMargin == 0m) == 0 ? String.Empty : String.Join(",", markets.Where(x => x.InitialMargin == 0m).Select(x => x.Symbol))));
            markets.Count(x => x.TickValue == 0m).ShouldBe(0, String.Format("Markets with no tick value: {0}",
                markets.Count(x => x.TickValue == 0m) == 0 ? String.Empty : String.Join(",", markets.Where(x => x.TickValue == 0m).Select(x => x.Symbol))));
        }

        [Fact]
        public async Task GenerateSeedCode_Test()
        {
            await this._MarketAppService.GenerateSeedCode();
        }

        [Fact]
        public async Task UpdateTosProperties_Test()
        {
            await this._MarketAppService.UpdateTosProperties();
        }
    }
}
