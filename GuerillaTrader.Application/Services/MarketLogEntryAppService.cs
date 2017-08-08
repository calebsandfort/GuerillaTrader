using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Entities;
using GuerillaTrader.Entities.Dtos;
using GuerillaTrader.Shared.SqlExecuter;
using Abp.BackgroundJobs;
using GuerillaTrader.Shared;

namespace GuerillaTrader.Services
{
    public class MarketLogEntryAppService : AppServiceBase, IMarketLogEntryAppService
    {
        public readonly IRepository<MarketLogEntry> _repository;
        readonly IMarketLogEntryDomainService _marketLogEntryDomainService;

        public MarketLogEntryAppService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, IBackgroundJobManager backgroundJobManager, IObjectMapper objectMapper,
            IMarketLogEntryDomainService marketLogEntryDomainService, IRepository<MarketLogEntry> repository)
            : base(sqlExecuter, consoleHubProxy, backgroundJobManager, objectMapper)
        {
            this._repository = repository;
            this._marketLogEntryDomainService = marketLogEntryDomainService;
        }

        public void Add(MarketLogEntryDto dto)
        {
            this._marketLogEntryDomainService.Add(dto);
        }

        public void Purge()
        {
            foreach(MarketLogEntry entry in this._repository.GetAll().Where(x => x.TradingAccount.Active))
            {
                this._repository.Delete(entry.Id);
            }
        }
    }
}
