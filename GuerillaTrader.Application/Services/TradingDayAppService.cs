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
    public class TradingDayAppService : AppServiceBase, ITradingDayAppService
    {
        public readonly IRepository<TradingDay> _repository;

        public TradingDayAppService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, IBackgroundJobManager backgroundJobManager, IObjectMapper objectMapper,
            IRepository<TradingDay> repository)
            : base(sqlExecuter, consoleHubProxy, backgroundJobManager, objectMapper)
        {
            this._repository = repository;
        }

        public TradingDayDto Get(DateTime date)
        {
            TradingDay tradingDay = _repository.FirstOrDefault(x => x.Day.Year == date.Year && x.Day.Month == date.Month && x.Day.Day == date.Day);
            if (tradingDay == null)
            {
                tradingDay = new TradingDay();
                tradingDay.Day = date.Date;
                this._repository.Insert(tradingDay);
            }

            return tradingDay.MapTo<TradingDayDto>();
        }
    }
}
