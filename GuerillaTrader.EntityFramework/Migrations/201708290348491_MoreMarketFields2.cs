namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreMarketFields2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Markets", "DailyVolume", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AddColumn("dbo.Markets", "DailyWave", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AddColumn("dbo.Markets", "VolumeScore", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AddColumn("dbo.Markets", "WaveScore", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AddColumn("dbo.Markets", "CompositeScore", c => c.Decimal(nullable: false, precision: 18, scale: 6));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Markets", "CompositeScore");
            DropColumn("dbo.Markets", "WaveScore");
            DropColumn("dbo.Markets", "VolumeScore");
            DropColumn("dbo.Markets", "DailyWave");
            DropColumn("dbo.Markets", "DailyVolume");
        }
    }
}
