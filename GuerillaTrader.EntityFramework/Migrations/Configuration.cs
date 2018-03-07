using System.Data.Entity.Migrations;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using GuerillaTrader.Migrations.SeedData;
using EntityFramework.DynamicFilters;

namespace GuerillaTrader.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<GuerillaTrader.EntityFramework.GuerillaTraderDbContext>, IMultiTenantSeed
    {
        public AbpTenantBase Tenant { get; set; }

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "GuerillaTrader";
        }

        protected override void Seed(GuerillaTrader.EntityFramework.GuerillaTraderDbContext context)
        {
            context.DisableAllFilters();

            if (Tenant == null)
            {
                //Host seed
                new InitialHostDbBuilder(context).Create();

                //Default tenant seed (in host database).
                new DefaultTenantCreator(context).Create();
                new TenantRoleAndUserBuilder(context, 1).Create();
                new MarketsCreator(context).Create();
                new SectorsCreator(context).Create();
                new TradeSettingsCreator(context).Create();
            }
            else
            {
                //You can add seed for tenant databases and use Tenant property...
                new MarketsCreator(context).Create();
            }

            context.SaveChanges();
        }
    }
}
