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
                    new Market { Name = "E-mini S&P 500 Index ", QtSymbol = "ES", Symbol = "ES", TickSize = 0.25m, TickValue = 12.50m, InitialMargin = 4620.00m, MTT = 8, TradingStartTime = "07:30:00", TradingEndTime = "13:00:00" },
                    new Market { Name = "10-Year U.S. Treasury Note ", QtSymbol = "TY", Symbol = "ZN", TickSize = 0.015625m, TickValue = 15.625m, InitialMargin = 1265.00m, MTT = 15, TradingStartTime = "07:00:00", TradingEndTime = "12:00:00" },
                    new Market { Name = "5-Year U.S. Treasury Note ", QtSymbol = "FV", Symbol = "ZF", TickSize = 0.0078125m, TickValue = 7.8125m, InitialMargin = 687.50m, MTT = 15, TradingStartTime = "07:00:00", TradingEndTime = "12:00:00" },
                    new Market { Name = "E-mini Nasdaq 100 Index ", QtSymbol = "NQ", Symbol = "NQ", TickSize = 0.25m, TickValue = 5.00m, InitialMargin = 4290.00m, MTT = 3, Active = true, TradingStartTime = "07:30:00", TradingEndTime = "13:00:00" },
                    new Market { Name = "2-Year U.S. Treasury Note ", QtSymbol = "TU", Symbol = "ZT", TickSize = 0.0078125m, TickValue = 15.625m, InitialMargin = 473.00m, MTT = 60, TradingStartTime = "07:00:00", TradingEndTime = "12:00:00" },
                    new Market { Name = "30-Year U.S. Treasury Bond ", QtSymbol = "US", Symbol = "ZB", TickSize = 0.03125m, TickValue = 31.25m, InitialMargin = 3300.00m, MTT = 20, TradingStartTime = "07:00:00", TradingEndTime = "12:00:00" },
                    new Market { Name = "Euro FX ", QtSymbol = "EC", Symbol = "6E", TickSize = 0.000050m, TickValue = 6.25m, InitialMargin = 2310.00m, Active = true, MTT = 3, TradingStartTime = "07:00:00", TradingEndTime = "12:00:00" },
                    new Market { Name = "Japanese Yen ", QtSymbol = "JY", Symbol = "6J", TickSize = 0.00000050m, TickValue = 6.25m, InitialMargin = 2750.00m, MTT = 5, TradingStartTime = "07:00:00", TradingEndTime = "12:00:00" },
                    //new Market { Name = "Eurodollar ", QtSymbol = "", Symbol = "GE", TickSize = 0.005m, TickValue = 6.25m, InitialMargin = 198.00m, MTT = 240, TradingStartTime = "07:00:00", TradingEndTime = "12:00:00" },
                    new Market { Name = "Natural Gas ", QtSymbol = "NG", Symbol = "NG", TickSize = 0.001m, TickValue = 10.00m, InitialMargin = 1760.00m, MTT = 4, TradingStartTime = "07:00:00", TradingEndTime = "11:30:00" },
                    new Market { Name = "Soybean ", QtSymbol = "SY", Symbol = "ZS", TickSize = 0.25m, TickValue = 12.50m, InitialMargin = 2090.00m, MTT = 5, TradingStartTime = "07:00:00", TradingEndTime = "11:15:00" },
                    new Market { Name = "Mini Dow Jones Industrial Average ", QtSymbol = "YM", Symbol = "YM", TickSize = 1.0m, TickValue = 5.00m, InitialMargin = 3685.00m, MTT = 5, TradingStartTime = "07:30:00", TradingEndTime = "13:00:00" },
                    //new Market { Name = "Russell 2000 Index Mini ", QtSymbol = "", Symbol = "TF", TickSize = 0.1m, TickValue = 5m, InitialMargin = 3685m, MTT = 5, TradingStartTime = "07:30:00", TradingEndTime = "13:00:00" },
                    new Market { Name = "British Pound ", QtSymbol = "BP", Symbol = "6B", TickSize = 0.00010m, TickValue = 6.25m, InitialMargin = 2310.00m, MTT = 8, TradingStartTime = "07:00:00", TradingEndTime = "12:00:00" },
                    new Market { Name = "Australian Dollar ", QtSymbol = "AD", Symbol = "6A", TickSize = 0.00010m, TickValue = 10.00m, InitialMargin = 1595.00m, MTT = 10, TradingStartTime = "07:00:00", TradingEndTime = "12:00:00" },
                    new Market { Name = "Ultra T-Bond ", QtSymbol = "UB", Symbol = "UB", TickSize = 0.03125m, TickValue = 31.25m, Active = true, InitialMargin = 4070.00m, Demoninator = 320m, MTT = 10, TradingStartTime = "07:00:00", TradingEndTime = "12:00:00" },
                    new Market { Name = "Ultra 10-Year U.S. Treasury Note ", QtSymbol = "", Symbol = "TN", TickSize = 0.015625m, TickValue = 15.625m, InitialMargin = 1650m, MTT = 20, TradingStartTime = "07:00:00", TradingEndTime = "12:00:00" },
                    new Market { Name = "Copper ", QtSymbol = "HG", Symbol = "HG", TickSize = 0.00050m, TickValue = 12.50m, InitialMargin = 3025.00m, MTT = 5, TradingStartTime = "07:00:00", TradingEndTime = "10:00:00" },
                    new Market { Name = "Silver ", QtSymbol = "SV", Symbol = "SI", TickSize = 0.005m, TickValue = 25.00m, InitialMargin = 5940.00m, Active = true, MTT = 10, TradingStartTime = "07:00:00", TradingEndTime = "10:25:00" },
                    new Market { Name = "Canadian Dollar ", QtSymbol = "CD", Symbol = "6C", TickSize = 0.000050m, TickValue = 10.00m, InitialMargin = 1045.00m, Active = true, MTT = 5, TradingStartTime = "07:00:00", TradingEndTime = "12:00:00" },
                    new Market { Name = "Wheat ", QtSymbol = "WC", Symbol = "ZW", TickSize = 0.25m, TickValue = 12.50m, InitialMargin = 1320.00m, MTT = 15, TradingStartTime = "07:00:00", TradingEndTime = "11:15:00" },
                    new Market { Name = "Soybean Oil ", QtSymbol = "BO", Symbol = "ZL", TickSize = 0.01m, TickValue = 6m, InitialMargin = 935m, MTT = 8, TradingStartTime = "07:00:00", TradingEndTime = "11:15:00" },
                    new Market { Name = "Sugar No. 11 ", QtSymbol = "SB", Symbol = "SB", TickSize = 0.01m, TickValue = 11.20m, InitialMargin = 1232m, MTT = 10, TradingStartTime = "07:00:00", TradingEndTime = "10:30:00" },
                    new Market { Name = "Soybean Meal ", QtSymbol = "SM", Symbol = "ZM", TickSize = 0.1m, TickValue = 10m, InitialMargin = 1980m, MTT = 10, TradingStartTime = "07:00:00", TradingEndTime = "11:15:00" },
                    new Market { Name = "Mexican Peso ", QtSymbol = "ME", Symbol = "6M", TickSize = 0.000010m, TickValue = 5.00m, InitialMargin = 1485.00m, MTT = 10, TradingStartTime = "07:00:00", TradingEndTime = "12:00:00" },
                    new Market { Name = "Swiss Franc ", QtSymbol = "SF", Symbol = "6S", TickSize = 0.00010m, TickValue = 12.50m, InitialMargin = 2970m, MTT = 10, TradingStartTime = "07:00:00", TradingEndTime = "12:00:00" },
                    new Market { Name = "Cocoa ", QtSymbol = "", Symbol = "CC", TickSize = 1.0m, TickValue = 10m, InitialMargin = 15.95m, MTT = 10, TradingStartTime = "07:00:00", TradingEndTime = "08:50:00" },
                    new Market { Name = "US Dollar Index ", QtSymbol = "", Symbol = "DX", TickSize = 0.005m, TickValue = 5m, InitialMargin = 1980m, MTT = 10, TradingStartTime = "07:00:00", TradingEndTime = "12:00:00" },
                    new Market { Name = "Live Cattle ", QtSymbol = "LC", Symbol = "LE", TickSize = 0.025m, TickValue = 10.00m, InitialMargin = 1925.00m, MTT = 5, TradingStartTime = "07:00:00", TradingEndTime = "11:00:00" },
                    new Market { Name = "Coffee 'C' ", QtSymbol = "", Symbol = "KC", TickSize = 0.05m, TickValue = 18.75m, InitialMargin = 2970m, MTT = 8, TradingStartTime = "07:00:00", TradingEndTime = "10:30:00" },
                    new Market { Name = "New Zealand Dollar ", QtSymbol = "NZ", Symbol = "6N", TickSize = 0.00010m, TickValue = 10m, InitialMargin = 1430m, MTT = 15, TradingStartTime = "07:00:00", TradingEndTime = "12:00:00" },
                    new Market { Name = "Cotton No. 2 ", QtSymbol = "", Symbol = "CT", TickSize = 0.01m, TickValue = 5m, InitialMargin = 2200m, MTT = 5, TradingStartTime = "07:00:00", TradingEndTime = "11:15:00" },
                    new Market { Name = "Lean Hog ", QtSymbol = "LH", Symbol = "HE", TickSize = 0.025m, TickValue = 10.00m, InitialMargin = 1320.00m, MTT = 8, TradingStartTime = "07:30:00", TradingEndTime = "11:00:00" },
                    //new Market { Name = "Thirty-Day Fed Funds ", QtSymbol = "", Symbol = "ZQ", TickSize = 0.005m, TickValue = 10.4175m, InitialMargin = 187.00m, MTT = 240, TradingStartTime = "07:00:00", TradingEndTime = "12:00:00" },
                    new Market { Name = "FCOJ-A ", QtSymbol = "", Symbol = "OJ", TickSize = 0.05m, TickValue = 7.50m, InitialMargin = 1760m, MTT = 20, TradingStartTime = "07:00:00", TradingEndTime = "11:00:00" },
                    new Market { Name = "Gold", QtSymbol = "GC", Symbol = "GC", TickSize = .10m, TickValue = 10, InitialMargin = 4345, MTT = 4, Active = true, TradingStartTime = "07:00:00", TradingEndTime = "10:30:00" },
                    new Market { Name = "Oil", QtSymbol = "CL", Symbol = "CL", TickSize = .01m, TickValue = 10, InitialMargin = 2750, MTT = 4, Active = true, TradingStartTime = "07:00:00", TradingEndTime = "11:30:00" }
                    );

                _context.SaveChanges();
            }
        }
    }
}
