using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using GuerillaTrader.EntityFramework;

namespace GuerillaTrader.Migrator
{
    [DependsOn(typeof(GuerillaTraderDataModule))]
    public class GuerillaTraderMigratorModule : AbpModule
    {
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