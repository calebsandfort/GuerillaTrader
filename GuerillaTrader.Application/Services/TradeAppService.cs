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
using GuerillaTrader.Services;
using GuerillaTrader.Entities;
using GuerillaTrader.Entities.Dtos;
using GuerillaTrader.Shared.SqlExecuter;
using GuerillaTrader.Framework;
using Abp.Timing;
using Abp.BackgroundJobs;
using GuerillaTrader.Shared;

namespace GuerillaTrader.Services
{
    public class TradeAppService : AppServiceBase, ITradeAppService
    {
        public readonly IRepository<Trade> _repository;
        public readonly IRepository<Market> _marketRepository;
        public readonly ITradingAccountAppService _tradingAccountAppService;
        public readonly ITradingDayAppService _tradingDayAppService;
        readonly IRepository<MarketLogEntry> _marketLogEntryRepository;

        public TradeAppService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, IBackgroundJobManager backgroundJobManager, IObjectMapper objectMapper, IRepository<Trade> repository,
            IRepository<Market> marketRepository, ITradingAccountAppService tradingAccountAppService,
            ITradingDayAppService tradingDayAppService, IRepository<MarketLogEntry> marketLogEntryRepository)
            : base(sqlExecuter, consoleHubProxy, backgroundJobManager, objectMapper)
        {
            this._repository = repository;
            this._marketRepository = marketRepository;
            this._tradingAccountAppService = tradingAccountAppService;
            this._tradingDayAppService = tradingDayAppService;
            this._marketLogEntryRepository = marketLogEntryRepository;
        }

        public bool Save(TradeDto dto)
        {
            bool reconcileTradingAccount = false;
            Trade trade = new Trade();

            if (dto.IsNew)
            {
                trade = dto.MapTo<Trade>();
                trade.TradingAccountId = this._tradingAccountAppService.GetActive().Id;
                trade.TradingDayId = this._tradingDayAppService.Get(trade.EntryDate).Id;

                MarketLogEntry tradeEnterLogEntry = new MarketLogEntry();
                tradeEnterLogEntry.MarketId = trade.MarketId;
                tradeEnterLogEntry.MarketLogEntryType = MarketLogEntryTypes.TradeEnter;
                if (trade.EntryScreenshotDbId.HasValue) tradeEnterLogEntry.ScreenshotDbId = trade.EntryScreenshotDbId;
                tradeEnterLogEntry.Text = String.Format("{0} {1} @ {2:C}<br/>{3}", trade.TradeType == TradeTypes.Long ? "Buy" : "Sell", trade.Size, trade.EntryPrice, trade.EntryRemarks);
                tradeEnterLogEntry.TimeStamp = trade.EntryDate;
                tradeEnterLogEntry.TradingAccountId = trade.TradingAccountId;
                tradeEnterLogEntry.TradingDayId = trade.TradingDayId;

                this._marketLogEntryRepository.Insert(tradeEnterLogEntry);

                if (trade.ExitReason != TradeExitReasons.None)
                {
                    trade.Market = this._marketRepository.Get(trade.MarketId);
                    trade.Reconcile();
                    reconcileTradingAccount = true;
                }
            }
            else
            {
                trade = this._repository.Get(dto.Id);
                bool exitReasonChanged = dto.ExitReason != trade.ExitReason && dto.ExitReason != TradeExitReasons.None;
                dto.MapTo(trade);

                if (exitReasonChanged)
                {
                    trade.Market = this._marketRepository.Get(trade.MarketId);
                    trade.Reconcile();
                    reconcileTradingAccount = true;
                }
            }

            if (reconcileTradingAccount)
            {
                MarketLogEntry tradeExitLogEntry = new MarketLogEntry();
                tradeExitLogEntry.MarketId = trade.MarketId;
                tradeExitLogEntry.MarketLogEntryType = MarketLogEntryTypes.TradeExit;
                if (trade.ExitScreenshotDbId.HasValue) tradeExitLogEntry.ScreenshotDbId = trade.ExitScreenshotDbId;
                tradeExitLogEntry.Text = String.Format("{0}: {1} {2} @ {3:C}, P/L: {4:C}<br/>{5}", trade.ExitReason.GetDisplay(), trade.TradeType == TradeTypes.Long ? "Sell" : "Buy", trade.Size, trade.ExitPrice, trade.ProfitLoss, trade.ExitRemarks);
                tradeExitLogEntry.TimeStamp = trade.ExitDate.Value;
                tradeExitLogEntry.TradingAccountId = trade.TradingAccountId;
                tradeExitLogEntry.TradingDayId = trade.TradingDayId;
                this._marketLogEntryRepository.Insert(tradeExitLogEntry);
            }

            if (dto.IsNew)
            {
                this._repository.Insert(trade);
            }

            return reconcileTradingAccount;
        }

        public List<TradeDto> GetAll()
        {
            return _objectMapper.Map<List<TradeDto>>(_repository.GetAll().OrderByDescending(x => x.EntryDate).ToList());
        }

        public void Purge()
        {
            foreach (Trade trade in this._repository.GetAll().Where(x => x.TradingAccount.Active))
            {
                this._repository.Delete(trade.Id);
            }
        }
    }
}
