using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GuerillaTrader.MultiTenancy.Dto;

namespace GuerillaTrader.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        ListResultDto<TenantListDto> GetTenants();

        Task CreateTenant(CreateTenantInput input);
    }
}
