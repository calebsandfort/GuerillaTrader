using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuerillaTrader.Entities;
using GuerillaTrader.Web.Framework;

namespace GuerillaTrader.Web.Controllers
{
    public class ViewRendererController : GuerillaTraderControllerBase
    {
        readonly IRepository<TradingDirective> _tradingDirectiveRepository;
        readonly IObjectMapper _objectMapper;

        public ViewRendererController(IRepository<TradingDirective> tradingDirectiveRepository, IObjectMapper objectMapper)
        {
            _tradingDirectiveRepository = tradingDirectiveRepository;
            _objectMapper = objectMapper;
        }

        #region TradingDirectives_Read
        public ActionResult TradingDirectives_Read([DataSourceRequest] DataSourceRequest request, TradingDirectiveTypes tradingDirectiveType)
        {
            DataSourceResult result = new DataSourceResult();

            result.Data = _tradingDirectiveRepository.GetAll().Where(x => x.TradingDirectiveType == tradingDirectiveType).ToList();

            return new GuerillaLogisticsApiJsonResult(result);
        }
        #endregion

        public ActionResult GetMarketLogEntryTypes()
        {
            return new GuerillaLogisticsApiJsonResult(Extensions.EnumToListItems<MarketLogEntryTypes>());
        }

        public ActionResult GetTradeTypes()
        {
            return new GuerillaLogisticsApiJsonResult(Extensions.EnumToListItems<TradeTypes>());
        }

        public ActionResult GetTradeExitReasons()
        {
            return new GuerillaLogisticsApiJsonResult(Extensions.EnumToListItems<TradeExitReasons>(true));
        }

        public ActionResult GetTradeClassifications()
        {
            return new GuerillaLogisticsApiJsonResult(Extensions.EnumToListItems<TradeClassifications>());
        }
    }
}