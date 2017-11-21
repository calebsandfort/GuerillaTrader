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
    public class SectorsCreator
    {
        private readonly GuerillaTraderDbContext _context;

        public SectorsCreator(GuerillaTraderDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            if (_context.Sectors.Count() == 0)
            {
                _context.Sectors.AddOrUpdate(
                    x => x.Name,
                    new Sector { Name = "Consumer Discretionary", EffectiveTaxRate = .2827m },
                    new Sector { Name = "Consumer Staples", EffectiveTaxRate = .3108m },
                    new Sector { Name = "Energy", EffectiveTaxRate = 0m },
                    new Sector { Name = "Financials", EffectiveTaxRate = .2558m },
                    new Sector { Name = "Health Care", EffectiveTaxRate = .1956m },
                    new Sector { Name = "Industrials", EffectiveTaxRate = .2477m },
                    new Sector { Name = "Information Technology", EffectiveTaxRate = .1702m },
                    new Sector { Name = "Materials", EffectiveTaxRate = .2406m },
                    new Sector { Name = "Real Estate", EffectiveTaxRate = .0493m },
                    new Sector { Name = "Telecommunication Services", EffectiveTaxRate = .2816m },
                    new Sector { Name = "Utilities", EffectiveTaxRate = .2947m }
                    );

                _context.SaveChanges();
            }
        }
    }
}
