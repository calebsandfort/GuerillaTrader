namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStockImportantDateFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "NextEarningsDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Stocks", "ExDividendsDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "ExDividendsDate");
            DropColumn("dbo.Stocks", "NextEarningsDate");
        }
    }
}
