using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.Runtime.Validation;
using GuerillaTrader.Entities;
using GuerillaTrader.Entities.Dtos;
using GuerillaTrader.Web.Framework;
using Kendo.DynamicLinq;
//using GuerillaTrader.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuerillaTrader.Web.Controllers
{
    public class StocksController : Controller
    {
        readonly IRepository<Stock> _stockRepository;
        readonly IRepository<Sector> _sectorRepository;
        readonly IRepository<StockBar> _stockBarRepository;
        readonly IObjectMapper _objectMapper;

        public StocksController(IRepository<Stock> stockRepository, IRepository<Sector> sectorRepository, IRepository<StockBar> stockBarRepository, IObjectMapper objectMapper)
        {
            _stockRepository = stockRepository;
            _sectorRepository = sectorRepository;
            _stockBarRepository = stockBarRepository;
            _objectMapper = objectMapper;
        }

        // GET: Stocks
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            return View(_objectMapper.Map<ViewStockDto>(_stockRepository.Get(id)));
        }

        [OutputCache(VaryByParam = "id", Duration = 360000000)]
        public ActionResult GetBars(int id)
        {
            return new GuerillaLogisticsApiJsonResult(_objectMapper.Map<List<StockBarDto>>(this._stockBarRepository.GetAll().Where(x => x.StockReportId == id).OrderBy(x => x.Date).ToList()));
        }

        #region Stocks_Read
        [DisableValidation]
        public ActionResult Stocks_Read([System.Web.Http.FromBody]DataSourceRequest realRequest)
        {
            Func<IQueryable<Stock>, List<ViewStockDto>> map = q => _objectMapper.Map<List<ViewStockDto>>(q.ToList());

            DataSourceResult result = _stockRepository.GetAll().Where(x => !x.FailedToRetrieveBars).ToDataSourceResult(realRequest, map);

            return new Framework.GuerillaLogisticsApiJsonResult(result);
        }
        #endregion

        #region Sectors_Read
        [DisableValidation]
        public ActionResult Sectors_Read([System.Web.Http.FromBody]DataSourceRequest realRequest)
        {
            Func<IQueryable<Sector>, List<SectorDto>> map = q => _objectMapper.Map<List<SectorDto>>(q.ToList());

            DataSourceResult result = _sectorRepository.GetAll().ToDataSourceResult(realRequest, map);

            return new Framework.GuerillaLogisticsApiJsonResult(result);
        }
        #endregion

        public ActionResult FilterMenu_Sectors()
        {
            List<String> sectors = new List<string>() {
                "Consumer Discretionary",
                "Consumer Staples",
                "Energy",
                "Financials",
                "Health Care",
                "Industrials",
                "Information Technology",
                "Materials",
                "Real Estate",
                "Telecommunication Services",
                "Utilities" };

            return this.Json(sectors.Select(x => new { Sector = x }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerateStockReportsModal()
        {
            GenerateStockReportsInput model = new GenerateStockReportsInput();
            model.StartDate = DateTime.Now.AddDays(-1);
            model.EndDate = model.StartDate.AddMonths(2);
            model.Lookback = 5;

            return PartialView("Modals/_GenerateStockReportsModal", model);
        }
    }
}