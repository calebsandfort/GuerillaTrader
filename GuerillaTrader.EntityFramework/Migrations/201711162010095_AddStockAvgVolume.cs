namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStockAvgVolume : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "AvgVolume", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "AvgVolume");
        }
    }
}
