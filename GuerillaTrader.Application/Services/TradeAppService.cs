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
using GuerillaTrader.Shared.Dtos;

namespace GuerillaTrader.Services
{
    public class TradeAppService : AppServiceBase, ITradeAppService
    {
        public readonly IRepository<Trade> _repository;
        public readonly IRepository<Market> _marketRepository;
        public readonly IRepository<Stock> _stockRepository;
        public readonly ITradingAccountAppService _tradingAccountAppService;
        readonly IRepository<MarketLogEntry> _marketLogEntryRepository;

        public TradeAppService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, IBackgroundJobManager backgroundJobManager, IObjectMapper objectMapper, IRepository<Trade> repository,
            IRepository<Market> marketRepository, IRepository<Stock> stockRepository, ITradingAccountAppService tradingAccountAppService,
            IRepository<MarketLogEntry> marketLogEntryRepository)
            : base(sqlExecuter, consoleHubProxy, backgroundJobManager, objectMapper)
        {
            this._repository = repository;
            this._marketRepository = marketRepository;
            this._stockRepository = stockRepository;
            this._tradingAccountAppService = tradingAccountAppService;
            this._marketLogEntryRepository = marketLogEntryRepository;
        }

        [UnitOfWork(IsDisabled = true)]
        public void AddTradeFromPaste(TradeFromPasteDto dto)
        {
            try
            {
                foreach (TradeDto trade in dto.ToFutureTradeDto(_marketRepository.GetAllList()))
                {
                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        Save(trade);
                        unitOfWork.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
            }
        }

        [UnitOfWork(IsDisabled = true)]
        public void OpenCoveredStockPositions(TradeFromPasteDto dto)
        {
            try
            {
                foreach (TradeDto trade in dto.ToOpenCoveredCalls(_stockRepository.GetAllList()))
                {
                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        SaveOptionTrade(trade, dto.Date);
                        unitOfWork.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
            }
        }

        [UnitOfWork(IsDisabled = true)]
        public void UpdateCoveredStockPositions(TradeFromPasteDto dto)
        {
            try
            {
                List<TradeDto> trades;

                using (var unitOfWork = this.UnitOfWorkManager.Begin())
                {
                    trades = _objectMapper.Map<List<TradeDto>>(_repository.GetAllIncluding(x => x.CoveredCallOption, x => x.Stock).Where(x => x.TradingAccountId == dto.TradingAccountId
                    && x.ExitReason == TradeExitReasons.None && x.CoveredCallOptionId.HasValue).ToList());
                    unitOfWork.Complete();
                }

                dto.ToUpdateCoveredCalls(trades);

                foreach (TradeDto trade in trades)
                {
                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        SaveOptionTrade(trade, dto.Date);
                        unitOfWork.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
            }
        }

        [UnitOfWork(IsDisabled = true)]
        public void OpenBullPutSpreadPositions(TradeFromPasteDto dto)
        {
            try
            {
                foreach (TradeDto trade in dto.ToOpenBullPutSpreads(_stockRepository.GetAllList()))
                {
                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        SaveOptionTrade(trade, dto.Date);
                        unitOfWork.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
            }
        }

        [UnitOfWork(IsDisabled = true)]
        public void UpdateBullPutSpreadPositions(TradeFromPasteDto dto)
        {
            try
            {
                List<TradeDto> trades;

                using (var unitOfWork = this.UnitOfWorkManager.Begin())
                {
                    trades = _objectMapper.Map<List<TradeDto>>(_repository.GetAllIncluding(x => x.BullPutSpreadLongOption, x=> x.BullPutSpreadShortOption, x => x.Stock).Where(x => x.TradingAccountId == dto.TradingAccountId
                    && x.ExitReason == TradeExitReasons.None && x.BullPutSpreadLongOptionId.HasValue && x.BullPutSpreadShortOptionId.HasValue).ToList());
                    unitOfWork.Complete();
                }

                dto.ToUpdateBullPutSpreads(trades);

                foreach (TradeDto trade in trades)
                {
                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        SaveOptionTrade(trade, dto.Date);
                        unitOfWork.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
            }
        }

        public void SaveOptionTrade(TradeDto dto, DateTime date)
        {
            Trade trade = new Trade();

            if (dto.IsNew)
            {
                trade = dto.MapTo<Trade>();
            }
            else
            {
                trade = this._repository.Get(dto.Id);
                bool exitReasonChanged = dto.ExitReason != trade.ExitReason && dto.ExitReason != TradeExitReasons.None;
                dto.MapTo(trade);

                if (exitReasonChanged)
                {
                    trade.ExitDate = date;
                }
            }

            trade.Reconcile();

            if (dto.IsNew)
            {
                this._repository.Insert(trade);
            }
        }

        public bool Save(TradeDto dto)
        {
            bool reconcileTradingAccount = false;
            Trade trade = new Trade();

            if (dto.IsNew)
            {
                trade = dto.MapTo<Trade>();

                //MarketLogEntry tradeEnterLogEntry = new MarketLogEntry();
                //tradeEnterLogEntry.MarketId = trade.MarketId;
                //tradeEnterLogEntry.MarketLogEntryType = MarketLogEntryTypes.TradeEnter;
                //if (trade.EntryScreenshotDbId.HasValue) tradeEnterLogEntry.ScreenshotDbId = trade.EntryScreenshotDbId;
                //tradeEnterLogEntry.Text = String.Format("{0} {1} @ {2:C5}<br/>{3}", trade.TradeType == TradeTypes.LongFuture ? "Buy" : "Sell", trade.Size, trade.EntryPrice, trade.EntryRemarks);
                //tradeEnterLogEntry.TimeStamp = trade.EntryDate;
                //tradeEnterLogEntry.TradingAccountId = trade.TradingAccountId;
                //tradeEnterLogEntry.TradingDayId = trade.TradingDayId;

                //this._marketLogEntryRepository.Insert(tradeEnterLogEntry);

                if (trade.ExitReason != TradeExitReasons.None)
                {
                    if(trade.MarketId.HasValue) trade.Market = this._marketRepository.Get(trade.MarketId.Value);
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
                    if (trade.MarketId.HasValue) trade.Market = this._marketRepository.Get(trade.MarketId.Value);
                    trade.Reconcile();
                    reconcileTradingAccount = true;
                }
            }

            if (reconcileTradingAccount)
            {
                //MarketLogEntry tradeExitLogEntry = new MarketLogEntry();
                //tradeExitLogEntry.MarketId = trade.MarketId;
                //tradeExitLogEntry.MarketLogEntryType = MarketLogEntryTypes.TradeExit;
                //if (trade.ExitScreenshotDbId.HasValue) tradeExitLogEntry.ScreenshotDbId = trade.ExitScreenshotDbId;
                //tradeExitLogEntry.Text = String.Format("{0}: {1} {2} @ {3:C5}, P/L: {4:C5}<br/>{5}", trade.ExitReason.GetDisplay(), trade.TradeType == TradeTypes.LongFuture ? "Sell" : "Buy", trade.Size, trade.ExitPrice, trade.ProfitLoss, trade.ExitRemarks);
                //tradeExitLogEntry.TimeStamp = trade.ExitDate.Value;
                //tradeExitLogEntry.TradingAccountId = trade.TradingAccountId;
                //tradeExitLogEntry.TradingDayId = trade.TradingDayId;
                //this._marketLogEntryRepository.Insert(tradeExitLogEntry);
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
