using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuerillaTrader.Web
{
    public class MySettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
                    {
                    new SettingDefinition(
                        "MarketsCacheCounter",
                        "0",
                        scopes: SettingScopes.Application,
                        isVisibleToClients: true
                        ),
                    new SettingDefinition(
                        "TradingAccountsCacheCounter",
                        "0",
                        scopes: SettingScopes.Application,
                        isVisibleToClients: true
                        )

                };
        }
    }
}