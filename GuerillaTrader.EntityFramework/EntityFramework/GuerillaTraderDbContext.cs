using System.Data.Common;
using Abp.Zero.EntityFramework;
using GuerillaTrader.Authorization.Roles;
using GuerillaTrader.MultiTenancy;
using GuerillaTrader.Users;
using GuerillaTrader.Entities;
using System.Data.Entity;

namespace GuerillaTrader.EntityFramework
{
    public class GuerillaTraderDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        //TODO: Define an IDbSet for your Entities...
		public virtual IDbSet<Trade> Trades { get; set; }
        public virtual IDbSet<TradingAccount> TradingAccounts { get; set; }
        public virtual IDbSet<TradingDay> TradingDays { get; set; }
        public virtual IDbSet<MarketLogEntry> MarketLogEntries { get; set; }
        public virtual IDbSet<TradingDirective> TradingDirectives { get; set; }
        public virtual IDbSet<Market> Markets { get; set; }
        public virtual IDbSet<MonteCarloSimulation> MonteCarloSimulations { get; set; }
        public virtual IDbSet<Screenshot> Screenshots { get; set; }		

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public GuerillaTraderDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in GuerillaTraderDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of GuerillaTraderDbContext since ABP automatically handles it.
         */
        public GuerillaTraderDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public GuerillaTraderDbContext(DbConnection existingConnection)
         : base(existingConnection, false)
        {

        }

        public GuerillaTraderDbContext(DbConnection existingConnection, bool contextOwnsConnection)
         : base(existingConnection, contextOwnsConnection)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<decimal>().Configure(c => c.HasPrecision(18, 6));

            modelBuilder.Entity<Trade>().HasOptional(m => m.EntryScreenshotDb)
                                 .WithMany(m => m.EntryTrades).HasForeignKey(m => m.EntryScreenshotDbId);
            modelBuilder.Entity<Trade>().HasOptional(m => m.ExitScreenshotDb)
                                 .WithMany(m => m.ExitTrades).HasForeignKey(m => m.ExitScreenshotDbId);
            modelBuilder.Entity<MarketLogEntry>().HasOptional(m => m.ScreenshotDb)
                                 .WithMany(m => m.MarketLogEntries).HasForeignKey(m => m.ScreenshotDbId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
