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
using System.Linq.Expressions;
using GuerillaTrader.Utilities;

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

        public TradingAccount Get(int id, params Expression<Func<TradingAccount, object>>[] propertySelectors)
        {
            return this._repository.GetAllIncluding(propertySelectors).Single(x => x.Id == id);
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

        [UnitOfWork(IsDisabled = true)]
        public void Reconcile(int tradingAccountId = 0, DateTime? snapshotDate = null)
        {
            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                this._sqlExecuter.Execute("DELETE FROM [PerformanceCycles] WHERE TradingAccountId = {0}", this._repository.GetAllIncluding(x => x.Trades).First(x => tradingAccountId == 0 ? x.Active : x.Id == tradingAccountId).Id);
                unitOfWork.Complete();
            }

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                TradingAccount tradingAccount = this._repository.GetAllIncluding(x => x.Trades).First(x => tradingAccountId == 0 ? x.Active : x.Id == tradingAccountId);
                List<Trade> closedTrades = this._tradeRepository.GetAll().Where(x => x.TradingAccountId == tradingAccount.Id).ToList();

                #region Old Guard
                tradingAccount.ProfitLoss = closedTrades.Sum(x => x.ProfitLoss);
                tradingAccount.Commissions = closedTrades.Sum(x => x.Commissions);
                tradingAccount.CurrentCapital = tradingAccount.InitialCapital + tradingAccount.ProfitLoss - tradingAccount.Commissions;
                tradingAccount.AdjProfitLoss = tradingAccount.ProfitLoss - tradingAccount.Commissions;

                tradingAccount.TotalReturn = (tradingAccount.CurrentCapital - tradingAccount.InitialCapital) / tradingAccount.InitialCapital;
                tradingAccount.CAGR = (Decimal)(Math.Pow((Double)(tradingAccount.CurrentCapital) / (Double)tradingAccount.InitialCapital, (1.0 / ((Double)(DateTime.Now - tradingAccount.InceptionDate).Days / 365.0))) - 1.0);
                #endregion

                #region PerformanceCycles
                tradingAccount.PerformanceCycles.AddRange(PerformanceCycle.BuildLists(tradingAccount));
                #endregion

                unitOfWork.Complete();
            }
        }

        public void BootstrapSnapshotsParent(int id = 0)
        {
            
        }

        public void BootstrapSnapshotsChild(TradingAccount tradingAccount)
        {
            
        }

        [UnitOfWork(IsDisabled = true)]
        public void Purge()
        {
            TradingAccount activeAccount;

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                activeAccount = this._repository.FirstOrDefault(x => x.Active);
                unitOfWork.Complete();
            }

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                this._sqlExecuter.Execute("DELETE FROM [Trades] WHERE TradingAccountId = {0}", activeAccount.Id);
                unitOfWork.Complete();
            }

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                this._sqlExecuter.Execute("DELETE FROM [PerformanceCycles] WHERE TradingAccountId = {0}", activeAccount.Id);
                unitOfWork.Complete();
            }

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                this.Reconcile();
                unitOfWork.Complete();
            }
        }
    }
}
