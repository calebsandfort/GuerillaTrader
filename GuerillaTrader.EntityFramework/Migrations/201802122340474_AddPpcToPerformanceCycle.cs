namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPpcToPerformanceCycle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerformanceCycles", "PPC", c => c.Decimal(nullable: false, precision: 18, scale: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerformanceCycles", "PPC");
        }
    }
}
