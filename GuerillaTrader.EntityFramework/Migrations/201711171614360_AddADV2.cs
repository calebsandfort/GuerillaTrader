namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddADV2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Stocks", "ADV", c => c.Decimal(precision: 18, scale: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stocks", "ADV", c => c.Int());
        }
    }
}
