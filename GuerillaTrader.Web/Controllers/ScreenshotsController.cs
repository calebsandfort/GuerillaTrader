using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuerillaTrader.Entities;
using GuerillaTrader.Entities.Dtos;
using GuerillaTrader.Services;

namespace GuerillaTrader.Web.Controllers
{
    public class ScreenshotsController : GuerillaTraderControllerBase
    {
        readonly IRepository<Screenshot> _screenshotRepository;
        readonly IScreenshotAppService _screenshotAppService;
        readonly IObjectMapper _objectMapper;

        public ScreenshotsController(IRepository<Screenshot> screenshotRepository, IScreenshotAppService screenshotAppService, IObjectMapper objectMapper)
        {
            _screenshotRepository = screenshotRepository;
            _screenshotAppService = screenshotAppService;
            _objectMapper = objectMapper;
        }

        // GET: Screenshots
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveBase64(String base64)
        {
            return Json(this._screenshotAppService.SaveBase64(base64));
        }

        [OutputCache(VaryByParam = "id", Duration = 3600)]
        public ActionResult Screenshot(int id)
        {
            ScreenshotDto dto = this._screenshotAppService.Get(id);

            if (dto.Data.Length > 0)
            {
                return File(dto.Data, "image/png");
            }
            return new EmptyResult();
        }
    }
}