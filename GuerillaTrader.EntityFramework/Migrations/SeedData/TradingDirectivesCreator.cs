using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Entities;
using GuerillaTrader.EntityFramework;

namespace GuerillaTrader.Migrations.SeedData
{
    public class TradingDirectivesCreator
    {
        private readonly GuerillaTraderDbContext _context;

        public TradingDirectivesCreator(GuerillaTraderDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            if (_context.TradingDirectives.Count() == 0)
            {
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Rule, Text = "Choose a pair of stop-loss and target level before assessing probability." });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Rule, Text = "Enter a trade only if you will not regret even if it turns out to be a loser." });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Rule, Text = "Have a directional market bias at all times" });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Rule, Text = "Trade along with the direction of the market bias." });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Rule, Text = "Draw trend lines with valid pivots only." });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Rule, Text = "Wait for session momentum to surface after a large opening gap." });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Rule, Text = "Do not adjust initial stop-loss order to assume more risk." });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Rule, Text = "Do not re-enter position for a second time." });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Rule, Text = "Classify every trade as consistent, discretionary, or rogue before you know its outcome." });

                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Guideline, Text = "Take high quality trading setups. (multiple setups, re-entry equivalent, or one setup with S/R)" });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Guideline, Text = "Trade in the direction of the momentum" });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Guideline, Text = "Consider a trend line effective until it is broken with momentum." });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Guideline, Text = "Take setup bars with lower than average range" });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Guideline, Text = "Do not use outside bars as setup bars." });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Guideline, Text = "Do not exit before the target order is hit." });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Guideline, Text = "Do not adjust target orders." });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Guideline, Text = "Do not trail stop-loss orders." });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Guideline, Text = "Treat every Congestion Zone as a support/resistance." });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Guideline, Text = "Trade an Anti-climax pattern only if it does not clear the previous swing pivot." });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Guideline, Text = "Avoid Pressure Zone Setups in congestion" });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Guideline, Text = "Do not take setups if market is within a compound Congestion Zone." });
                _context.TradingDirectives.Add(new TradingDirective() { TradingDirectiveType = TradingDirectiveTypes.Guideline, Text = "Do not trade when price is trapped between two trend lines." });
            }

            _context.SaveChanges();
        }
    }
}
