namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreFieldsToPerformanceCycle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerformanceCycles", "StartCapital", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.PerformanceCycles", "EndCapital", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.PerformanceCycles", "ProfitLoss", c => c.Decimal(nullable: false, precision: 18, scale: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerformanceCycles", "ProfitLoss");
            DropColumn("dbo.PerformanceCycles", "EndCapital");
            DropColumn("dbo.PerformanceCycles", "StartCapital");
        }
    }
}
