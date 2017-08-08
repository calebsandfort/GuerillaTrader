using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace GuerillaTrader.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : GuerillaTraderControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}