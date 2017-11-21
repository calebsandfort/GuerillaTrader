namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStockImportantDateFields3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "ExDividendDate", c => c.DateTime());
            DropColumn("dbo.Stocks", "ExDividendsDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stocks", "ExDividendsDate", c => c.DateTime());
            DropColumn("dbo.Stocks", "ExDividendDate");
        }
    }
}
