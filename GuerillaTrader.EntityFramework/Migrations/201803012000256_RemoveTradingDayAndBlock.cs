namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTradingDayAndBlock : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MarketLogEntries", "TradingDayId", "dbo.TradingDays");
            DropForeignKey("dbo.Trades", "TradingDayId", "dbo.TradingDays");
            DropIndex("dbo.MarketLogEntries", new[] { "TradingDayId" });
            DropIndex("dbo.Trades", new[] { "TradingDayId" });
            DropColumn("dbo.MarketLogEntries", "TradingDayId");
            DropColumn("dbo.Trades", "Block");
            DropColumn("dbo.Trades", "TradingDayId");
            DropTable("dbo.TradingDays");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TradingDays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Day = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Trades", "TradingDayId", c => c.Int(nullable: false));
            AddColumn("dbo.Trades", "Block", c => c.Int(nullable: false));
            AddColumn("dbo.MarketLogEntries", "TradingDayId", c => c.Int(nullable: false));
            CreateIndex("dbo.Trades", "TradingDayId");
            CreateIndex("dbo.MarketLogEntries", "TradingDayId");
            AddForeignKey("dbo.Trades", "TradingDayId", "dbo.TradingDays", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MarketLogEntries", "TradingDayId", "dbo.TradingDays", "Id", cascadeDelete: true);
        }
    }
}
