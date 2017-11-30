namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPerfFieldsToTradingAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradingAccounts", "TotalReturn", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.TradingAccounts", "CAGR", c => c.Decimal(nullable: false, precision: 18, scale: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradingAccounts", "CAGR");
            DropColumn("dbo.TradingAccounts", "TotalReturn");
        }
    }
}
