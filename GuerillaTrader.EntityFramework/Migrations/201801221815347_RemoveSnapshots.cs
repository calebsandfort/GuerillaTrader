namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSnapshots : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TradingAccountSnapshots", "TradingAccountId", "dbo.TradingAccounts");
            DropIndex("dbo.TradingAccountSnapshots", new[] { "TradingAccountId" });
            DropTable("dbo.TradingAccountSnapshots");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TradingAccountSnapshots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentCapital = c.Decimal(nullable: false, precision: 18, scale: 7),
                        Commissions = c.Decimal(nullable: false, precision: 18, scale: 7),
                        ProfitLoss = c.Decimal(nullable: false, precision: 18, scale: 7),
                        AdjProfitLoss = c.Decimal(nullable: false, precision: 18, scale: 7),
                        TotalReturn = c.Decimal(nullable: false, precision: 18, scale: 7),
                        CAGR = c.Decimal(nullable: false, precision: 18, scale: 7),
                        Date = c.DateTime(nullable: false),
                        TradingAccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.TradingAccountSnapshots", "TradingAccountId");
            AddForeignKey("dbo.TradingAccountSnapshots", "TradingAccountId", "dbo.TradingAccounts", "Id", cascadeDelete: true);
        }
    }
}
