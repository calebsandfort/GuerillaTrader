namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPerformanceCycleEntity2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerformanceCycles", "Display", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerformanceCycles", "Display");
        }
    }
}
