using Abp.Application.Services;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Entities;
using GuerillaTrader.Entities.Dtos;

namespace GuerillaTrader.Services
{
    public interface ITradingDayAppService : IApplicationService
    {
        TradingDayDto Get(DateTime date);
    }
}
