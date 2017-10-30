using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Entities
{
    [Table("Markets")]
    public class Market : EntityBase
    {
        public String Name { get; set; }
        public String Symbol { get; set; }
        public String QtSymbol { get; set; }

        [DataType(DataType.Currency)]
        public Decimal TickValue { get; set; }
        public Decimal TickSize { get; set; }

        [DataType(DataType.Currency)]
        public Decimal InitialMargin { get; set; }

        public String TradingStartTime { get; set; }
        public String TradingEndTime { get; set; }

        public int MTT { get; set; }
        public int AverageRange { get; set; }
        public Decimal Demoninator { get; set; }

        public Decimal TosDailyVolume { get; set; }
        [DataType(DataType.Currency)]
        public Decimal TosDailyWave { get; set; }
        public Decimal TosVolumeScore { get; set; }
        public Decimal TosWaveScore { get; set; }
        public Decimal TosCompositeScore { get; set; }

        public Decimal QtDailyVolume { get; set; }
        [DataType(DataType.Currency)]
        public Decimal QtDailyWave { get; set; }
        public Decimal QtRSquared { get; set; }
        public Decimal QtVolumeScore { get; set; }
        public Decimal QtWaveScore { get; set; }
        public Decimal QtRSquaredScore { get; set; }
        public Decimal QtCompositeScore { get; set; }

        public bool Active { get; set; }

        [DataType(DataType.Currency)]
        [NotMapped]
        public Decimal PointValue
        {
            get
            {
                return 1.0m / this.TickSize * this.TickValue;
            }
        }

        [ForeignKey("MarketId")]
        public virtual ICollection<Trade> Trades { get; set; }
    }
}
