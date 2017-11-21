using Abp.Application.Navigation;
using Abp.Localization;
using GuerillaTrader.Authorization;

namespace GuerillaTrader.Web
{
    /// <summary>
    /// This class defines menus for the application.
    /// It uses ABP's menu system.
    /// When you add menu items here, they are automatically appear in angular application.
    /// See Views/Layout/_TopMenu.cshtml file to know how to render menu.
    /// </summary>
    public class GuerillaTraderNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        "Dashboard",
                        new LocalizableString("Dashboard", GuerillaTraderConsts.LocalizationSourceName),
                        url: "Dashboard",
                        icon: "fa fa-dashboard"
                        )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "MarketLog",
                        new LocalizableString("MarketLog", GuerillaTraderConsts.LocalizationSourceName),
                        url: "MarketLog",
                        icon: "fa fa-pencil-square-o"
                        )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "TradingAccounts",
                        new LocalizableString("TradingAccounts", GuerillaTraderConsts.LocalizationSourceName),
                        url: "TradingAccounts",
                        icon: "fa fa-book"
                        )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "Markets",
                        new LocalizableString("Markets", GuerillaTraderConsts.LocalizationSourceName),
                        url: "Markets",
                        icon: "fa fa-money"
                        )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "MonteCarloSimulations",
                        new LocalizableString("MonteCarloSimulations", GuerillaTraderConsts.LocalizationSourceName),
                        url: "MonteCarloSimulations",
                        icon: "fa fa-cloud"
                        )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "Stocks",
                        new LocalizableString("Stocks", GuerillaTraderConsts.LocalizationSourceName),
                        url: "Stocks",
                        icon: "fa fa-line-chart"
                        )
                ).AddItem(
                    new MenuItemDefinition(
                        "Users",
                        L("Users"),
                        url: "Users",
                        icon: "fa fa-users",
                        requiredPermissionName: PermissionNames.Pages_Users
                        )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, GuerillaTraderConsts.LocalizationSourceName);
        }
    }
}
