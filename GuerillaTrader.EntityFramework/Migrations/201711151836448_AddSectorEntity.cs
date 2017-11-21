namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSectorEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sectors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RecentPerf = c.Decimal(nullable: false, precision: 18, scale: 7),
                        PastPositivePerf = c.Decimal(nullable: false, precision: 18, scale: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Sectors");
        }
    }
}
