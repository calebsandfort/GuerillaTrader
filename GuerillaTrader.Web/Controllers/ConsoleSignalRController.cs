using System.Web.Mvc;
using GuerillaTrader.Web.Hubs;
using GuerillaTrader.Shared.Dtos;

namespace GuerillaTrader.Web.Controllers
{
    //[AbpMvcAuthorize]
    public class ConsoleSignalRController : GuerillaTraderControllerBase
    {
        private readonly ConsoleHub _consoleHub;

        public ConsoleSignalRController(ConsoleHub consoleHub)
        {
            _consoleHub = consoleHub;
        }

        [HttpPost]
        public ActionResult WriteLine(ConsoleWriteLineInput input)
        {
            this._consoleHub.WriteLine(input);
            return this.Json(new { success = true });
        }
    }
}