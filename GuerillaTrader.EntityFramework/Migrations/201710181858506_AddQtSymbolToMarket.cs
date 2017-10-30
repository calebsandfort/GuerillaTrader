namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQtSymbolToMarket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Markets", "QtSymbol", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Markets", "QtSymbol");
        }
    }
}
