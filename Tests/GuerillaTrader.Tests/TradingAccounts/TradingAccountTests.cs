using GuerillaTrader.Entities.Dtos;
using GuerillaTrader.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using GuerillaTrader.Entities;
using Abp.Domain.Repositories;

namespace GuerillaTrader.Tests.TradingAccounts
{
    public class TradingAccountTests : GuerillaTraderTestBase
    {
        readonly ITradingAccountAppService _TradingAccountAppService;

        public TradingAccountTests()
        {
            _TradingAccountAppService = Resolve<TradingAccountAppService>();
        }

        [Fact(DisplayName = "TradingAccounts.Reconcile")]
        public void Reconcile_Test()
        {
            foreach (TradingAccountDto dto in _TradingAccountAppService.GetAll())
            {
                this._TradingAccountAppService.Reconcile(dto.Id);
            }
        }

        [Fact(DisplayName = "TradingAccounts.BootstrapSnapshotsParent")]
        public void BootstrapSnapshotsParent_Test()
        {
            this._TradingAccountAppService.BootstrapSnapshotsParent(4);
        }

        [Fact(DisplayName = "TradingAccounts.BootstrapSnapshotsChild")]
        public void BootstrapSnapshotsChild_Test()
        {
            
        }
    }
}
