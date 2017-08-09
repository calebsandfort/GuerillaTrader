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
using Tesseract;
using System.IO;
using GuerillaTrader.Web.Models;
using Abp.AutoMapper;

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
        public ActionResult SaveBase64(String base64, TradeTypes tradeType)
        {
            ScreenshotDto dto = this._screenshotAppService.SaveBase64(base64);
            RecognizeText(dto.Id, tradeType).MapTo(dto);
            return Json(dto);
        }

        [OutputCache(VaryByParam = "id", Duration = 360000000)]
        public ActionResult Screenshot(int id)
        {
            ScreenshotDto dto = this._screenshotAppService.Get(id);

            if (dto.Data.Length > 0)
            {
                return File(dto.Data, "image/png");
            }
            return new EmptyResult();
        }

        private ExtractedPricesModel RecognizeText(int id, TradeTypes tradeType)
        {
            Screenshot screenshot = this._screenshotRepository.Get(id);
            ExtractedPricesModel model = null;

            using (var engine = new TesseractEngine(Server.MapPath(@"~/tessdata"), "eng", EngineMode.Default))
            {
                using (var image = new System.Drawing.Bitmap(new MemoryStream(screenshot.Data)))
                {
                    using (var pix = PixConverter.ToPix(image))
                    {
                        using (var page = engine.Process(pix))
                        {
                            var text = page.GetText();
                            String[] priceStrings = text.Replace("\n", ":").Replace(": ", ":").Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            model = new ExtractedPricesModel(priceStrings, tradeType);
                        }
                    }
                }
            }

            return model;
        }
    }
}