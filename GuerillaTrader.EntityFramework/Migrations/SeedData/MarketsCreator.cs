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
                    new Market { Name = "E-mini S&P 500 Index ", Symbol = "ES", TickSize = 0.25m, TickValue = 12.50m, InitialMargin = 4620.00m, MTT = 10},
                    new Market { Name = "10-Year U.S. Treasury Note ", Symbol = "ZN", TickSize = 0.015625m, TickValue = 15.625m, InitialMargin = 1265.00m, MTT = 15},
                    new Market { Name = "5-Year U.S. Treasury Note ", Symbol = "ZF", TickSize = 0.0078125m, TickValue = 7.8125m, InitialMargin = 687.50m, MTT = 0},
                    new Market { Name = "E-mini Nasdaq 100 Index ", Symbol = "NQ", TickSize = 0.25m, TickValue = 5.00m, InitialMargin = 4290.00m, MTT = 4},
                    new Market { Name = "2-Year U.S. Treasury Note ", Symbol = "ZT", TickSize = 0.0078125m, TickValue = 15.625m, InitialMargin = 473.00m, MTT = 0},
                    new Market { Name = "30-Year U.S. Treasury Bond ", Symbol = "ZB", TickSize = 0.03125m, TickValue = 31.25m, InitialMargin = 3300.00m, MTT = 0},
                    new Market { Name = "Euro FX ", Symbol = "6E", TickSize = 0.000050m, TickValue = 6.25m, InitialMargin = 2310.00m, MTT = 5, Active = true },
                    new Market { Name = "Japanese Yen ", Symbol = "6J", TickSize = 0.00000050m, TickValue = 6.25m, InitialMargin = 2750.00m, MTT = 0},
                    new Market { Name = "Eurodollar ", Symbol = "GE", TickSize = 0.005m, TickValue = 6.25m, InitialMargin = 198.00m, MTT = 0},
                    new Market { Name = "Natural Gas ", Symbol = "NG", TickSize = 0.001m, TickValue = 10.00m, InitialMargin = 1760.00m, MTT = 0},
                    new Market { Name = "Soybean ", Symbol = "ZS", TickSize = 0.25m, TickValue = 12.50m, InitialMargin = 2090.00m, MTT = 0},
                    new Market { Name = "Mini Dow Jones Industrial Average ", Symbol = "YM", TickSize = 1.0m, TickValue = 5.00m, InitialMargin = 3685.00m, MTT = 4},
                    new Market { Name = "Russell 2000 Index Mini ", Symbol = "TF", TickSize = 0.1m, TickValue = 5m, InitialMargin = 3685m, MTT = 0},
                    new Market { Name = "British Pound ", Symbol = "6B", TickSize = 0.00010m, TickValue = 6.25m, InitialMargin = 2310.00m, MTT = 0},
                    new Market { Name = "Australian Dollar ", Symbol = "6A", TickSize = 0.00010m, TickValue = 10.00m, InitialMargin = 1595.00m, MTT = 0},
                    new Market { Name = "Ultra T-Bond ", Symbol = "UB", TickSize = 0.03125m, TickValue = 31.25m, InitialMargin = 4070.00m, MTT = 0},
                    new Market { Name = "Ultra 10-Year U.S. Treasury Note ", Symbol = "TN", TickSize = 0.015625m, TickValue = 15.625m, InitialMargin = 1650m, MTT = 0},
                    new Market { Name = "Copper ", Symbol = "HG", TickSize = 0.00050m, TickValue = 12.50m, InitialMargin = 3025.00m, MTT = 0},
                    new Market { Name = "Silver ", Symbol = "SI", TickSize = 0.005m, TickValue = 25.00m, InitialMargin = 5940.00m, MTT = 0},
                    new Market { Name = "Canadian Dollar ", Symbol = "6C", TickSize = 0.000050m, TickValue = 10.00m, InitialMargin = 1045.00m, MTT = 0},
                    new Market { Name = "Wheat ", Symbol = "ZW", TickSize = 0.25m, TickValue = 12.50m, InitialMargin = 1320.00m, MTT = 0},
                    new Market { Name = "Soybean Oil ", Symbol = "ZL", TickSize = 0.01m, TickValue = 6m, InitialMargin = 935m, MTT = 0},
                    new Market { Name = "Sugar No. 11 ", Symbol = "SB", TickSize = 0.01m, TickValue = 11.20m, InitialMargin = 1232m, MTT = 0},
                    new Market { Name = "Soybean Meal ", Symbol = "ZM", TickSize = 0.1m, TickValue = 10m, InitialMargin = 1980m, MTT = 0},
                    new Market { Name = "Mexican Peso ", Symbol = "6M", TickSize = 0.000010m, TickValue = 5.00m, InitialMargin = 1485.00m, MTT = 0},
                    new Market { Name = "Swiss Franc ", Symbol = "6S", TickSize = 0.00010m, TickValue = 12.50m, InitialMargin = 2970m, MTT = 0},
                    new Market { Name = "Cocoa ", Symbol = "CC", TickSize = 1.0m, TickValue = 10m, InitialMargin = 15.95m, MTT = 0},
                    new Market { Name = "US Dollar Index ", Symbol = "DX", TickSize = 0.005m, TickValue = 5m, InitialMargin = 1980m, MTT = 0},
                    new Market { Name = "Live Cattle ", Symbol = "LE", TickSize = 0.025m, TickValue = 10.00m, InitialMargin = 1925.00m, MTT = 0},
                    new Market { Name = "Coffee 'C' ", Symbol = "KC", TickSize = 0.05m, TickValue = 18.75m, InitialMargin = 2970m, MTT = 0},
                    new Market { Name = "New Zealand Dollar ", Symbol = "6N", TickSize = 0.00010m, TickValue = 10m, InitialMargin = 1430m, MTT = 0},
                    new Market { Name = "Cotton No. 2 ", Symbol = "CT", TickSize = 0.01m, TickValue = 5m, InitialMargin = 2200m, MTT = 0},
                    new Market { Name = "Lean Hog ", Symbol = "HE", TickSize = 0.025m, TickValue = 10.00m, InitialMargin = 1320.00m, MTT = 0},
                    new Market { Name = "Thirty-Day Fed Funds ", Symbol = "ZQ", TickSize = 0.005m, TickValue = 10.4175m, InitialMargin = 187.00m, MTT = 0},
                    new Market { Name = "FCOJ-A ", Symbol = "OJ", TickSize = 0.05m, TickValue = 7.50m, InitialMargin = 1760m, MTT = 0 },
                    new Market { Name = "Gold", Symbol = "GC", TickSize = .10m, TickValue = 10, InitialMargin = 4345, MTT = 4 },
                    new Market { Name = "Oil", Symbol = "CL", TickSize = .01m, TickValue = 10, InitialMargin = 2750, MTT = 4 }
                    );

                _context.SaveChanges(); 
            }
        }
    }
}
