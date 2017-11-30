using System.Reflection;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Zero;
using Abp.Zero.Configuration;
using GuerillaTrader.Authorization;
using GuerillaTrader.Authorization.Roles;
using GuerillaTrader.MultiTenancy;
using GuerillaTrader.Users;
using GuerillaTrader.Shared;
using Abp.AutoMapper;
using GuerillaTrader.Entities;
using GuerillaTrader.Framework;
using GuerillaTrader.Entities.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GuerillaTrader
{
    [DependsOn(typeof(AbpZeroCoreModule), typeof(AbpAutoMapperModule), typeof(GuerillaTraderSharedModule))]
    public class GuerillaTraderCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

			Configuration.Modules.AbpAutoMapper().Configurators.Add(config =>
            {
                #region Trade
                config.CreateMap<Trade, TradeDto>()
                              .ForMember(u => u.Market, options => options.MapFrom(input => input.Market.Symbol))
                              //.ForMember(u => u.Stock, options => options.MapFrom(input => input.Stock.Symbol))
                              //.ForMember(u => u.CoveredCallOption, options => options.MapFrom(input => input.CoveredCallOption.Name))
                              //.ForMember(u => u.BullPutSpreadShortOption, options => options.MapFrom(input => input.BullPutSpreadShortOption.Name))
                              //.ForMember(u => u.BullPutSpreadLongOption, options => options.MapFrom(input => input.BullPutSpreadLongOption.Name))
                              .ForMember(u => u.EntrySetups, options => options.MapFrom(input => EnumExtensions.FlaggedEnumToList<TradingSetups>(input.EntrySetups)))
                              .ForMember(u => u.EntryScreenshotDbId, options => options.MapFrom(input => input.EntryScreenshotDbId.HasValue ? input.EntryScreenshotDbId.Value : 0))
                              .ForMember(u => u.ExitScreenshotDbId, options => options.MapFrom(input => input.ExitScreenshotDbId.HasValue ? input.ExitScreenshotDbId.Value : 0));

                config.CreateMap<TradeDto, Trade>()
                      .ForMember(u => u.EntrySetups, options => options.MapFrom(input => EnumExtensions.ListToFlaggedEnum<TradingSetups>(input.EntrySetups)))
                              .ForMember(u => u.EntryScreenshotDbId, options => options.MapFrom(input => Extensions.GetNullableValue(input.EntryScreenshotDbId)))
                              .ForMember(u => u.ExitScreenshotDbId, options => options.MapFrom(input => Extensions.GetNullableValue(input.ExitScreenshotDbId)));
                #endregion

                #region MarketLogEntry
                config.CreateMap<MarketLogEntry, MarketLogEntryDto>()
                              .ForMember(u => u.Market, options => options.MapFrom(input => input.Market.Symbol))
                              .ForMember(u => u.ScreenshotDbId, options => options.MapFrom(input => input.ScreenshotDbId.HasValue ? input.ScreenshotDbId.Value : 0));

                config.CreateMap<MarketLogEntryDto, MarketLogEntry>();
                #endregion

                #region MonteCarloSimulation
                config.CreateMap<MonteCarloSimulation, MonteCarloSimulationDto>()
                              .ForMember(u => u.TradingAccount, options => options.MapFrom(input => input.TradingAccount.Name))
                              .ForMember(u => u.MarketMaxContractsList, options => options.MapFrom(input => JsonConvert.DeserializeObject<List<MarketMaxContracts>>(input.MarketMaxContractsJson)));

                config.CreateMap<MonteCarloSimulationDto, MonteCarloSimulation>()
                    .ForMember(u => u.TradingAccount, options => options.Ignore())
                    .ForMember(u => u.MarketMaxContractsJson, options => options.MapFrom(input => JsonConvert.SerializeObject(input.MarketMaxContractsList)));
                #endregion

                #region Market
                config.CreateMap<Market, TosMarketDto>()
                    .ForMember(u => u.Id, options => options.MapFrom(input => input.Id))
                    .ForMember(u => u.TickValue, options => options.MapFrom(input => input.TickValue))
                    .ForAllOtherMembers(options => options.Ignore());
                #endregion

                //Configuration.Settings.Providers.Add<MySettingProvider>();
            });

            //Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            //Remove the following line to disable multi-tenancy.
            Configuration.MultiTenancy.IsEnabled = GuerillaTraderConsts.MultiTenancyEnabled;

            //Add/remove localization sources here
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    GuerillaTraderConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "GuerillaTrader.Localization.Source"
                        )
                    )
                );

            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Authorization.Providers.Add<GuerillaTraderAuthorizationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
