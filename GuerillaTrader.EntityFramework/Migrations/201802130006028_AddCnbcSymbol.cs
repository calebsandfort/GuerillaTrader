namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCnbcSymbol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Markets", "CnbcSymbol", c => c.String());
            AddColumn("dbo.Options", "CnbcSymbol", c => c.String());
            AddColumn("dbo.Stocks", "CnbcSymbol", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "CnbcSymbol");
            DropColumn("dbo.Options", "CnbcSymbol");
            DropColumn("dbo.Markets", "CnbcSymbol");
        }
    }
}
