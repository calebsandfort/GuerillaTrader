using Abp.AutoMapper;
using CsvHelper.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace GuerillaTrader.Entities.Dtos
{
    [AutoMap(typeof(Stock))]
    public class PfStockDto
    {
        public String Name { get; set; }
        public String Symbol { get; set; }
        public Decimal Yield { get; set; }

        public int DividendYieldScore { get; set; }
        public int CashFlowScore { get; set; }
        public int RelativeValueScore { get; set; }
        public int TotalScore { get; set; }

        [DataType(DataType.Currency)]
        public Decimal Price { get; set; }
        [DataType(DataType.Currency)]
        public Decimal IdealValue { get; set; }

        public String Sector { get; set; }
    }

    public sealed class PfStockDtoMap : CsvClassMap<PfStockDto>
    {
        public PfStockDtoMap()
        {
            Map(m => m.Name).Name("Security");
            Map(m => m.Symbol).Name("Ticker");
            Map(m => m.Yield).Name("Yield (%)").Default(0m).ConvertUsing(row =>
            {
                decimal amount = 0.0m;
                decimal.TryParse(row.GetField<string>("Yield (%)"), NumberStyles.Any,
                    CultureInfo.InvariantCulture, out amount);
                return amount / 100m;
            });
            Map(m => m.DividendYieldScore).Name("Dividend Yield Score");
            Map(m => m.CashFlowScore).Name("Cash Flow Score");
            Map(m => m.RelativeValueScore).Name("Relative Value Score");
            Map(m => m.TotalScore).Name("Total Score");
            Map(m => m.Price).Name("Price (USD)1").Default(0m);
            Map(m => m.IdealValue).Name("IDEAL Value ($)").Default(0m);
            Map(m => m.Sector).Name("GICS Sector");
        }
    }
}
