namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOptionPropertiesToTrade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trades", "CoveredCallOptionId", c => c.Int());
            AddColumn("dbo.Trades", "BullPutSpreadShortOptionId", c => c.Int());
            AddColumn("dbo.Trades", "BullPutSpreadLongOptionId", c => c.Int());
            AddColumn("dbo.Trades", "Mark", c => c.Decimal(nullable: false, precision: 18, scale: 7));
            CreateIndex("dbo.Trades", "CoveredCallOptionId");
            CreateIndex("dbo.Trades", "BullPutSpreadShortOptionId");
            CreateIndex("dbo.Trades", "BullPutSpreadLongOptionId");
            AddForeignKey("dbo.Trades", "BullPutSpreadLongOptionId", "dbo.Options", "Id");
            AddForeignKey("dbo.Trades", "BullPutSpreadShortOptionId", "dbo.Options", "Id");
            AddForeignKey("dbo.Trades", "CoveredCallOptionId", "dbo.Options", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trades", "CoveredCallOptionId", "dbo.Options");
            DropForeignKey("dbo.Trades", "BullPutSpreadShortOptionId", "dbo.Options");
            DropForeignKey("dbo.Trades", "BullPutSpreadLongOptionId", "dbo.Options");
            DropIndex("dbo.Trades", new[] { "BullPutSpreadLongOptionId" });
            DropIndex("dbo.Trades", new[] { "BullPutSpreadShortOptionId" });
            DropIndex("dbo.Trades", new[] { "CoveredCallOptionId" });
            DropColumn("dbo.Trades", "Mark");
            DropColumn("dbo.Trades", "BullPutSpreadLongOptionId");
            DropColumn("dbo.Trades", "BullPutSpreadShortOptionId");
            DropColumn("dbo.Trades", "CoveredCallOptionId");
        }
    }
}
