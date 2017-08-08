using Abp.Web.Mvc.Views;

namespace GuerillaTrader.Web.Views
{
    public abstract class GuerillaTraderWebViewPageBase : GuerillaTraderWebViewPageBase<dynamic>
    {

    }

    public abstract class GuerillaTraderWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected GuerillaTraderWebViewPageBase()
        {
            LocalizationSourceName = GuerillaTraderConsts.LocalizationSourceName;
        }
    }
}