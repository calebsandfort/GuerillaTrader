namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trades", "MFE", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AlterColumn("dbo.Trades", "StopLossPrice", c => c.Decimal(nullable: false, precision: 18, scale: 7, defaultValueSql: "0"));
            AlterColumn("dbo.Trades", "ProfitTakerPrice", c => c.Decimal(nullable: false, precision: 18, scale: 7, defaultValueSql: "0"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trades", "MFE");
        }
    }
}
