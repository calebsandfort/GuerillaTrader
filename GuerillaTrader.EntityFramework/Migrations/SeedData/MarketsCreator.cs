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
    public class MarketsCreator
    {
        private readonly GuerillaTraderDbContext _context;

        public MarketsCreator(GuerillaTraderDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            if (_context.Markets.Count() == 0)
            {
                _context.Markets.AddOrUpdate(
                            x => x.Symbol,
                            new Market { Name = "E-Mini NASDAQ 100", Symbol = "NQ", TickSize = .25m, TickValue = 5, InitialMargin = 4290, MTT = 4, Active = true },
                            new Market { Name = "E-Mini S&P 500", Symbol = "ES", TickSize = .25m, TickValue = 12.50m, InitialMargin = 4620, MTT = 10, Active = true },
                            new Market { Name = "E-Mini Dow", Symbol = "YM", TickSize = 1, TickValue = 5, InitialMargin = 3905, MTT = 4, Active = false },
                            new Market { Name = "Gold", Symbol = "GC", TickSize = .10m, TickValue = 10, InitialMargin = 4345, MTT = 4, Active = false },
                            new Market { Name = "Oil", Symbol = "CL", TickSize = .01m, TickValue = 10, InitialMargin = 2750, MTT = 4, Active = true }
                        );

                _context.SaveChanges(); 
            }
        }
    }
}
