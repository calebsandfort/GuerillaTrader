namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTradingAccountSnapshotEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TradingAccountSnapshots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TotalReturn = c.Decimal(nullable: false, precision: 18, scale: 7),
                        CAGR = c.Decimal(nullable: false, precision: 18, scale: 7),
                        Date = c.DateTime(nullable: false),
                        TradingAccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TradingAccounts", t => t.TradingAccountId, cascadeDelete: true)
                .Index(t => t.TradingAccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TradingAccountSnapshots", "TradingAccountId", "dbo.TradingAccounts");
            DropIndex("dbo.TradingAccountSnapshots", new[] { "TradingAccountId" });
            DropTable("dbo.TradingAccountSnapshots");
        }
    }
}
