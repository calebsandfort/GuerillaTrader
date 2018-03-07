namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTradeSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TradeSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TickValue = c.Decimal(nullable: false, precision: 18, scale: 7),
                        Contracts = c.Int(nullable: false),
                        RewardTicks = c.Int(nullable: false),
                        RiskTicks = c.Int(nullable: false),
                        RoundTripCommissions = c.Decimal(nullable: false, precision: 18, scale: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TradeSettings");
        }
    }
}
