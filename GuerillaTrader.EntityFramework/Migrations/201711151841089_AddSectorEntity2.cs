namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSectorEntity2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sectors", "EffectiveTaxRate", c => c.Decimal(nullable: false, precision: 18, scale: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sectors", "EffectiveTaxRate");
        }
    }
}
