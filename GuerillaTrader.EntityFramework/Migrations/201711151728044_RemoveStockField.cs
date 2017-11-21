namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveStockField : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Stocks", "FailedToRetrieveBars");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stocks", "FailedToRetrieveBars", c => c.Boolean(nullable: false));
        }
    }
}
