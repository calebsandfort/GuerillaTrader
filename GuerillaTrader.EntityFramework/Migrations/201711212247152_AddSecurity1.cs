namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSecurity1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MarketLogEntries", "MarketId", "dbo.Markets");
            DropForeignKey("dbo.Trades", "MarketId", "dbo.Markets");
            DropIndex("dbo.MarketLogEntries", new[] { "MarketId" });
            DropIndex("dbo.Trades", new[] { "MarketId" });
            AddColumn("dbo.MarketLogEntries", "StockId", c => c.Int());
            AddColumn("dbo.Markets", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Trades", "StockId", c => c.Int());
            AddColumn("dbo.Stocks", "TickValue", c => c.Decimal(nullable: false, precision: 18, scale: 7, defaultValueSql: ".01"));
            AddColumn("dbo.Stocks", "TickSize", c => c.Decimal(nullable: false, precision: 18, scale: 7, defaultValueSql: ".01"));
            AlterColumn("dbo.MarketLogEntries", "MarketId", c => c.Int());
            AlterColumn("dbo.Trades", "MarketId", c => c.Int());
            CreateIndex("dbo.MarketLogEntries", "MarketId");
            CreateIndex("dbo.MarketLogEntries", "StockId");
            CreateIndex("dbo.Trades", "MarketId");
            CreateIndex("dbo.Trades", "StockId");
            AddForeignKey("dbo.Trades", "StockId", "dbo.Stocks", "Id");
            AddForeignKey("dbo.MarketLogEntries", "StockId", "dbo.Stocks", "Id");
            AddForeignKey("dbo.MarketLogEntries", "MarketId", "dbo.Markets", "Id");
            AddForeignKey("dbo.Trades", "MarketId", "dbo.Markets", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trades", "MarketId", "dbo.Markets");
            DropForeignKey("dbo.MarketLogEntries", "MarketId", "dbo.Markets");
            DropForeignKey("dbo.MarketLogEntries", "StockId", "dbo.Stocks");
            DropForeignKey("dbo.Trades", "StockId", "dbo.Stocks");
            DropIndex("dbo.Trades", new[] { "StockId" });
            DropIndex("dbo.Trades", new[] { "MarketId" });
            DropIndex("dbo.MarketLogEntries", new[] { "StockId" });
            DropIndex("dbo.MarketLogEntries", new[] { "MarketId" });
            AlterColumn("dbo.Trades", "MarketId", c => c.Int(nullable: false));
            AlterColumn("dbo.MarketLogEntries", "MarketId", c => c.Int(nullable: false));
            DropColumn("dbo.Stocks", "TickSize");
            DropColumn("dbo.Stocks", "TickValue");
            DropColumn("dbo.Trades", "StockId");
            DropColumn("dbo.Markets", "Price");
            DropColumn("dbo.MarketLogEntries", "StockId");
            CreateIndex("dbo.Trades", "MarketId");
            CreateIndex("dbo.MarketLogEntries", "MarketId");
            AddForeignKey("dbo.Trades", "MarketId", "dbo.Markets", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MarketLogEntries", "MarketId", "dbo.Markets", "Id", cascadeDelete: true);
        }
    }
}
