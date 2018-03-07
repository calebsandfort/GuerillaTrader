namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPerfFields1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradingAccounts", "AdjProfitLoss", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.TradingAccounts", "R", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.TradingAccounts", "MaxDrawdown", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.TradingAccountSnapshots", "AdjProfitLoss", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.TradingAccountSnapshots", "R", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.TradingAccountSnapshots", "MaxDrawdown", c => c.Decimal(nullable: false, precision: 18, scale: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradingAccountSnapshots", "MaxDrawdown");
            DropColumn("dbo.TradingAccountSnapshots", "R");
            DropColumn("dbo.TradingAccountSnapshots", "AdjProfitLoss");
            DropColumn("dbo.TradingAccounts", "MaxDrawdown");
            DropColumn("dbo.TradingAccounts", "R");
            DropColumn("dbo.TradingAccounts", "AdjProfitLoss");
        }
    }
}
