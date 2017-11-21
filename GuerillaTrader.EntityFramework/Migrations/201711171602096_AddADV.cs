namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddADV : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "ADV", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "ADV");
        }
    }
}
