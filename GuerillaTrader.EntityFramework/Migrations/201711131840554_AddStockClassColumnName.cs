namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStockClassColumnName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "CashFlowScore", c => c.Int(nullable: false));
            DropColumn("dbo.Stocks", "ChasFlowScore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stocks", "ChasFlowScore", c => c.Int(nullable: false));
            DropColumn("dbo.Stocks", "CashFlowScore");
        }
    }
}
