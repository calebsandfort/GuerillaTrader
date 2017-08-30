namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreMarketFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Markets", "AverageRange", c => c.Int(nullable: false));
            AddColumn("dbo.Markets", "Demoninator", c => c.Decimal(nullable: false, precision: 18, scale: 6));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Markets", "Demoninator");
            DropColumn("dbo.Markets", "AverageRange");
        }
    }
}
