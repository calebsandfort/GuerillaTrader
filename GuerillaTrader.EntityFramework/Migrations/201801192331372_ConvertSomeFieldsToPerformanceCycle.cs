namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConvertSomeFieldsToPerformanceCycle : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TradingAccounts", "R");
            DropColumn("dbo.TradingAccounts", "MaxDrawdown");
            DropColumn("dbo.TradingAccountSnapshots", "R");
            DropColumn("dbo.TradingAccountSnapshots", "MaxDrawdown");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TradingAccountSnapshots", "MaxDrawdown", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.TradingAccountSnapshots", "R", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.TradingAccounts", "MaxDrawdown", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.TradingAccounts", "R", c => c.Decimal(nullable: false, precision: 18, scale: 7));
        }
    }
}
