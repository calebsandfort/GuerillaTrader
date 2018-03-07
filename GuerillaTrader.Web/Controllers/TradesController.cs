using Abp.AutoMapper;
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
    public class TradesController : Controller
    {
        readonly IRepository<Trade> _tradeRepository;
        readonly ITradeAppService _tradeAppService;
        readonly IObjectMapper _objectMapper;

        public TradesController(IRepository<Trade> tradeRepository, ITradeAppService tradeAppService, IObjectMapper objectMapper)
        {
            _tradeRepository = tradeRepository;
            _tradeAppService = tradeAppService;
            _objectMapper = objectMapper;
        }

        // GET: Trades
        public ActionResult Index()
        {
            return View();
        }

        #region Trades_Read
        public ActionResult Trades_Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = new DataSourceResult();

            result.Data = _objectMapper.Map<List<TradeDto>>(_tradeRepository.GetAll().Where(request.Filters).Where(x => x.TradingAccount.Active).OrderBy(request.Sorts[0]).Take(20).ToList());
            //result.Total = _tradeRepository.GetAll().Where(request.Filters).Where(x => x.TradingAccount.Active).Count();
            result.Total = 20;

            return new GuerillaLogisticsApiJsonResult(result);
        }
        #endregion

        #region Trade_Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Trade_Create([DataSourceRequest] DataSourceRequest request, TradeDto model)
        {
            if (model != null && ModelState.IsValid)
            {
                this._tradeAppService.Save(model);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Trade_Update
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Trade_Update([DataSourceRequest] DataSourceRequest request, TradeDto model)
        {
            if (model != null && ModelState.IsValid)
            {
                this._tradeAppService.Save(model);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Trade_Destroy
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Trade_Destroy([DataSourceRequest] DataSourceRequest request, TradeDto model)
        {
            if (model != null)
            {
                this._tradeRepository.Delete(model.Id);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        public ActionResult TradeModal(int id)
        {
            TradeDto model = new TradeDto();

            if (id == 0)
            {

                model.EntryDate = DateTime.Now;
                model.Size = 1;
                if (this._tradeRepository.Count(x => x.TradingAccount.Active) > 0)
                {
                    Trade lastTrade = this._tradeRepository.GetAll().Where(x => x.TradingAccount.Active).OrderByDescending(x => x.EntryDate).First();

                    model.EntryDate = lastTrade.EntryDate;
                    model.MarketId = lastTrade.MarketId;
                    model.Size = lastTrade.Size;
                }

                model.ExitDate = model.EntryDate.AddHours(1);
            }
            else
            {
                Trade trade = this._tradeRepository.GetAllIncluding(x => x.Market).Single(x => x.Id == id);
                trade.MapTo(model);
            }

            return PartialView("Modals/_TradeModal", model);
        }
    }
}