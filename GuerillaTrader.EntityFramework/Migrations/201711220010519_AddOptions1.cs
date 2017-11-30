namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOptions1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Options",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Expiry = c.DateTime(nullable: false),
                        Strike = c.Decimal(nullable: false, precision: 18, scale: 7),
                        OptionType = c.Int(nullable: false),
                        Name = c.String(),
                        Symbol = c.String(),
                        TickValue = c.Decimal(nullable: false, precision: 18, scale: 7),
                        TickSize = c.Decimal(nullable: false, precision: 18, scale: 7),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Options");
        }
    }
}
