namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStockImportantDateFields2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "TargetPrice", c => c.Decimal(precision: 18, scale: 7));
            AlterColumn("dbo.Stocks", "NextEarningsDate", c => c.DateTime());
            AlterColumn("dbo.Stocks", "ExDividendsDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stocks", "ExDividendsDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Stocks", "NextEarningsDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Stocks", "TargetPrice");
        }
    }
}
