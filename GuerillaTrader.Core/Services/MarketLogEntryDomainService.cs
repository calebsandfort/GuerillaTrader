using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.ObjectMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Entities;
using GuerillaTrader.Entities.Dtos;

namespace GuerillaTrader.Services
{
    public class MarketLogEntryDomainService : DomainService, IMarketLogEntryDomainService
    {
        public readonly IRepository<MarketLogEntry> _marketLogEntryRepository;
        public readonly IRepository<TradingDay> _tradingDayRepository;
        readonly IObjectMapper _objectMapper;

        public MarketLogEntryDomainService(IRepository<MarketLogEntry> marketLogEntryRepository, IRepository<TradingDay> tradingDayRepository, IObjectMapper objectMapper)
        {
            this._marketLogEntryRepository = marketLogEntryRepository;
            this._tradingDayRepository = tradingDayRepository;
            this._objectMapper = objectMapper;
        }

        public void Add(MarketLogEntryDto dto)
        {
            TradingDay tradingDay = _tradingDayRepository.FirstOrDefault(x => x.Day.Year == dto.TimeStamp.Year && x.Day.Month == dto.TimeStamp.Month && x.Day.Day == dto.TimeStamp.Day);
            if (tradingDay == null)
            {
                tradingDay = new TradingDay();
                tradingDay.Day = dto.TimeStamp.Date;
                this._tradingDayRepository.Insert(tradingDay);
            }
            dto.TradingDayId = tradingDay.Id;

            if(dto.ScreenshotDbId.HasValue && dto.ScreenshotDbId == 0)
            {
                dto.ScreenshotDbId = null;
            }

            this._marketLogEntryRepository.Insert(dto.MapTo<MarketLogEntry>());
        }
    }
}
