namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFailedToRetrieveBars2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "FailedToRetrieveBars", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "FailedToRetrieveBars");
        }
    }
}
