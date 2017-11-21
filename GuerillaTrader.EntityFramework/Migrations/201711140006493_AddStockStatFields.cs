namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStockStatFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockReports", "Perf", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.StockReports", "CAGR", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Stocks", "RecentPerf", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Stocks", "RecentCAGR", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Stocks", "PastPositivePerf", c => c.Decimal(nullable: false, precision: 18, scale: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "PastPositivePerf");
            DropColumn("dbo.Stocks", "RecentCAGR");
            DropColumn("dbo.Stocks", "RecentPerf");
            DropColumn("dbo.StockReports", "CAGR");
            DropColumn("dbo.StockReports", "Perf");
        }
    }
}
