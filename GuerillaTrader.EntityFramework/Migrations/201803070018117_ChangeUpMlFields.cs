namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUpMlFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trades", "BracketGood", c => c.Boolean(nullable: false, defaultValueSql: "0"));
            AddColumn("dbo.Trades", "SmaDiff", c => c.Decimal(nullable: false, precision: 18, scale: 7, defaultValueSql: "0"));
            AddColumn("dbo.Trades", "ATR", c => c.Decimal(nullable: false, precision: 18, scale: 7, defaultValueSql: "0"));
            DropColumn("dbo.Trades", "Volatile");
            DropColumn("dbo.Trades", "TickRange");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trades", "TickRange", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Trades", "Volatile", c => c.Boolean(nullable: false));
            DropColumn("dbo.Trades", "ATR");
            DropColumn("dbo.Trades", "SmaDiff");
            DropColumn("dbo.Trades", "BracketGood");
        }
    }
}
