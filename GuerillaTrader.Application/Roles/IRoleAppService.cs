using System.Threading.Tasks;
using Abp.Application.Services;
using GuerillaTrader.Roles.Dto;

namespace GuerillaTrader.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);
    }
}
