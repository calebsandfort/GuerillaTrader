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
using GuerillaTrader.Framework;
using System.Linq.Expressions;

namespace GuerillaTrader.Services
{
    public class TradingAccountAppService : AppServiceBase, ITradingAccountAppService
    {
        public readonly IRepository<TradingAccount> _repository;
        public readonly IRepository<Trade> _tradeRepository;
        public readonly IRepository<TradingAccountSnapshot> _tradingAccountSnapshotRepository;

        public TradingAccountAppService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, IBackgroundJobManager backgroundJobManager, IObjectMapper objectMapper,
            IRepository<TradingAccount> repository, IRepository<Trade> tradeRepository, IRepository<TradingAccountSnapshot> tradingAccountSnapshotRepository)
            : base(sqlExecuter, consoleHubProxy, backgroundJobManager, objectMapper)
        {
            this._repository = repository;
            this._tradeRepository = tradeRepository;
            this._tradingAccountSnapshotRepository = tradingAccountSnapshotRepository;
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

        public void Reconcile(int tradingAccountId = 0, DateTime? snapshotDate = null)
        {
            TradingAccount tradingAccount = this._repository.GetAllIncluding(x => x.Trades).First(x => tradingAccountId == 0 ? x.Active : x.Id == tradingAccountId);
            List<Trade> closedTrades = this._tradeRepository.GetAll().Where(x => x.TradingAccountId == tradingAccount.Id).ToList();

            tradingAccount.ProfitLoss = closedTrades.Sum(x => x.ProfitLoss);
            tradingAccount.CurrentCapital = tradingAccount.InitialCapital + tradingAccount.ProfitLoss;
            tradingAccount.Commissions = closedTrades.Sum(x => x.Commissions);

            tradingAccount.TotalReturn = (tradingAccount.CurrentCapital - tradingAccount.Commissions - tradingAccount.InitialCapital) / tradingAccount.InitialCapital;
            tradingAccount.CAGR = (Decimal)(Math.Pow((Double)(tradingAccount.CurrentCapital - tradingAccount.Commissions) / (Double)tradingAccount.InitialCapital, (1.0 / ((Double)(DateTime.Now - tradingAccount.InceptionDate).Days / 365.0))) - 1.0);

            Func<TradingAccountSnapshot, DateTime, bool> dateFunc = (x, date) => x.Date.Year == date.Year && x.Date.Month == date.Month && x.Date.Day == date.Day;
            DateTime comparisonDate = snapshotDate.HasValue ? snapshotDate.Value : DateTime.Now;
            TradingAccountSnapshot snapshot;

            if (this._tradingAccountSnapshotRepository.GetAll().Any(x => x.TradingAccountId == tradingAccount.Id && x.Date.Year == comparisonDate.Year && x.Date.Month == comparisonDate.Month && x.Date.Day == comparisonDate.Day))
            {
                snapshot = this._tradingAccountSnapshotRepository.GetAll().Single(x => x.TradingAccountId == tradingAccount.Id && x.Date.Year == comparisonDate.Year && x.Date.Month == comparisonDate.Month && x.Date.Day == comparisonDate.Day);               
            }
            else
            {
                snapshot = new TradingAccountSnapshot { Date = comparisonDate };
            }

            snapshot.ProfitLoss = tradingAccount.ProfitLoss;
            snapshot.CurrentCapital = tradingAccount.CurrentCapital;
            snapshot.Commissions = tradingAccount.Commissions;
            snapshot.TotalReturn = tradingAccount.TotalReturn;
            snapshot.CAGR = tradingAccount.CAGR;

            if (snapshot.IsNew) tradingAccount.Snapshots.Add(snapshot);
        }

        public void BootstrapSnapshotsParent(int id = 0)
        {
            foreach (TradingAccount tradingAccount in this._repository.GetAllIncluding(x => x.Snapshots, x => x.Trades).Where(x => id > 0 ? x.Id == id : x.Id > 0))
            {
                BootstrapSnapshotsChild(tradingAccount);
            }
        }

        public void BootstrapSnapshotsChild(TradingAccount tradingAccount)
        {
            TradingAccountSnapshot rollingSnapshot = new TradingAccountSnapshot
            {
                CurrentCapital = tradingAccount.InitialCapital,
                ProfitLoss = 0m,
                Commissions = 0m,
                TotalReturn = 0m,
                CAGR = 0m
            };

            TradingAccountSnapshot initialSnapshot = new TradingAccountSnapshot(tradingAccount.InceptionDate, rollingSnapshot);

            tradingAccount.Snapshots.Add(initialSnapshot);

            var groupings = from t in tradingAccount.Trades
                            group t by t.EntryDate.Date into g
                            select new { Date = g.Key, Trades = g.ToList() };

            foreach(var grouping in groupings.OrderBy(x => x.Date))
            {
                rollingSnapshot.ProfitLoss += grouping.Trades.Sum(x => x.ProfitLoss);
                rollingSnapshot.CurrentCapital += tradingAccount.ProfitLoss;
                rollingSnapshot.Commissions += grouping.Trades.Sum(x => x.Commissions);

                rollingSnapshot.TotalReturn = (rollingSnapshot.CurrentCapital - tradingAccount.InitialCapital) / tradingAccount.InitialCapital;
                rollingSnapshot.CAGR = (Decimal)(Math.Pow((Double)rollingSnapshot.CurrentCapital / (Double)tradingAccount.InitialCapital, (1.0 / ((Double)(grouping.Date - tradingAccount.InceptionDate).Days / 365.0))) - 1.0);

                tradingAccount.Snapshots.Add(new TradingAccountSnapshot(grouping.Date, rollingSnapshot));
            }
        }

        public void Purge()
        {
            foreach (Trade trade in this._tradeRepository.GetAll().Where(x => x.TradingAccount.Active))
            {
                this._tradeRepository.Delete(trade.Id);
            }

            foreach (TradingAccountSnapshot snapshot in this._tradingAccountSnapshotRepository.GetAll().Where(x => x.TradingAccount.Active))
            {
                this._tradingAccountSnapshotRepository.Delete(snapshot.Id);
            }

            this.Reconcile();
        }
    }
}
