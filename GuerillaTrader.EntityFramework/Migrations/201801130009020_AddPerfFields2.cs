namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPerfFields2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trades", "AdjProfitLoss", c => c.Decimal(nullable: false, precision: 18, scale: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trades", "AdjProfitLoss");
        }
    }
}
