namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSomeMarketFieldNames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Markets", "TradingStartTime", c => c.String());
            AddColumn("dbo.Markets", "TradingEndTime", c => c.String());
            AddColumn("dbo.Markets", "TosDailyVolume", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Markets", "TosDailyWave", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Markets", "TosVolumeScore", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Markets", "TosWaveScore", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Markets", "TosCompositeScore", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            DropColumn("dbo.Markets", "DailyVolume");
            DropColumn("dbo.Markets", "DailyWave");
            DropColumn("dbo.Markets", "VolumeScore");
            DropColumn("dbo.Markets", "WaveScore");
            DropColumn("dbo.Markets", "CompositeScore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Markets", "CompositeScore", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Markets", "WaveScore", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Markets", "VolumeScore", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Markets", "DailyWave", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Markets", "DailyVolume", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            DropColumn("dbo.Markets", "TosCompositeScore");
            DropColumn("dbo.Markets", "TosWaveScore");
            DropColumn("dbo.Markets", "TosVolumeScore");
            DropColumn("dbo.Markets", "TosDailyWave");
            DropColumn("dbo.Markets", "TosDailyVolume");
            DropColumn("dbo.Markets", "TradingEndTime");
            DropColumn("dbo.Markets", "TradingStartTime");
        }
    }
}
