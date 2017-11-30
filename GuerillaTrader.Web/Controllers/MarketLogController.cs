using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using GuerillaTrader.Entities;
using GuerillaTrader.Entities.Dtos;
using GuerillaTrader.Web.Framework;
using GuerillaTrader.Web.Models;

namespace GuerillaTrader.Web.Controllers
{
    public class MarketLogController : GuerillaTraderControllerBase
    {
        readonly IRepository<MarketLogEntry> _marketLogEntryRepository;
        readonly IRepository<TradingDirective> _tradingDirectiveRepository;
        readonly IObjectMapper _objectMapper;

        public MarketLogController(IRepository<MarketLogEntry> marketLogEntryRepository, IRepository<TradingDirective> tradingDirectiveRepository, IObjectMapper objectMapper)
        {
            _marketLogEntryRepository = marketLogEntryRepository;
            _tradingDirectiveRepository = tradingDirectiveRepository;
            _objectMapper = objectMapper;
        }

        // GET: Log
        public ActionResult Index()
        {
            return View();
        }

        #region MarketLog_Read
        public ActionResult MarketLog_Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = new DataSourceResult();

            //DateTime currentDate = Clock.Now;

            //Expression<Func<NbaGame, bool>> todayFunc = x => x.Date.Year == currentDate.Year && x.Date.Month == currentDate.Month && x.Date.Day == currentDate.Day;

            //result.Data = _objectMapper.Map<List<MarketLogEntryDto>>(_movieRepository.GetAllIncluding(x => x.StatLines.Select(y => y.Participant)).Where(request.Filters).Where(todayFunc).OrderBy(request.Sorts[0]).ToList());
            //result.Total = _marketLogEntryRepository.GetAll().Where(request.Filters).Where(todayFunc).Count();

            result.Data = _objectMapper.Map<List<MarketLogEntryDto>>(_marketLogEntryRepository.GetAllIncluding(x => x.Market).Where(x => x.TradingAccount.Active).OrderByDescending(x => x.TimeStamp).ThenByDescending(x => x.Id).Take(100).ToList());

            return new GuerillaLogisticsApiJsonResult(result);
        }
        #endregion

        public ActionResult AddLogEntryModal()
        {
            AddLogEntryModel model = new AddLogEntryModel();

            model.Date = DateTime.Now;
            if(this._marketLogEntryRepository.Count(x => x.TradingAccount.Active) > 0)
            {
                MarketLogEntry lastEntry = this._marketLogEntryRepository.GetAll().Where(x => x.TradingAccount.Active).OrderByDescending(x => x.TimeStamp).First();

                model.Date = lastEntry.TimeStamp;
                if(lastEntry.MarketId.HasValue) model.MarketId = lastEntry.MarketId.Value;
            }

            return PartialView("Modals/_AddLogEntryModal", model);
        }
    }
}