using Abp.AutoMapper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Entities.Dtos
{
    public class TosMarketDto : EntityDtoBase
    {
        public String Name { get; set; }
        public String Symbol { get; set; }

        public Decimal TickValue { get; set; }
        public Decimal TickSize { get; set; }
        public Decimal InitialMargin { get; set; }

        public Decimal DailyVolume { get; set; }
        public Decimal DailyWave { get; set; }
        public Decimal VolumeScore { get; set; }
        public Decimal WaveScore { get; set; }
        public Decimal CompositeScore { get; set; }
    }

    public sealed class TosMarketDtoMap : CsvClassMap<TosMarketDto>
    {
        public TosMarketDtoMap()
        {
            Map(m => m.Symbol).ConvertUsing(row =>
            {
                String symbol = row.GetField<string>("Symbol").Replace("/", String.Empty);
                symbol = symbol.Substring(0, symbol.IndexOf("[", StringComparison.CurrentCulture));
                return symbol;
            });
            Map(m => m.Name).Name("Description");
            Map(m => m.Name).ConvertUsing(row =>
            {
                String name = row.GetField<string>("Description");
                name = name.Substring(0, name.IndexOf("Future", StringComparison.CurrentCulture));
                return name;
            });
            Map(m => m.DailyVolume).Name("SimpleMovingAvg");
            Map(m => m.TickSize).Name("TickSize");
            Map(m => m.DailyWave).Default(0.0m).ConvertUsing(row =>
            {
                decimal amount = 0.0m;
                decimal.TryParse(row.GetField<string>("Avg Tick Range"), NumberStyles.Any,
                    CultureInfo.InvariantCulture, out amount);
                return amount;
            });
        }
    }
}
