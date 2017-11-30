namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTradingAccountSnapshotEntity2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradingAccountSnapshots", "CurrentCapital", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.TradingAccountSnapshots", "Commissions", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.TradingAccountSnapshots", "ProfitLoss", c => c.Decimal(nullable: false, precision: 18, scale: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradingAccountSnapshots", "ProfitLoss");
            DropColumn("dbo.TradingAccountSnapshots", "Commissions");
            DropColumn("dbo.TradingAccountSnapshots", "CurrentCapital");
        }
    }
}
