namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBlockToTrade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trades", "Block", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trades", "Block");
        }
    }
}
