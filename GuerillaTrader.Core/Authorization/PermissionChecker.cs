using Abp.Authorization;
using GuerillaTrader.Authorization.Roles;
using GuerillaTrader.MultiTenancy;
using GuerillaTrader.Users;

namespace GuerillaTrader.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
