using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Framework;

namespace GuerillaTrader.Entities.Dtos
{
    public class TradeDto : EntityDtoBase
    {
        public int RefNumber { get; set; }

        public String Market { get; set; }
        [UIHint("Market")]
        [Display(Name = "Market")]
        public int MarketId { get; set; }

        [UIHint("MyInt")]
        [Display(Name = "TF")]
        public int Timeframe { get; set; }

        [UIHint("TradeTypes")]
        [Display(Name = "Dir")]
        public TradeTypes TradeType { get; set; }

        [DataType(DataType.DateTime)]
        [UIHint("MyDateTime")]
        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        [DataType(DataType.Currency)]
        [UIHint("MyCurrency")]
        [Display(Name = "Entry")]
        public Decimal EntryPrice { get; set; }

        [UIHint("TradingSetups")]
        [Display(Name = "Entry Setups")]
        public List<int> EntrySetups { get; set; }

        public String EntrySetupsString
        {
            get
            {
                if(!this.EntrySetups.Any(x => x != 0))
                {
                    return String.Empty;
                }
                else
                {
                    return String.Join(", ", this.EntrySetups.Where(x => x != 0).Select(x => ((TradingSetups)x).GetDisplay()));
                }
            }
        }

        [DataType(DataType.MultilineText)]
        [UIHint("MyMultiline")]
        [Display(Name = "Entry Rmks")]
        public String EntryRemarks { get; set; }

        [UIHint("Screenshot")]
        [Display(Name = "Entry SS")]
        public String EntryScreenshot { get; set; }
        public bool ShowEntryScreenshot
        {
            get
            {
                return this.EntryScreenshotDbId > 0;
            }
        }

        //[DataType(DataType.Currency)]
        //public Decimal MFE { get; set; }

        //[DataType(DataType.Currency)]
        //public Decimal MFA { get; set; }

        [DataType(DataType.Currency)]
        [UIHint("MyCurrency")]
        [Display(Name = "Stop-Loss")]
        public Decimal StopLossPrice { get; set; }

        [DataType(DataType.Currency)]
        [UIHint("MyCurrency")]
        [Display(Name = "Profit Taker")]
        public Decimal ProfitTakerPrice { get; set; }

        [DataType(DataType.DateTime)]
        [UIHint("MyDateTime")]
        [Display(Name = "Exit Date")]
        public DateTime? ExitDate { get; set; }

        [DataType(DataType.Currency)]
        [UIHint("MyCurrency")]
        [Display(Name = "Exit")]
        public Decimal ExitPrice { get; set; }

        [UIHint("TradeExitReasons")]
        [Display(Name = "Exit Reason")]
        public TradeExitReasons ExitReason { get; set; }

        public String ExitReasonString
        {
            get
            {
                return this.ExitReason.GetDisplay();
            }
        }

        [UIHint("MyMultiline")]
        [Display(Name = "Exit Rmks")]
        public String ExitRemarks { get; set; }

        [UIHint("Screenshot")]
        [Display(Name = "Exit SS")]
        public String ExitScreenshot { get; set; }
        public bool ShowExitScreenshot
        {
            get
            {
                return this.ExitScreenshotDbId > 0;
            }
        }

        [DataType(DataType.Currency)]
        public Decimal Commissions { get; set; }

        [UIHint("TradeClassifications")]
        [Display(Name = "Classification")]
        public TradeClassifications Classification { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Profit/Loss")]
        public Decimal ProfitLoss { get; set; }

        [UIHint("MyInt")]
        public int Size { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Profit/Loss Per Contract")]
        public Decimal ProfitLossPerContract { get; set; }

        public String TradingAccount { get; set; }
        public int TradingAccountId { get; set; }

        public int TradingDayId { get; set; }

        [UIHint("Screenshot")]
        [Display(Name = "Entry SS")]
        public virtual int EntryScreenshotDbId { get; set; }

        [UIHint("Screenshot")]
        [Display(Name = "Exit SS")]
        public virtual int ExitScreenshotDbId { get; set; }
    }
}
