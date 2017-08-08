using Abp.Modules;
using Abp.MultiTenancy;
using Abp.TestBase;
using Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;
using GuerillaTrader.EntityFramework;
using GuerillaTrader.Shared;
using NSubstitute;
using System.Data.Entity;
using System.Reflection;

namespace GuerillaTrader.Tests
{
    [DependsOn(typeof(GuerillaTraderApplicationModule), typeof(GuerillaTraderSharedModule),
        typeof(GuerillaTraderCoreModule), typeof(GuerillaTraderDataModule), typeof(AbpTestBaseModule))]
    public class GuerillaTraderTestModule : AbpModule
    {
        private readonly string _connectionString;

        public GuerillaTraderTestModule()
        {

        }

        public override void PreInitialize()
        {
            Database.SetInitializer<GuerillaTraderDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
