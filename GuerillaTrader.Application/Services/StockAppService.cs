using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Entities;
using GuerillaTrader.Entities.Dtos;
using GuerillaTrader.Shared.SqlExecuter;
using Abp.BackgroundJobs;
using GuerillaTrader.Shared;
using System.IO;
using CsvHelper;
using GuerillaTrader.Shared.Dtos;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using System.Net;
using GuerillaTrader.Framework;
using System.Text.RegularExpressions;

namespace GuerillaTrader.Services
{
    public class StockAppService : AppServiceBase, IStockAppService
    {
        const String YahooUrl = "https://finance.yahoo.com/quote/{0}";

        #region Regex
        private static Regex _regexExDividendDate = new Regex(@"<!-- react-text: 124 -->(?<ExDividendDate>\d{4}-\d{1,2}-\d{1,2})", RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static Regex _regexTargetPrice = new Regex(@"<!-- react-text: 130 -->(?<TargetPrice>\d+,*\d*\.\d{2})", RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static Regex _regexNextEarningsDate = new Regex(@"<span data-reactid=""112"">(?<NextEarningsDate>\D{3} \d{1,2}, \d{4})", RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static Regex _regexPrice = new Regex(@"<!-- react-text: 36 -->(?<Price>\d+,*\d*\.\d{2})", RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static Regex _regexAvgVolume = new Regex(@"<!-- react-text: 80 -->(?<AvgVolume>\d+,\d+,\d+)", RegexOptions.CultureInvariant | RegexOptions.Compiled);
        #endregion

        public readonly IRepository<Stock> _repository;
        public readonly IRepository<Sector> _sectorRepository;

        public StockAppService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, IBackgroundJobManager backgroundJobManager, IObjectMapper objectMapper, IRepository<Stock> repository, IRepository<Sector> sectorRepository)
            : base(sqlExecuter, consoleHubProxy, backgroundJobManager, objectMapper)
        {
            this._repository = repository;
            this._sectorRepository = sectorRepository;
        }

        public StockDto Get(int id)
        {
            return _repository.Get(id).MapTo<StockDto>();
        }

        public List<StockDto> GetAll()
        {
            return _objectMapper.Map<List<StockDto>>(_repository.GetAll().OrderBy(x => x.Name).ToList());
        }

        public void Save(StockDto dto)
        {
            if (dto.IsNew)
            {
                Stock stock = dto.MapTo<Stock>();
                this._repository.Insert(stock);
            }
            else
            {
                Stock stock = this._repository.Get(dto.Id);
                dto.MapTo(stock);
            }
        }

        public void Save(StockUpdatePriceAndDatesDto dto)
        {
            if (dto.IsNew)
            {
                Stock stock = dto.MapTo<Stock>();
                this._repository.Insert(stock);
            }
            else
            {
                Stock stock = this._repository.Get(dto.Id);
                dto.MapTo(stock);
            }
        }

        public void Save(PfStockDto dto)
        {
            Stock stock = this._repository.GetAll().SingleOrDefault(x => x.Symbol == dto.Symbol);

            if (stock == null || stock.IsNew)
            {
                stock = dto.MapTo<Stock>();
                this._repository.Insert(stock);
            }
            else
            {
                dto.MapTo(stock);
            }
        }

        public void Save(SectorDto dto)
        {
            if (dto.IsNew)
            {
                Sector sector = dto.MapTo<Sector>();
                this._sectorRepository.Insert(sector);
            }
            else
            {
                Sector sector = this._sectorRepository.Get(dto.Id);
                dto.MapTo(sector);
            }
        }

        public void UpdatePfProperties()
        {
            List<PfStockDto> pfStocks = new List<PfStockDto>();

            using (TextReader reader = File.OpenText("Files\\SP500.csv"))
            {
                var csv = new CsvReader(reader);
                csv.Configuration.RegisterClassMap<PfStockDtoMap>();
                pfStocks = csv.GetRecords<PfStockDto>().ToList();
            }

            foreach (PfStockDto pfStock in pfStocks)
            {
                if (String.IsNullOrEmpty(pfStock.Symbol))
                {
                    throw new Exception($"{pfStock.Name} doesn't have a symbol.");
                }

                Save(pfStock);
            }
        }

        public void UpdateTaxProperties()
        {
            List<TaxRateStockDto> taxRateStocks = new List<TaxRateStockDto>();

            using (TextReader reader = File.OpenText("Files\\SP500TaxRates.csv"))
            {
                var csv = new CsvReader(reader);
                csv.Configuration.RegisterClassMap<TaxRateStockDtoMap>();
                taxRateStocks = csv.GetRecords<TaxRateStockDto>().ToList();
            }

            List<Stock> stocks = _repository.GetAllList();

            foreach(Stock stock in stocks)
            {
                TaxRateStockDto dto = taxRateStocks.FirstOrDefault(x => x.CompanyName.StartsWith(stock.Name, StringComparison.InvariantCulture));
                if(dto == null)
                {
                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"{stock.Name}: {stock.Symbol}"));
                }
            }
        }

        public void UpdateSectorProperties()
        {
            List<Sector> sectors = this._sectorRepository.GetAllList();
            List<Stock> stocks = this._repository.GetAll().Where(x => !x.FailedToRetrieveBars).ToList();

            foreach (Sector sector in sectors)
            {
                sector.RecentPerf = stocks.Where(x => x.Sector == sector.Name).Average(x => x.RecentPerf);
                sector.PastPerf = stocks.Where(x => x.Sector == sector.Name).Average(x => x.PastPerf);
                sector.PastPositivePerf = stocks.Where(x => x.Sector == sector.Name).Average(x => x.PastPositivePerf);
            }
        }

        #region UpdatePriceAndDates
        [UnitOfWork(IsDisabled = true)]
        public async Task UpdatePriceAndDates()
        {
            try
            {
                List<Stock> stocks = new List<Stock>();

                using (var unitOfWork = this.UnitOfWorkManager.Begin())
                {
                    //stocks = this._repository.GetAll().Where(x => x.Symbol == "GOOGL").ToList();
                    stocks = this._repository.GetAll().ToList();
                    unitOfWork.Complete();
                }

                int pageSize = 50;
                int pageCount = (int)Math.Ceiling(((Double)stocks.Count / (Double)pageSize));

                for (int i = 0; i < pageCount; i++)
                {
                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Page {i + 1} of {pageCount}..."));

                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        List<StockUpdatePriceAndDatesDto> dtos = (await Task.WhenAll(stocks.Skip(i * pageSize).Take(pageSize).Select(x => ScrapePriceAndDates(x)).ToArray())).ToList();
                        foreach (StockUpdatePriceAndDatesDto dto in dtos)
                        {
                            Save(dto);
                        }
                        unitOfWork.Complete();
                    }

                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Page {i + 1} of {pageCount} finished."));
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
            }
        } 

        private async Task<StockUpdatePriceAndDatesDto> ScrapePriceAndDates(Stock stock)
        {
            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"ScrapePriceAndDates for : {stock.Symbol}"));

            StockUpdatePriceAndDatesDto dto = stock.MapTo<StockUpdatePriceAndDatesDto>();

            using (WebClient webClient = new WebClient())
            {
                String html = await webClient.DownloadStringTaskAsync(String.Format(YahooUrl, stock.Symbol));
                var matches = _regexExDividendDate.Matches(html);

                dto.ExDividendDate = null;
                DateTime exDividendDate = DateTime.MinValue;
                if (matches.Count > 0 && DateTime.TryParse(matches[0].Groups["ExDividendDate"].Value, out exDividendDate))
                {
                    dto.ExDividendDate = exDividendDate;
                }

                matches = _regexTargetPrice.Matches(html);
                dto.TargetPrice = null;
                Decimal targetPrice = 0m;
                if (matches.Count > 0 && Decimal.TryParse(matches[0].Groups["TargetPrice"].Value, out targetPrice))
                {
                    dto.TargetPrice = targetPrice;
                }

                matches = _regexNextEarningsDate.Matches(html);
                dto.NextEarningsDate = null;
                DateTime nextEarningsDate = DateTime.MinValue;
                if (matches.Count > 0 && DateTime.TryParse(matches[0].Groups["NextEarningsDate"].Value, out nextEarningsDate))
                {
                    dto.NextEarningsDate = nextEarningsDate;
                }

                matches = _regexPrice.Matches(html);
                dto.Price = 0m;
                Decimal price = 0m;
                if (matches.Count > 0 && Decimal.TryParse(matches[0].Groups["Price"].Value, out price))
                {
                    dto.Price = price;
                }

                matches = _regexAvgVolume.Matches(html);
                dto.AvgVolume = 0;
                int avgVolume = 0;
                if (matches.Count > 0 && Int32.TryParse(matches[0].Groups["AvgVolume"].Value.Replace(",", String.Empty), out avgVolume))
                {
                    dto.AvgVolume = avgVolume;
                }

                if (dto.AvgVolume.HasValue) dto.ADV = dto.AvgVolume.Value * dto.Price;
            }


            return dto;
        }
        #endregion

        #region DataDownload
        [UnitOfWork(IsDisabled = true)]
        public void DeleteReports()
        {
            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                this._sqlExecuter.Execute("DELETE FROM [StockReports]");
                unitOfWork.Complete();
            }
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task GenerateReports(GenerateStockReportsInput input)
        {
            try
            {

                //this._sqlExecuter.Execute("DELETE FROM [GuerillaTrader].[dbo].[StockReports]");

                List<Stock> stocks = new List<Stock>();

                using (var unitOfWork = this.UnitOfWorkManager.Begin())
                {
                    stocks = this._repository.GetAllIncluding(x => x.StockReports.Select(y => y.StockBars)).ToList();
                    unitOfWork.Complete();
                }

                int pageSize = 50;
                int pageCount = (int)Math.Ceiling(((Double)stocks.Count / (Double)pageSize));

                for (int i = 0; i < pageCount; i++)
                {
                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Page {i + 1} of {pageCount}..."));

                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        List<StockDto> dtos = (await Task.WhenAll(stocks.Skip(i*pageSize).Take(pageSize).Select(x => ReportWrapper(x, input.StartDate, input.Period, input.Lookback)).ToArray())).ToList();
                        foreach(StockDto dto in dtos)
                        {
                            dto.SetStats(input.Period);
                            Save(dto);
                        }
                        unitOfWork.Complete();
                    }

                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Page {i + 1} of {pageCount} finished."));
                }
            }
            catch(Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
            }
        }

        private async Task<StockDto> ReportWrapper(Stock stock, DateTime date, int period, int lookback)
        {
            StockDto dto = stock.MapTo<StockDto>();
            dto.StockReports = await GetReports(stock.Symbol, date, period, lookback);

            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Generating reports for : {stock.Symbol}"));

            return dto;
        }

        private async Task<List<StockReportDto>> GetReports(String symbol, DateTime date, int period, int lookback)
        {
            List<Task<StockReportDto>> getReportTasks = new List<Task<StockReportDto>>();
            getReportTasks.Add(GetReportViaYahoo(symbol, date.AddDays(-period), date, "usa"));

            for(int i = 1; i <= lookback; i++)
            {
                getReportTasks.Add(GetReportViaYahoo(symbol, date.AddYears(-i), date.AddYears(-i).AddDays(period), "usa"));
            }

            List<StockReportDto> reports = (await Task.WhenAll(getReportTasks.ToArray())).ToList();

            return reports;
        }

        #region Yahoo
        /// <summary>
        /// Get stock historical price from Yahoo Finance
        /// </summary>
        /// <param name="symbol">Stock ticker symbol</param>
        /// <param name="start">Starting datetime</param>
        /// <param name="end">Ending datetime</param>
        /// <returns>List of history price</returns>
        public async Task<StockReportDto> GetReportViaYahoo(string symbol, DateTime start, DateTime end, string eventCode)
        {
            var historyPrices = new List<StockBarDto>();

            try
            {
                var csvData = GetRaw(symbol, start, end, eventCode);
                if (csvData != null)
                {
                    historyPrices = Parse(csvData);
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Unable to retrieve bars for: {symbol}"));

                return new StockReportDto { StartDate = start, EndDate = end, StockBars = historyPrices, FailedToRetrieveBars = true };
            }

            return new StockReportDto { StartDate = start, EndDate = end, StockBars = historyPrices };

        }

        /// <summary>
        /// Get raw stock historical price from Yahoo Finance
        /// </summary>
        /// <param name="symbol">Stock ticker symbol</param>
        /// <param name="start">Starting datetime</param>
        /// <param name="end">Ending datetime</param>
        /// <returns>Raw history price string</returns>
        public string GetRaw(string symbol, DateTime start, DateTime end, string eventCode)
        {

            string csvData = null;

            try
            {
                var url = "https://query1.finance.yahoo.com/v7/finance/download/{0}?period1={1}&period2={2}&interval=1d&events={3}&crumb={4}";

                //if no token found, refresh it
                if (string.IsNullOrEmpty(Token.Cookie) | string.IsNullOrEmpty(Token.Crumb))
                {
                    if (!Token.Refresh(symbol))
                    {
                        return GetRaw(symbol, start, end, eventCode);
                    }
                }

                url = string.Format(url, symbol, Math.Round(Time.DateTimeToUnixTimeStamp(start), 0), Math.Round(Time.DateTimeToUnixTimeStamp(end), 0), eventCode, Token.Crumb);
                using (var wc = new WebClient())
                {
                    wc.Headers.Add(HttpRequestHeader.Cookie, Token.Cookie);
                    csvData = wc.DownloadString(url);
                }

            }
            catch (WebException webEx)
            {
                var response = (HttpWebResponse)webEx.Response;

                //Re-fecthing token
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //Debug.Print(webEx.Message);
                    //Token.Reset();
                    //Debug.Print("Re-fetch");
                    return GetRaw(symbol, start, end, eventCode);
                }
                throw;

            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
            }

            return csvData;

        }

        /// <summary>
        /// Parse raw historical price data into list
        /// </summary>
        /// <param name="csvData"></param>
        /// <returns></returns>
        private List<StockBarDto> Parse(string csvData)
        {

            var hps = new List<StockBarDto>();

            try
            {
                var rows = csvData.Split(Convert.ToChar(10));

                //row(0) was ignored because is column names 
                //data is read from oldest to latest
                for (var i = 1; i <= rows.Length - 1; i++)
                {

                    var row = rows[i];
                    if (string.IsNullOrEmpty(row))
                    {
                        continue;
                    }

                    var cols = row.Split(',');
                    if (cols[1] == "null")
                    {
                        continue;
                    }

                    var hp = new StockBarDto
                    {
                        Date = DateTime.Parse(cols[0]),
                        Open = Convert.ToDecimal(cols[1]),
                        High = Convert.ToDecimal(cols[2]),
                        Low = Convert.ToDecimal(cols[3]),
                        //Close = Convert.ToDecimal(cols[4]),
                        Close = Convert.ToDecimal(cols[5])
                    };

                    hps.Add(hp);

                }

            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
            }

            return hps;

        }
        #endregion

        #region Google
        #region Props
        private const string UrlPrototypeDaily = @"https://finance.google.com/finance/historical?q={0}&startdate={1}&enddate={2}&output=csv";
        private enum ConvertMonths
        {
            Jan = 1,
            Feb = 2,
            Mar = 3,
            Apr = 4,
            May = 5,
            Jun = 6,
            Jul = 7,
            Aug = 8,
            Sep = 9,
            Oct = 10,
            Nov = 11,
            Dec = 12
        };
        #endregion

        private async Task<StockReportDto> GetReportViaGoogle(String symbol, DateTime start, DateTime end)
        {
            try
            {
                var startdate = ((ConvertMonths)start.Month).ToString()
                    + @"+" + start.Day.ToString()
                    + @"%2C+" + start.Year.ToString();
                var enddate = ((ConvertMonths)end.Month).ToString()
                    + @"+" + end.Day.ToString()
                    + @"%2C+" + end.Year.ToString();

                // Create the Google formatted URL.
                var url = string.Format(UrlPrototypeDaily, symbol, startdate, enddate);

                // Download the data from Google.
                string[] lines;
                using (var client = new WebClient())
                {
                    var data = client.DownloadString(url);
                    lines = data.Split('\n');
                }

                // first line is header
                var currentLine = 1;

                List<StockBarDto> bars = new List<StockBarDto>();

                while (currentLine < lines.Length - 1)
                {
                    // Format: Date,Open,High,Low,Close,Volume
                    var columns = lines[currentLine].Split(',');

                    // date format: DD-Mon-YY, e.g. 27-Sep-16
                    var DMY = columns[0].Split('-');

                    // date = 20160927
                    var day = DMY[0].ToInt32();
                    var month = (int)Enum.Parse(typeof(ConvertMonths), DMY[1]);
                    var year = (DMY[2].ToInt32() > 70) ? 1900 + DMY[2].ToInt32() : 2000 + DMY[2].ToInt32();
                    var time = new DateTime(year, month, day, 0, 0, 0);

                    // occasionally, the columns will have a '-' instead of a proper value
                    List<Decimal?> ohlc = new List<Decimal?>()
                {
                    columns[1] != "-" ? (Decimal?)columns[1].ToDecimal() : null,
                    columns[2] != "-" ? (Decimal?)columns[2].ToDecimal() : null,
                    columns[3] != "-" ? (Decimal?)columns[3].ToDecimal() : null,
                    columns[4] != "-" ? (Decimal?)columns[4].ToDecimal() : null
                };

                    if (ohlc.Where(val => val == null).Count() > 0)
                    {
                        // let's try hard to fix any issues as good as we can
                        // this code assumes that there is at least 1 good value
                        if (ohlc[1] == null) ohlc[1] = ohlc.Where(val => val != null).Max();
                        if (ohlc[2] == null) ohlc[2] = ohlc.Where(val => val != null).Min();
                        if (ohlc[0] == null) ohlc[0] = ohlc.Where(val => val != null).Average();
                        if (ohlc[3] == null) ohlc[3] = ohlc.Where(val => val != null).Average();

                        //Log.Error(string.Format("Corrupt bar on {0}: {1},{2},{3},{4}. Saved as {5},{6},{7},{8}.",
                        //    columns[0], columns[1], columns[2], columns[3], columns[4],
                        //    ohlc[0], ohlc[1], ohlc[2], ohlc[3]));
                    }

                    //long volume = columns[5].ToInt64();

                    bars.Add(new StockBarDto { Date = time, Open = (Decimal)ohlc[0], High = (Decimal)ohlc[1], Low = (Decimal)ohlc[2], Close = (Decimal)ohlc[3] });

                    currentLine++;
                }

                return new StockReportDto { StartDate = start, EndDate = end, StockBars = bars };
            }
            catch
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Unable to retrieve bars for: {symbol}"));
                return new StockReportDto { StartDate = start, EndDate = end, StockBars = new List<StockBarDto>(), FailedToRetrieveBars = true };
            }
        } 
        #endregion
        #endregion
    }
}
