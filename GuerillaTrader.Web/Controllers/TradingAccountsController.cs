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
using GuerillaTrader.Web.Models;

namespace GuerillaTrader.Web.Controllers
{
    public class TradingAccountsController : GuerillaTraderControllerBase
    {
        readonly IRepository<TradingAccount> _tradingAccountRepository;
        readonly ITradingAccountAppService _tradingAccountAppService;
        readonly IRepository<TradingAccountSnapshot> _tradingAccountSnapshotRepository;
        readonly IObjectMapper _objectMapper;

        public TradingAccountsController(IRepository<TradingAccount> tradingAccountRepository, IRepository<TradingAccountSnapshot> tradingAccountSnapshotRepository, ITradingAccountAppService tradingAccountAppService, IObjectMapper objectMapper)
        {
            _tradingAccountRepository = tradingAccountRepository;
            _tradingAccountSnapshotRepository = tradingAccountSnapshotRepository;
            _tradingAccountAppService = tradingAccountAppService;
            _objectMapper = objectMapper;
        }

        // GET: TradingAccounts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            TradingAccountDetailsModel model = new TradingAccountDetailsModel
            {
                TradingAccount = _objectMapper.Map<TradingAccountDto>(_tradingAccountRepository.GetAllIncluding(x => x.Snapshots).Single(x => x.Id == id)),
                Snapshots = _objectMapper.Map<List<TradingAccountSnapshotDto>>(_tradingAccountSnapshotRepository.GetAll().Where(x => x.TradingAccountId == id).OrderBy(x => x.Date).ToList())
            };

            return View(model);
        }

        [OutputCache(VaryByParam = "cacheCounter", Duration = 360000000)]
        public ActionResult GetTradingAccounts(int cacheCounter)
        {
            return new GuerillaLogisticsApiJsonResult(_tradingAccountAppService.GetAll());
        }

        [ChildActionOnly]
        public PartialViewResult TradingAccountMenu()
        {
            return PartialView("_TradingAccountMenu", this._tradingAccountAppService.GetAll());
        }

        #region MarketLog_Read
        public ActionResult GetActiveAsListItems()
        {
            DataSourceResult result = new DataSourceResult();
            TradingAccountDto activeAccount = _tradingAccountAppService.GetActive();

            List<ListItem> listItems = new List<ListItem>();
            listItems.Add(new ListItem() { Display = "Name", Value = activeAccount.Name });
            listItems.Add(new ListItem() { Display = "Net Liq", Value = activeAccount.CurrentCapital.ToString("C") });
            listItems.Add(new ListItem() { Display = "P/L", Value = activeAccount.ProfitLoss.ToString("C") });
            listItems.Add(new ListItem() { Display = "Commissions", Value = activeAccount.Commissions.ToString("C") });
            listItems.Add(new ListItem() { Display = "Adj P/L", Value = activeAccount.AdjProfitLoss.ToString("C") });
            listItems.Add(new ListItem() { Display = "Total Return", Value = activeAccount.TotalReturn.ToString("P2") });
            listItems.Add(new ListItem() { Display = "CAGR", Value = activeAccount.CAGR.ToString("P2") });

            result.Data = listItems;

            return new GuerillaLogisticsApiJsonResult(result);
        }
        #endregion

        #region TradingAccounts_Read
        public ActionResult TradingAccounts_Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = new DataSourceResult();

            result.Data = _objectMapper.Map<List<TradingAccountDto>>(_tradingAccountRepository.GetAllIncluding(x => x.Snapshots).Where(request.Filters).OrderBy(request.Sorts[0]).ToList());
            result.Total = _tradingAccountRepository.GetAll().Where(request.Filters).Count();

            return new GuerillaLogisticsApiJsonResult(result);
        }
        #endregion

        #region TradingAccount_Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TradingAccount_Create([DataSourceRequest] DataSourceRequest request, TradingAccountDto model)
        {
            this.SettingManager.IncrementCacheCounter("TradingAccounts");

            if (model != null && ModelState.IsValid)
            {
                this._tradingAccountAppService.Save(model);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region TradingAccount_Update
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TradingAccount_Update([DataSourceRequest] DataSourceRequest request, TradingAccountDto model)
        {
            this.SettingManager.IncrementCacheCounter("TradingAccounts");

            if (model != null && ModelState.IsValid)
            {
                this._tradingAccountAppService.Save(model);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region TradingAccount_Destroy
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TradingAccount_Destroy([DataSourceRequest] DataSourceRequest request, TradingAccountDto model)
        {
            this.SettingManager.IncrementCacheCounter("TradingAccounts");

            if (model != null)
            {
                this._tradingAccountRepository.Delete(model.Id);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        #endregion
    }
}