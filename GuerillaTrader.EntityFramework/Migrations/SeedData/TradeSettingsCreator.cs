using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Entities;
using GuerillaTrader.EntityFramework;

namespace GuerillaTrader.Migrations.SeedData
{
    public class TradeSettingsCreator
    {
        private readonly GuerillaTraderDbContext _context;

        public TradeSettingsCreator(GuerillaTraderDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            //public String Name { get; set; }
            //public Decimal TickValue { get; set; }
            //public int Contracts { get; set; }
            //public int RewardTicks { get; set; }
            //public int RiskTicks { get; set; }
            //public Decimal RoundTripCommissions { get; set; }

            if (_context.TradeSettings.Count() == 0)
            {
                _context.TradeSettings.AddOrUpdate(
                    x => x.Name,
                    new TradeSettings { Name = "NQ - THA - Scalp ", TickValue = 5m, Contracts = 3, RewardTicks = 10, RiskTicks = 24, RoundTripCommissions = 6.88m }
                    );

                _context.SaveChanges();
            }
        }
    }
}
