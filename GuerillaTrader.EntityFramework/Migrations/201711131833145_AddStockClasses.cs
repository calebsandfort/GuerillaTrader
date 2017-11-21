namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStockClasses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockBars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Open = c.Decimal(nullable: false, precision: 18, scale: 7),
                        High = c.Decimal(nullable: false, precision: 18, scale: 7),
                        Low = c.Decimal(nullable: false, precision: 18, scale: 7),
                        Close = c.Decimal(nullable: false, precision: 18, scale: 7),
                        StockReportId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StockReports", t => t.StockReportId, cascadeDelete: true)
                .Index(t => t.StockReportId);
            
            CreateTable(
                "dbo.StockReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        StockId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stocks", t => t.StockId, cascadeDelete: true)
                .Index(t => t.StockId);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Symbol = c.String(),
                        Yield = c.Decimal(nullable: false, precision: 18, scale: 7),
                        DividendYieldScore = c.Int(nullable: false),
                        ChasFlowScore = c.Int(nullable: false),
                        RelativeValueScore = c.Int(nullable: false),
                        TotalScore = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 7),
                        IdealValue = c.Decimal(nullable: false, precision: 18, scale: 7),
                        Sector = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockBars", "StockReportId", "dbo.StockReports");
            DropForeignKey("dbo.StockReports", "StockId", "dbo.Stocks");
            DropIndex("dbo.StockReports", new[] { "StockId" });
            DropIndex("dbo.StockBars", new[] { "StockReportId" });
            DropTable("dbo.Stocks");
            DropTable("dbo.StockReports");
            DropTable("dbo.StockBars");
        }
    }
}
