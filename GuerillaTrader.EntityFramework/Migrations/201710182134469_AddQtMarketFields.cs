namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQtMarketFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Markets", "QtDailyVolume", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Markets", "QtDailyWave", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Markets", "QtRSquared", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Markets", "QtVolumeScore", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Markets", "QtWaveScore", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Markets", "QtRSquaredScore", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            AddColumn("dbo.Markets", "QtCompositeScore", c => c.Decimal(nullable: false, precision: 18, scale: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Markets", "QtCompositeScore");
            DropColumn("dbo.Markets", "QtRSquaredScore");
            DropColumn("dbo.Markets", "QtWaveScore");
            DropColumn("dbo.Markets", "QtVolumeScore");
            DropColumn("dbo.Markets", "QtRSquared");
            DropColumn("dbo.Markets", "QtDailyWave");
            DropColumn("dbo.Markets", "QtDailyVolume");
        }
    }
}
