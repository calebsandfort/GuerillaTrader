namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveStockTaxFields : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Stocks", "TaxRatePaid");
            DropColumn("dbo.Stocks", "EffectiveTaxRate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stocks", "EffectiveTaxRate", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Stocks", "TaxRatePaid", c => c.Decimal(nullable: false, precision: 18, scale: 7));
        }
    }
}
