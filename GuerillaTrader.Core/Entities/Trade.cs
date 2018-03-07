using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Entities.Interfaces;
using GuerillaTrader.Framework;
using System.ComponentModel;

namespace GuerillaTrader.Entities
{
    [Table("Trades")]
    public class Trade : EntityBase, IReconciliable
    {
        public int RefNumber { get; set; }

        [ForeignKey("MarketId")]
        public virtual Market Market { get; set; }
        public virtual int? MarketId { get; set; }

        [ForeignKey("StockId")]
        public virtual Stock Stock { get; set; }
        public virtual int? StockId { get; set; }

        [ForeignKey("CoveredCallOptionId")]
        [InverseProperty("CoveredCallTrades")]
        public virtual Option CoveredCallOption { get; set; }
        public virtual int? CoveredCallOptionId { get; set; }

        [ForeignKey("BullPutSpreadShortOptionId")]
        [InverseProperty("BullPutSpreadShortTrades")]
        public virtual Option BullPutSpreadShortOption { get; set; }
        public virtual int? BullPutSpreadShortOptionId { get; set; }

        [ForeignKey("BullPutSpreadLongOptionId")]
        [InverseProperty("BullPutSpreadLongTrades")]
        public virtual Option BullPutSpreadLongOption { get; set; }
        public virtual int? BullPutSpreadLongOptionId { get; set; }

        public int Timeframe { get; set; }
        public TradeTypes TradeType { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EntryDate { get; set; }

        [DataType(DataType.Currency)]
        public Decimal EntryPrice { get; set; }
        public TradingSetups EntrySetups { get; set; }
        public String EntryRemarks { get; set; }

        //[DataType(DataType.Currency)]
        //public Decimal MFE { get; set; }

        //[DataType(DataType.Currency)]
        //public Decimal MFA { get; set; }

        [DataType(DataType.Currency)]
        public Decimal Mark { get; set; }

        [DataType(DataType.Currency)]
        public Decimal StopLossPrice { get; set; }
        [DataType(DataType.Currency)]
        public Decimal ProfitTakerPrice { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ExitDate { get; set; }

        [DataType(DataType.Currency)]
        public Decimal ExitPrice { get; set; }
        public TradeExitReasons ExitReason { get; set; }
        public String ExitRemarks { get; set; }

        [DataType(DataType.Currency)]
        public Decimal Commissions { get; set; }

        public TradeClassifications Classification { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Profit/Loss")]
        public Decimal ProfitLoss { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Adj Profit/Loss")]
        public Decimal AdjProfitLoss { get; set; }

        public int Size { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Profit/Loss Per Contract")]
        public Decimal ProfitLossPerContract { get; set; }

        #region ML Fields
        public TradeTriggers Trigger { get; set; }
        public TrendTypes Trend { get; set; }
        public bool BracketGood { get; set; }
        public Decimal SmaDiff { get; set; }
        public Decimal ATR { get; set; }
        #endregion

        [ForeignKey("TradingAccountId")]
        public virtual TradingAccount TradingAccount { get; set; }
        public virtual int TradingAccountId { get; set; }

        [ForeignKey("EntryScreenshotDbId")]
        [InverseProperty("EntryTrades")]
        public virtual Screenshot EntryScreenshotDb { get; set; }
        public virtual int? EntryScreenshotDbId { get; set; }

        [ForeignKey("ExitScreenshotDbId")]
        [InverseProperty("ExitTrades")]
        public virtual Screenshot ExitScreenshotDb { get; set; }
        public virtual int? ExitScreenshotDbId { get; set; }

        [NotMapped]
        public Decimal AdjEntryPrice
        {
            get
            {
                Decimal adjEntryPrice = this.EntryPrice;

                if (this.Market.Demoninator > 0)
                {
                    adjEntryPrice = this.EntryPrice.FromFraction(this.Market.Demoninator);
                }

                return adjEntryPrice;
            }
        }

        [NotMapped]
        public Decimal AdjMark
        {
            get
            {
                Decimal adjMarkPrice = this.Mark;

                if (this.Market.Demoninator > 0)
                {
                    adjMarkPrice = this.Mark.FromFraction(this.Market.Demoninator);
                }

                return adjMarkPrice;
            }
        }

        [NotMapped]
        public Decimal AdjExitPrice
        {
            get
            {
                Decimal adjExitPrice = this.ExitPrice;

                if (this.Market.Demoninator > 0)
                {
                    adjExitPrice = this.ExitPrice.FromFraction(this.Market.Demoninator);
                }

                return adjExitPrice;
            }
        }

        public void Reconcile()
        {
            if(this.Commissions == 0m) this.Commissions = this.Size * 6.15m;

            switch (this.TradeType)
            {
                case TradeTypes.LongFuture:
                    this.ProfitLoss = this.Size * ((((this.AdjExitPrice - this.AdjEntryPrice) / this.Market.TickSize) * this.Market.TickValue));
                    this.ProfitLossPerContract = this.ProfitLoss / this.Size;
                    break;
                case TradeTypes.ShortFuture:
                    this.ProfitLoss = this.Size * ((((this.AdjEntryPrice - this.AdjExitPrice) / this.Market.TickSize) * this.Market.TickValue));
                    this.ProfitLossPerContract = this.ProfitLoss / this.Size;
                    break;
                case TradeTypes.CoveredCall:
                    if (this.IsNew)
                    {
                        this.ProfitLoss = 0;
                    }
                    else
                    {
                        this.ProfitLoss = this.Size * (this.Mark - this.EntryPrice);
                    }

                    this.ProfitLossPerContract = this.ProfitLoss / (this.Size / 100);
                    break;
                case TradeTypes.BullPutSpread:
                    if (this.IsNew)
                    {
                        this.ProfitLoss = 0;
                    }
                    else
                    {
                        this.ProfitLoss = this.Size * 100 * (this.EntryPrice - this.Mark);
                    }

                    this.ProfitLossPerContract = this.ProfitLoss / this.Size;
                    break;
            }

            this.AdjProfitLoss = this.ProfitLoss - this.Commissions;
            
        }
    }
}
