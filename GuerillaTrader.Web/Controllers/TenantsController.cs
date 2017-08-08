using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using GuerillaTrader.Authorization;
using GuerillaTrader.MultiTenancy;

namespace GuerillaTrader.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantsController : GuerillaTraderControllerBase
    {
        private readonly ITenantAppService _tenantAppService;

        public TenantsController(ITenantAppService tenantAppService)
        {
            _tenantAppService = tenantAppService;
        }

        public ActionResult Index()
        {
            var output = _tenantAppService.GetTenants();
            return View(output);
        }
    }
}