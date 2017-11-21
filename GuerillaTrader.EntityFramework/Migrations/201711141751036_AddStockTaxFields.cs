namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStockTaxFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "TaxRatePaid", c => c.Decimal(nullable: false, precision: 18, scale: 7, defaultValueSql: "-2"));
            AddColumn("dbo.Stocks", "EffectiveTaxRate", c => c.Decimal(nullable: false, precision: 18, scale: 7, defaultValueSql: "-2"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "EffectiveTaxRate");
            DropColumn("dbo.Stocks", "TaxRatePaid");
        }
    }
}
