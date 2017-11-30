using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GuerillaTrader.Entities.Dtos
{
    public class TradeFromPasteDto
    {
        #region regex
        static Regex _regexOpenCoveredCallTrade = new Regex(@"(?<Date>\d{1,2}_\d{1,2}_\d{2}\s+\d{2}:\d{2}:\d{2})\sTRD\s\d{5,15}\s+(?<Type>(BOT|SOLD))\s.(?<Quantity>\d) COVERED (?<Symbol>\w{1,5}) \d{1,4} (?<OptionExpiry>\d{1,2} \w{3} \d{2}) (?<Strike>\d+\.*\d*) (CALL|PUT)_\w{1,5} @(?<Price>\d+\.\d+) \w{1,10}\s+-(?<MiscFees>\d*\.*\d*)\s+-(?<Commissions>\d*\.*\d*)");
        static Regex _regexOpenBullPutSpread = new Regex(@"(?<Date>\d{1,2}_\d{1,2}_\d{2}\s+\d{2}:\d{2}:\d{2})\sTRD\s\d{5,15}\s+(?<Type>(BOT|SOLD))\s.(?<Quantity>\d+) VERTICAL (?<Symbol>\w{1,5}) \d{1,4} (?<OptionExpiry>\d{1,2} \w{3} \d{2}) (?<ShortStrike>\d+\.*\d*)_(?<LongStrike>\d+\.*\d*)\s+(CALL|PUT)\s+@(?<Price>\d*\.\d+)\s+-(?<MiscFees>\d*\.*\d*)\s+-(?<Commissions>\d*\.*\d*)");
        #endregion

        [DataType(DataType.Date)]
        [UIHint("MyDate")]
        public DateTime Date { get; set; }

        [UIHint("TradingAccount")]
        [Display(Name = "Account")]
        public int TradingAccountId { get; set; }

        [UIHint("MyMultiline")]
        public String Trades { get; set; }

        [UIHint("TradeExitReasons")]
        [Display(Name = "Exit Reason")]
        public TradeExitReasons ExitReason { get; set; }

        public List<TradeDto> ToOpenBullPutSpreads(List<Stock> stocks)
        {
            List<TradeDto> trades = new List<TradeDto>();
            List<String> tradeLines = this.Trades.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Replace("tAndroid", String.Empty).Replace("\t", " ").Replace("/", "_")).ToList();

            foreach (String tradeLine in tradeLines)
            {
                var matches = _regexOpenBullPutSpread.Matches(tradeLine);
                TradeDto dto = new TradeDto();
                dto.TradingAccountId = this.TradingAccountId;
                dto.EntryDate = DateTime.Parse(matches[0].Groups["Date"].Value.Replace("_", "/"));
                dto.EntryPrice = Decimal.Parse(matches[0].Groups["Price"].Value);
                dto.Size = Int32.Parse(matches[0].Groups["Quantity"].Value);
                dto.Mark = dto.EntryPrice;
                dto.TradeType = TradeTypes.BullPutSpread;
                dto.Classification = TradeClassifications.Consistent;
                dto.Commissions = Decimal.Parse(matches[0].Groups["MiscFees"].Value) + Decimal.Parse(matches[0].Groups["Commissions"].Value);
                dto.StockId = stocks.Single(x => x.Symbol == (matches[0].Groups["Symbol"].Value)).Id;

                OptionDto longOptionDto = new OptionDto();
                longOptionDto.Expiry = DateTime.Parse(matches[0].Groups["OptionExpiry"].Value);
                longOptionDto.Strike = Decimal.Parse(matches[0].Groups["LongStrike"].Value);
                longOptionDto.Symbol = matches[0].Groups["Symbol"].Value;
                longOptionDto.Name = String.Format("{0:dd MMM yy} {1:N0}", longOptionDto.Expiry, longOptionDto.Strike);
                longOptionDto.OptionType = OptionTypes.Put;

                dto.BullPutSpreadLongOption = longOptionDto;

                OptionDto shortOptionDto = new OptionDto();
                shortOptionDto.Expiry = DateTime.Parse(matches[0].Groups["OptionExpiry"].Value);
                shortOptionDto.Strike = Decimal.Parse(matches[0].Groups["ShortStrike"].Value);
                shortOptionDto.Symbol = matches[0].Groups["Symbol"].Value;
                shortOptionDto.Name = String.Format("{0:dd MMM yy} {1:N0}/{2:N0}", shortOptionDto.Expiry, shortOptionDto.Strike, longOptionDto.Strike);
                shortOptionDto.OptionType = OptionTypes.Put;

                dto.BullPutSpreadShortOption = shortOptionDto;

                trades.Add(dto);
            }

            return trades;
        }

        public List<TradeDto> ToOpenCoveredCalls(List<Stock> stocks)
        {
            List<TradeDto> trades = new List<TradeDto>();
            List<String> tradeLines = this.Trades.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Replace("tAndroid", String.Empty).Replace("\t", " ").Replace("/", "_")).ToList();

            foreach(String tradeLine in tradeLines)
            {
                var matches = _regexOpenCoveredCallTrade.Matches(tradeLine);
                TradeDto dto = new TradeDto();
                dto.TradingAccountId = this.TradingAccountId;
                dto.EntryDate = DateTime.Parse(matches[0].Groups["Date"].Value.Replace("_", "/"));
                dto.EntryPrice = Decimal.Parse(matches[0].Groups["Price"].Value);
                dto.Size = Int32.Parse(matches[0].Groups["Quantity"].Value) * 100;
                dto.Mark = dto.EntryPrice;
                dto.TradeType = TradeTypes.CoveredCall;
                dto.Classification = TradeClassifications.Consistent;
                dto.Commissions = Decimal.Parse(matches[0].Groups["MiscFees"].Value) + Decimal.Parse(matches[0].Groups["Commissions"].Value);
                dto.StockId = stocks.Single(x => x.Symbol == (matches[0].Groups["Symbol"].Value)).Id;

                OptionDto optionDto = new OptionDto();
                optionDto.Expiry = DateTime.Parse(matches[0].Groups["OptionExpiry"].Value);
                optionDto.Strike = Decimal.Parse(matches[0].Groups["Strike"].Value);
                optionDto.Symbol = matches[0].Groups["Symbol"].Value;
                optionDto.Name = String.Format("{0:dd MMM yy} {1:N0}", optionDto.Expiry, optionDto.Strike);
                optionDto.OptionType = OptionTypes.Call;

                dto.CoveredCallOption = optionDto;

                trades.Add(dto);
            }

            return trades;
        }

        public void ToUpdateCoveredCalls(List<TradeDto> trades)
        {
            List<String> statementLines = this.Trades.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Replace("tAndroid", String.Empty).Replace("\t", " ").Replace("/", "_")).ToList();

            for(int i = 0; i < statementLines.Count; i += 3)
            {
                TradeDto dto = trades.First(x => x.Stock.Symbol == statementLines[i].Trim());
                dto.ExitReason = this.ExitReason;
                dto.Stock.Price = Decimal.Parse(statementLines[i + 1]);
                dto.CoveredCallOption.Price = Decimal.Parse(statementLines[i + 2]);
                dto.Mark = dto.Stock.Price - dto.CoveredCallOption.Price;
            }
        }

        public void ToUpdateBullPutSpreads(List<TradeDto> trades)
        {
            List<String> statementLines = this.Trades.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Replace("tAndroid", String.Empty).Replace("\t", " ").Replace("/", "_")).ToList();

            for (int i = 0; i < statementLines.Count; i += 4)
            {
                TradeDto dto = trades.First(x => x.Stock.Symbol == statementLines[i].Trim());
                dto.ExitReason = this.ExitReason;
                dto.Stock.Price = Decimal.Parse(statementLines[i + 1]);
                dto.BullPutSpreadLongOption.Price = Decimal.Parse(statementLines[i + 2]);
                dto.BullPutSpreadShortOption.Price = Decimal.Parse(statementLines[i + 3]);
                dto.Mark = dto.BullPutSpreadShortOption.Price - dto.BullPutSpreadLongOption.Price;
            }
        }

        public List<TradeDto> ToFutureTradeDto(List<Market> markets)
        {
            List<TradeDto> trades = new List<TradeDto>();

            List<String> tradeLines = this.Trades.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Replace("tAndroid", String.Empty).Replace("\t", " ")).ToList();

            if (tradeLines == null || tradeLines.Count == 0) return trades;

            int tradeCount = tradeLines.Count / 2;

            while (trades.Count < tradeCount)
            {
                String openingLine = tradeLines[0];
                String[] openingParts = openingLine.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                tradeLines.RemoveAt(0);

                int closingIndex = tradeLines.FindIndex(x => x.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[6] == openingParts[6]);
                String closingLine = tradeLines[closingIndex];
                String[] closingParts = closingLine.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                tradeLines.RemoveAt(closingIndex);

                TradeDto tradeDto = new TradeDto();

                tradeDto.EntryDate = DateTime.Parse($"{openingParts[0]} {openingParts[1]}");
                Market market = markets.First(x => x.Symbol == openingParts[6].Substring(1, 2));
                tradeDto.MarketId = market.Id;
                tradeDto.TradingAccountId = this.TradingAccountId;
                tradeDto.Timeframe = market.MTT;
                tradeDto.Size = Math.Abs(Int32.Parse(openingParts[5], System.Globalization.NumberStyles.Any));
                tradeDto.Classification = TradeClassifications.Consistent;
                tradeDto.TradeType = openingParts[4] == "BOT" ? TradeTypes.LongFuture : TradeTypes.ShortFuture;
                tradeDto.EntryPrice = Decimal.Parse(openingParts[7].Replace("@", String.Empty).Replace("'", "."));


                tradeDto.ExitDate = DateTime.Parse($"{closingParts[0]} {closingParts[1]}");
                tradeDto.ExitPrice = Decimal.Parse(closingParts[7].Replace("@", String.Empty).Replace("'", "."));
                tradeDto.Mark = tradeDto.ExitPrice;
                tradeDto.ExitReason = ((tradeDto.TradeType == TradeTypes.LongFuture && tradeDto.EntryPrice < tradeDto.ExitPrice) || (tradeDto.TradeType == TradeTypes.ShortFuture && tradeDto.EntryPrice > tradeDto.ExitPrice)) ? TradeExitReasons.TargetHit : TradeExitReasons.StopLossHit;

                if ((tradeDto.TradeType == TradeTypes.LongFuture && tradeDto.EntryPrice < tradeDto.ExitPrice) || (tradeDto.TradeType == TradeTypes.ShortFuture && tradeDto.EntryPrice > tradeDto.ExitPrice))
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
