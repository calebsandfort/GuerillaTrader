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
    public interface IStockAppService : IApplicationService
    {
        List<StockDto> GetAll();
        StockDto Get(int id);
        void Save(StockDto dto);
        void Save(PfStockDto dto);
        void Save(SectorDto dto);
        void UpdatePfProperties();
        void DeleteReports();
        Task GenerateReports(GenerateStockReportsInput input);
        void UpdateTaxProperties();
        void UpdateSectorProperties();
        Task UpdatePriceAndDates();
        void Save(StockUpdatePriceAndDatesDto dto);
    }
}
