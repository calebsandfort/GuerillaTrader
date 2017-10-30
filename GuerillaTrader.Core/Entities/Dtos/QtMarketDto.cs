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
    [AutoMap(typeof(Market))]
    public class QtMarketDto
    {
        public String QtSymbol { get; set; }
        public Decimal QtDailyVolume { get; set; }
        public Decimal QtDailyWave { get; set; }
        public Decimal QtRSquared { get; set; }
        public Decimal QtVolumeScore { get; set; }
        public Decimal QtWaveScore { get; set; }
        public Decimal QtRSquaredScore { get; set; }
        public Decimal QtCompositeScore { get; set; }
    }

    public sealed class QtMarketDtoMap : CsvClassMap<QtMarketDto>
    {
        public QtMarketDtoMap()
        {
            Map(m => m.QtSymbol).Name("Symbol");
            Map(m => m.QtDailyVolume).Name("Volume");
            Map(m => m.QtDailyWave).Name("Wave");
            Map(m => m.QtRSquared).Name("RSquared");
        }
    }
}
