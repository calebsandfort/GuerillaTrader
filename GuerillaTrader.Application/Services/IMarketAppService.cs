using Abp.Application.Services;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Entities.Dtos;

namespace GuerillaTrader.Services
{
    public interface IMarketAppService : IApplicationService
    {
        List<MarketDto> GetAll();
        List<MarketDto> GetAllActive();
        MarketDto Get(int id);
        void Save(MarketDto dto);
        void Save(TosMarketDto dto);
        Task GenerateSeedCode();
        void GenerateQtCode(String startDate, int maPeriod);
        Task UpdateTosProperties();
        void UpdateQtProperties();
        Task<List<TosMarketDto>> LoadAndScrape(bool scrape = true);
        Task Scrape(TosMarketDto dto);
    }
}
