﻿using Abp.Application.Services;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Entities.Dtos;

namespace GuerillaTrader.Services
{
    public interface ITradeAppService : IApplicationService
    {
        bool Save(TradeDto dto);
        List<TradeDto> GetAll();
        void Purge();
        void AddTradeFromPaste(TradeFromPasteDto dto);
        void OpenCoveredStockPositions(TradeFromPasteDto dto);
        void UpdateCoveredStockPositions(TradeFromPasteDto dto);
        void OpenBullPutSpreadPositions(TradeFromPasteDto dto);
        void UpdateBullPutSpreadPositions(TradeFromPasteDto dto);
        void SaveOptionTrade(TradeDto dto, DateTime date);
    }
}
