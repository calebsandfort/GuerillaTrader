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
        readonly IObjectMapper _objectMapper;

        public MarketLogEntryDomainService(IRepository<MarketLogEntry> marketLogEntryRepository, IObjectMapper objectMapper)
        {
            this._marketLogEntryRepository = marketLogEntryRepository;
            this._objectMapper = objectMapper;
        }

        public void Add(MarketLogEntryDto dto)
        {
            if(dto.ScreenshotDbId.HasValue && dto.ScreenshotDbId == 0)
            {
                dto.ScreenshotDbId = null;
            }

            this._marketLogEntryRepository.Insert(dto.MapTo<MarketLogEntry>());
        }
    }
}
