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
    public class MarketAppService : AppServiceBase, IMarketAppService
    {
        public readonly IRepository<Market> _repository;

        public MarketAppService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, IBackgroundJobManager backgroundJobManager, IObjectMapper objectMapper, IRepository<Market> repository)
            : base(sqlExecuter, consoleHubProxy, backgroundJobManager, objectMapper)
        {
            this._repository = repository;
        }

        public MarketDto Get(int id)
        {
            return _repository.Get(id).MapTo<MarketDto>();
        }

        public List<MarketDto> GetAll()
        {
            return _objectMapper.Map<List<MarketDto>>(_repository.GetAll().OrderBy(x => x.Symbol).ToList());
        }

        public List<MarketDto> GetAllActive()
        {
            return _objectMapper.Map<List<MarketDto>>(_repository.GetAll().Where(x => x.Active).OrderBy(x => x.Symbol).ToList());
        }

        public void Save(MarketDto dto)
        {
            if (dto.IsNew)
            {
                Market market = dto.MapTo<Market>();
                this._repository.Insert(market);
            }
            else
            {
                Market market = this._repository.Get(dto.Id);
                dto.MapTo(market);
            }
        }
    }
}
