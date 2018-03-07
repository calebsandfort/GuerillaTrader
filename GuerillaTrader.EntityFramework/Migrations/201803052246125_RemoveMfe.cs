namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveMfe : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Trades", "MFE");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trades", "MFE", c => c.Decimal(nullable: false, precision: 18, scale: 7));
        }
    }
}
