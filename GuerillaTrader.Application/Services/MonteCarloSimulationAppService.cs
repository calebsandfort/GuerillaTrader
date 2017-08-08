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
using GuerillaTrader.BackgroundJobs;
using GuerillaTrader.Shared.Dtos;

namespace GuerillaTrader.Services
{
    public class MonteCarloSimulationAppService : AppServiceBase, IMonteCarloSimulationAppService
    {
        public readonly IRepository<MonteCarloSimulation> _repository;
        public readonly IRepository<Trade> _tradeRepository;
        public readonly IRepository<Market> _marketRepository;

        public MonteCarloSimulationAppService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, IBackgroundJobManager backgroundJobManager, IObjectMapper objectMapper,
            IRepository<MonteCarloSimulation> repository, IRepository<Trade> tradeRepository, IRepository<Market> marketRepository)
            : base(sqlExecuter, consoleHubProxy, backgroundJobManager, objectMapper)
        {
            this._repository = repository;
            this._tradeRepository = tradeRepository;
            this._marketRepository = marketRepository;
        }

        public void Save(MonteCarloSimulationDto dto)
        {
            if (dto.IsNew)
            {
                MonteCarloSimulation monteCarloSimulation = dto.MapTo<MonteCarloSimulation>();
                int id = this._repository.InsertAndGetId(monteCarloSimulation);
                this.RunSimulationEnqueue(new MonteCarloSimulationDto() { Id = id });
            }
            else
            {
                MonteCarloSimulation monteCarloSimulation = this._repository.Get(dto.Id);
                dto.MapTo(monteCarloSimulation);
            }
        }

        public void RunSimulationEnqueue(MonteCarloSimulationDto dto)
        {
            _backgroundJobManager.Enqueue<RunMonteCarloSimulationBackgroundJob, MonteCarloSimulationDto>(dto);
        }

        public void RunSimulation(MonteCarloSimulationDto dto)
        {
            MonteCarloSimulation sim = _repository.Get(dto.Id);
            sim.MapTo(dto);
            List<Trade> sample = this._tradeRepository.GetAll().Where(x => x.TradingAccountId == dto.TradingAccountId && x.ExitReason != TradeExitReasons.None).ToList();
            List<Market> markets = this._marketRepository.GetAll().Where(x => x.Active).ToList();
            dto.Simulate(sample, markets, this._consoleHubProxy);
            dto.MapTo(sim);
        }

        public List<MonteCarloSimulationDto> GetAll()
        {
            return _objectMapper.Map<List<MonteCarloSimulationDto>>(_repository.GetAll().OrderByDescending(x => x.TimeStamp).ToList());
        }

        public void Purge()
        {
            foreach (Trade trade in this._tradeRepository.GetAll().Where(x => x.TradingAccount.Active))
            {
                this._tradeRepository.Delete(trade.Id);
            }
        }
    }
}
