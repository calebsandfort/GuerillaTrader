namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTradeMlFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trades", "Trigger", c => c.Int(nullable: false, defaultValueSql: "0"));
            AddColumn("dbo.Trades", "Trend", c => c.Int(nullable: false, defaultValueSql: "0"));
            AddColumn("dbo.Trades", "Volatile", c => c.Boolean(nullable: false, defaultValueSql: "0"));
            AddColumn("dbo.Trades", "TickRange", c => c.Decimal(nullable: false, precision: 18, scale: 7, defaultValueSql: "0"));

            AlterColumn("dbo.Trades", "RefNumber", c => c.Int(defaultValueSql: "0"));
            AlterColumn("dbo.Trades", "Timeframe", c => c.Int(defaultValueSql: "0"));
            AlterColumn("dbo.Trades", "EntrySetups", c => c.Int(defaultValueSql: "0"));
            AlterColumn("dbo.Trades", "StopLossPrice", c => c.Int(defaultValueSql: "0"));
            AlterColumn("dbo.Trades", "ProfitTakerPrice", c => c.Int(defaultValueSql: "0"));
            AlterColumn("dbo.Trades", "ExitReason", c => c.Int(defaultValueSql: "0"));
            AlterColumn("dbo.Trades", "Classification", c => c.Int(defaultValueSql: "0"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trades", "TickRange");
            DropColumn("dbo.Trades", "Volatile");
            DropColumn("dbo.Trades", "Trend");
            DropColumn("dbo.Trades", "Trigger");
        }
    }
}
