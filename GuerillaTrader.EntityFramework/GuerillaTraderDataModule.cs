using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using GuerillaTrader.EntityFramework;

namespace GuerillaTrader
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(GuerillaTraderCoreModule))]
    public class GuerillaTraderDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<GuerillaTraderDbContext>());

            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
