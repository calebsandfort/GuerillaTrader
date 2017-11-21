using GuerillaTrader.Entities.Dtos;
using GuerillaTrader.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace GuerillaTrader.Tests.Stocks
{
    public class StockTests : GuerillaTraderTestBase
    {
        readonly IStockAppService _StockAppService;

        public StockTests()
        {
            _StockAppService = Resolve<IStockAppService>();
        }

        [Fact]
        public void UpdatePfProperties_Test()
        {
            this._StockAppService.UpdatePfProperties();
        }

        [Fact(DisplayName = "Stocks.GenerateReports")]
        public void GenerateReports_Test()
        {
            this._StockAppService.DeleteReports();
            this._StockAppService.GenerateReports(new GenerateStockReportsInput { StartDate = new DateTime(2017, 11, 13), EndDate = new DateTime(2017, 12, 13), Lookback = 5 });
        }

        [Fact(DisplayName = "Stocks.UpdateTaxProperties")]
        public void UpdateTaxProperties_Test()
        {
            this._StockAppService.UpdateTaxProperties();
        }
    }
}
