namespace GuerillaTrader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreDecimalPlaces : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Markets", "TickValue", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.Markets", "TickSize", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.Markets", "InitialMargin", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.Trades", "EntryPrice", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.Trades", "StopLossPrice", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.Trades", "ProfitTakerPrice", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.Trades", "ExitPrice", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.Trades", "Commissions", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.Trades", "ProfitLoss", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.Trades", "ProfitLossPerContract", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.TradingAccounts", "InitialCapital", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.TradingAccounts", "CurrentCapital", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.TradingAccounts", "Commissions", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.TradingAccounts", "ProfitLoss", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.MonteCarloSimulations", "CumulativeProfitK", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.MonteCarloSimulations", "CumulativeProfit", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.MonteCarloSimulations", "ConsecutiveLossesK", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.MonteCarloSimulations", "MaxDrawdownK", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.MonteCarloSimulations", "MaxDrawdown", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.MonteCarloSimulations", "AccountSize", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.MonteCarloSimulations", "RuinPoint", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.MonteCarloSimulations", "MaxDrawdownMultiple", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.MonteCarloSimulations", "OneContractFunds", c => c.Decimal(nullable: false, precision: 18, scale: 6));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MonteCarloSimulations", "OneContractFunds", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MonteCarloSimulations", "MaxDrawdownMultiple", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MonteCarloSimulations", "RuinPoint", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MonteCarloSimulations", "AccountSize", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MonteCarloSimulations", "MaxDrawdown", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MonteCarloSimulations", "MaxDrawdownK", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MonteCarloSimulations", "ConsecutiveLossesK", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MonteCarloSimulations", "CumulativeProfit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MonteCarloSimulations", "CumulativeProfitK", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.TradingAccounts", "ProfitLoss", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.TradingAccounts", "Commissions", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.TradingAccounts", "CurrentCapital", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.TradingAccounts", "InitialCapital", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Trades", "ProfitLossPerContract", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Trades", "ProfitLoss", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Trades", "Commissions", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Trades", "ExitPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Trades", "ProfitTakerPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Trades", "StopLossPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Trades", "EntryPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Markets", "InitialMargin", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Markets", "TickSize", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Markets", "TickValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
