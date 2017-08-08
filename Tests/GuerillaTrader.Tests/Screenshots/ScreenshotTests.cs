using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using GuerillaTrader.Services;

namespace GuerillaTrader.Tests.Screenshots
{
    public class ScreenshotTests : GuerillaTraderTestBase
    {
        readonly IScreenshotAppService _screenshotAppService;

        public ScreenshotTests()
        {
            _screenshotAppService = Resolve<IScreenshotAppService>();
        }

        [Fact]
        public async Task NflTeamCount_Test()
        {
            this._screenshotAppService.RecognizeText(64);
        }
    }
}
