using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Entities.Dtos
{
    public class TradeFromPasteDto
    {
        [UIHint("MyMultiline")]
        public String Opening { get; set; }

        [UIHint("MyMultiline")]
        public String Closing { get; set; }

        public List<TradeDto> ToTradeDto(List<Market> markets)
        {
            List<TradeDto> trades = new List<TradeDto>();

            String[] openingLines = this.Opening.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            String[] closingLines = this.Closing.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            if (openingLines == null || openingLines.Length == 0) return trades;

            for(int i = 0; i < openingLines.Length; i++)
            {
                TradeDto tradeDto = new TradeDto();

                openingLines[i] = openingLines[i].Replace("tAndroid", String.Empty).Replace("\t", " ");
                closingLines[i] = closingLines[i].Replace("tAndroid", String.Empty).Replace("\t", " ");

                String[] openingParts = openingLines[i].Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                tradeDto.EntryDate = DateTime.Parse($"{openingParts[0]} {openingParts[1]}");
                Market market = markets.First(x => x.Symbol == openingParts[6].Substring(1, 2));
                tradeDto.MarketId = market.Id;
                tradeDto.Timeframe = market.MTT;
                tradeDto.Size = Math.Abs(Int32.Parse(openingParts[5], System.Globalization.NumberStyles.Any));
                tradeDto.Classification = TradeClassifications.Consistent;
                tradeDto.TradeType = openingParts[4] == "BOT" ? TradeTypes.Long : TradeTypes.Short;
                tradeDto.EntryPrice = Decimal.Parse(openingParts[7].Replace("@", String.Empty).Replace("'", "."));

                String[] closingParts = closingLines[i].Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                tradeDto.ExitDate = DateTime.Parse($"{closingParts[0]} {closingParts[1]}");
                tradeDto.ExitPrice = Decimal.Parse(closingParts[7].Replace("@", String.Empty).Replace("'", "."));
                tradeDto.ExitReason = ((tradeDto.TradeType == TradeTypes.Long && tradeDto.EntryPrice < tradeDto.ExitPrice) || (tradeDto.TradeType == TradeTypes.Short && tradeDto.EntryPrice > tradeDto.ExitPrice)) ? TradeExitReasons.TargetHit : TradeExitReasons.StopLossHit;
                
                if ((tradeDto.TradeType == TradeTypes.Long && tradeDto.EntryPrice < tradeDto.ExitPrice) || (tradeDto.TradeType == TradeTypes.Short && tradeDto.EntryPrice > tradeDto.ExitPrice))
                {
                    tradeDto.ProfitTakerPrice = tradeDto.ExitPrice;
                }
                else
                {
                    tradeDto.StopLossPrice = tradeDto.ExitPrice;
                }

                tradeDto.Commissions = Math.Abs(Decimal.Parse(openingParts[8]) + Decimal.Parse(openingParts[9]) + Decimal.Parse(closingParts[8]) + Decimal.Parse(closingParts[9]));
                trades.Add(tradeDto);
            }

            return trades;
        }
    }
}
