using Abp.AutoMapper;
using GuerillaTrader.Entities;
using GuerillaTrader.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuerillaTrader.Web.Models
{
    [AutoMap(typeof(ScreenshotDto))]
    public class ExtractedPricesModel
    {
        public Decimal EntryPrice { get; set; }
        public Decimal StopLossPrice { get; set; }
        public Decimal ProfitTakerPrice { get; set; }

        public ExtractedPricesModel()
        {

        }

        public ExtractedPricesModel(String[] priceStrings, TradeTypes tradeType)
        {
            if(priceStrings != null && priceStrings.Length > 0)
            {
                if(tradeType == TradeTypes.LongFuture)
                {
                    this.EntryPrice = ExtractPrice(priceStrings, "Upper Step", "Upper Stop", "Upper smp", "Upper Slop");
                    this.StopLossPrice = ExtractPrice(priceStrings, "Lower Step", "Lower Stop", "Lower smp", "Lower Slop");
                    this.ProfitTakerPrice = ExtractPrice(priceStrings, "Upper Target");
                }
                else
                {
                    this.EntryPrice = ExtractPrice(priceStrings, "Lower Step", "Lower Stop", "Lower smp", "Lower Slop");
                    this.StopLossPrice = ExtractPrice(priceStrings, "Upper Step", "Upper Stop", "Upper smp", "Upper Slop");
                    this.ProfitTakerPrice = ExtractPrice(priceStrings, "Lower Target");
                }
            }
        }

        private static Decimal ExtractPrice(String[] priceStrings, params String[] labels)
        {
            Decimal extractedPrice = 0.0m;

            foreach(String label in labels)
            {
                if(priceStrings.Any(x => x.Contains(label)))
                {
                    String p = priceStrings.First(x => x.Contains(label));
                    p = p.Substring(p.IndexOf(label) + label.Length);
                    p = p.Replace(label, String.Empty).Replace(",", String.Empty).Replace("(", String.Empty).Replace("$", String.Empty).Replace(")", String.Empty).Replace(" ", String.Empty).Replace("'", ".");
                    if (Decimal.TryParse(p, out extractedPrice) && extractedPrice > 0.0m) return extractedPrice;
                }
            }

            return extractedPrice;
        }
    }
}