namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPastPerfFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sectors", "PastPerf", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Stocks", "PastPerf", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            DropColumn("dbo.StockReports", "CAGR");
            DropColumn("dbo.Stocks", "RecentCAGR");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stocks", "RecentCAGR", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.StockReports", "CAGR", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            DropColumn("dbo.Stocks", "PastPerf");
            DropColumn("dbo.Sectors", "PastPerf");
        }
    }
}
