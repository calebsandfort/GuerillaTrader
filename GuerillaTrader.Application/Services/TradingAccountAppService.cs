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
    public class TradingAccountAppService : AppServiceBase, ITradingAccountAppService
    {
        public readonly IRepository<TradingAccount> _repository;
        public readonly IRepository<Trade> _tradeRepository;

        public TradingAccountAppService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, IBackgroundJobManager backgroundJobManager, IObjectMapper objectMapper,
            IRepository<TradingAccount> repository, IRepository<Trade> tradeRepository)
            : base(sqlExecuter, consoleHubProxy, backgroundJobManager, objectMapper)
        {
            this._repository = repository;
            this._tradeRepository = tradeRepository;
        }

        public void Save(TradingAccountDto dto)
        {
            if (dto.IsNew)
            {
                TradingAccount tradingAccount = dto.MapTo<TradingAccount>();
                this._repository.Insert(tradingAccount);
            }
            else
            {
                TradingAccount tradingAccount = this._repository.Get(dto.Id);
                dto.MapTo(tradingAccount);
            }

            if (dto.Active) this.SetActive(dto.Id);
        }

        public TradingAccountDto GetActive()
        {
            return this._repository.FirstOrDefault(x => x.Active).MapTo<TradingAccountDto>();
        }

        public void SetActive(int id)
        {
            foreach(TradingAccount tradingAccount in this._repository.GetAllList())
            {
                tradingAccount.Active = tradingAccount.Id == id;
            }
        }

        public List<TradingAccountDto> GetAll()
        {
            return _objectMapper.Map<List<TradingAccountDto>>(_repository.GetAll().OrderBy(x => x.Name).ToList());
        }

        public void Reconcile()
        {
            TradingAccount tradingAccount = this._repository.GetAllIncluding(x => x.Trades).First(x => x.Active);
            List<Trade> closedTrades = this._tradeRepository.GetAll().Where(x => x.TradingAccountId == tradingAccount.Id && x.ExitReason != TradeExitReasons.None).ToList();

            tradingAccount.ProfitLoss = closedTrades.Sum(x => x.ProfitLoss);
            tradingAccount.CurrentCapital = tradingAccount.InitialCapital + tradingAccount.ProfitLoss;
            tradingAccount.Commissions = closedTrades.Sum(x => x.Commissions);
        }

        public void Purge()
        {
            foreach (Trade trade in this._tradeRepository.GetAll().Where(x => x.TradingAccount.Active))
            {
                this._tradeRepository.Delete(trade.Id);
            }

            this.Reconcile();
        }
    }
}
