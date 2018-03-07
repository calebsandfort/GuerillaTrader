namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPerformanceCycleEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PerformanceCycles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Position = c.Int(nullable: false),
                        CycleType = c.Int(nullable: false),
                        R = c.Decimal(nullable: false, precision: 18, scale: 7),
                        MaxDrawdown = c.Decimal(nullable: false, precision: 18, scale: 7),
                        TradingAccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TradingAccounts", t => t.TradingAccountId, cascadeDelete: true)
                .Index(t => t.TradingAccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PerformanceCycles", "TradingAccountId", "dbo.TradingAccounts");
            DropIndex("dbo.PerformanceCycles", new[] { "TradingAccountId" });
            DropTable("dbo.PerformanceCycles");
        }
    }
}
