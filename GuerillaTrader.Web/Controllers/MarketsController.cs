using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuerillaTrader.Entities;
using GuerillaTrader.Entities.Dtos;
using GuerillaTrader.Services;
using GuerillaTrader.Web.Framework;

namespace GuerillaTrader.Web.Controllers
{
    public class MarketsController : GuerillaTraderControllerBase
    {
        readonly IRepository<Market> _marketRepository;
        readonly IMarketAppService _marketAppService;
        readonly IObjectMapper _objectMapper;

        public MarketsController(IRepository<Market> marketRepository, IMarketAppService marketAppService, IObjectMapper objectMapper)
        {
            _marketRepository = marketRepository;
            _marketAppService = marketAppService;
            _objectMapper = objectMapper;
        }

        // GET: Market
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(VaryByParam = "cacheCounter", Duration = 360000000)]
        public ActionResult GetMarkets(int cacheCounter)
        {
            return new GuerillaLogisticsApiJsonResult(_marketAppService.GetAllActive());
        }

        [OutputCache(VaryByParam = "cacheCounter", Duration = 360000000)]
        public ActionResult GetAllMarkets(int cacheCounter)
        {
            return new GuerillaLogisticsApiJsonResult(_marketAppService.GetAll());
        }

        #region Markets_Read
        public ActionResult Markets_Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = new DataSourceResult();

            result.Data = _objectMapper.Map<List<MarketDto>>(_marketRepository.GetAll().Where(request.Filters).OrderBy(request.Sorts[0]).ToList());
            result.Total = _marketRepository.GetAll().Where(request.Filters).Count();

            return new GuerillaLogisticsApiJsonResult(result);
        }
        #endregion

        #region Market_Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Market_Create([DataSourceRequest] DataSourceRequest request, MarketDto model)
        {
            this.SettingManager.IncrementCacheCounter("Markets");

            if (model != null && ModelState.IsValid)
            {
                this._marketAppService.Save(model);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Market_Update
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Market_Update([DataSourceRequest] DataSourceRequest request, MarketDto model)
        {
            this.SettingManager.IncrementCacheCounter("Markets");

            if (model != null && ModelState.IsValid)
            {
                this._marketAppService.Save(model);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Market_Destroy
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Market_Destroy([DataSourceRequest] DataSourceRequest request, MarketDto model)
        {
            this.SettingManager.IncrementCacheCounter("Markets");

            if (model != null)
            {
                this._marketRepository.Delete(model.Id);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        #endregion
    }
}