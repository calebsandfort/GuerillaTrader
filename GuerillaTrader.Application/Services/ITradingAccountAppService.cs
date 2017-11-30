using Abp.Application.Services;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Entities.Dtos;
using GuerillaTrader.Entities;
using System.Linq.Expressions;

namespace GuerillaTrader.Services
{
    public interface ITradingAccountAppService : IApplicationService
    {
        TradingAccount Get(int id, params Expression<Func<TradingAccount, object>>[] propertySelectors);
        void Save(TradingAccountDto dto);
        TradingAccountDto GetActive();
        void SetActive(int id);
        List<TradingAccountDto> GetAll();
        void Reconcile(int tradingAccountId = 0, DateTime? snapshotDate = null);
        void Purge();
        void BootstrapSnapshotsParent(int id = 0);
        void BootstrapSnapshotsChild(TradingAccount tradingAccount);
    }
}
