namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPerformanceCycleEntity3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerformanceCycles", "WinningTrades", c => c.Int(nullable: false));
            AddColumn("dbo.PerformanceCycles", "LosingTrades", c => c.Int(nullable: false));
            AddColumn("dbo.PerformanceCycles", "TotalTrades", c => c.Int(nullable: false));
            AddColumn("dbo.PerformanceCycles", "SuccessRate", c => c.Decimal(nullable: false, precision: 18, scale: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerformanceCycles", "SuccessRate");
            DropColumn("dbo.PerformanceCycles", "TotalTrades");
            DropColumn("dbo.PerformanceCycles", "LosingTrades");
            DropColumn("dbo.PerformanceCycles", "WinningTrades");
        }
    }
}
