using Abp.AutoMapper;
using CsvHelper.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace GuerillaTrader.Entities.Dtos
{
    [AutoMap(typeof(Stock))]
    public class TaxRateStockDto
    {
        public String CompanyName { get; set; }
        public Decimal TaxRatePaid { get; set; }
        public Decimal EffectiveTaxRate { get; set; }
    }

    public sealed class TaxRateStockDtoMap : CsvClassMap<TaxRateStockDto>
    {
        public TaxRateStockDtoMap()
        {
            Map(m => m.CompanyName).Name("Company Name");
            Map(m => m.TaxRatePaid).Name("U.S. Tax Rate Paid").Default(0m).ConvertUsing(row =>
            {
                String val = row.GetField<string>("U.S. Tax Rate Paid");
                if (val == "Negative" || val == "Tax-Free REIT" || val == "Exempt" || val == "Refund") return -1m;

                decimal rate = 0.0m;
                decimal.TryParse(val, NumberStyles.Any,
                    CultureInfo.InvariantCulture, out rate);
                return rate / 100m;
            });
            Map(m => m.EffectiveTaxRate).Name("Effective Tax Rate").Default(0m).ConvertUsing(row =>
            {
                String val = row.GetField<string>("Effective Tax Rate");
                if (val == "Negative" || val == "Tax-Free REIT" || val == "Exempt" || val == "Refund") return -1m;

                decimal rate = 0.0m;
                decimal.TryParse(val, NumberStyles.Any,
                    CultureInfo.InvariantCulture, out rate);
                return rate / 100m;
            });
        }
    }
}
