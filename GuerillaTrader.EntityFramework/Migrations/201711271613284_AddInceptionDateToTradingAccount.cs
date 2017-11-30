namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInceptionDateToTradingAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradingAccounts", "InceptionDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradingAccounts", "InceptionDate");
        }
    }
}
