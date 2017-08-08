using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.TestBase;
using GuerillaTrader.EntityFramework;
using GuerillaTrader.Migrations.SeedData;
using GuerillaTrader.MultiTenancy;
using GuerillaTrader.Users;
using Castle.MicroKernel.Registration;
using Effort;
using EntityFramework.DynamicFilters;
using System.Configuration;

namespace GuerillaTrader.Tests
{
    public abstract class GuerillaTraderTestBase : AbpIntegratedTestBase<GuerillaTraderTestModule>
    {
        private readonly string _connectionString;

        protected GuerillaTraderTestBase()
        {
            Resolve<IMultiTenancyConfig>().IsEnabled = true;
            _connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

            Resolve<IAbpStartupConfiguration>().DefaultNameOrConnectionString = _connectionString;
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            //LocalIocManager.IocContainer.Register(
            //    Component.For<DbConnection>()
            //             .UsingFactoryMethod(() =>
            //             {
            //                 var connection = new SqlConnection(_connectionString);
            //                 return connection;
            //             })
            //             .LifestyleSingleton()
            //);
        }
    }
}