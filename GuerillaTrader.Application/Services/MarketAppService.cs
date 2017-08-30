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

namespace GuerillaTrader.Services
{
    public class MarketAppService : AppServiceBase, IMarketAppService
    {
        public readonly IRepository<Market> _repository;

        public MarketAppService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, IBackgroundJobManager backgroundJobManager, IObjectMapper objectMapper, IRepository<Market> repository)
            : base(sqlExecuter, consoleHubProxy, backgroundJobManager, objectMapper)
        {
            this._repository = repository;
        }

        public MarketDto Get(int id)
        {
            return _repository.Get(id).MapTo<MarketDto>();
        }

        public List<MarketDto> GetAll()
        {
            return _objectMapper.Map<List<MarketDto>>(_repository.GetAll().OrderBy(x => x.Symbol).ToList());
        }

        public List<MarketDto> GetAllActive()
        {
            return _objectMapper.Map<List<MarketDto>>(_repository.GetAll().Where(x => x.Active).OrderBy(x => x.Symbol).ToList());
        }

        public void Save(MarketDto dto)
        {
            if (dto.IsNew)
            {
                Market market = dto.MapTo<Market>();
                this._repository.Insert(market);
            }
            else
            {
                Market market = this._repository.Get(dto.Id);
                dto.MapTo(market);
            }
        }

        public void Save(TosMarketDto dto)
        {
            if (dto.IsNew)
            {
                Market market = dto.MapTo<Market>();
                this._repository.Insert(market);
            }
            else
            {
                Market market = this._repository.Get(dto.Id);
                dto.MapTo(market);
            }
        }

        public async Task GenerateSeedCode()
        {
            List<TosMarketDto> tosMarkets = await LoadAndScrape();

            StreamWriter marketsSeedFile = new StreamWriter("marketsSeed.cs");

            foreach(TosMarketDto tosMarket in tosMarkets)
            {
                //new Market { Name = "E-Mini NASDAQ 100", Symbol = "NQ", TickSize = .25m, TickValue = 5, InitialMargin = 4290, MTT = 4, Active = false },
                int mtt = this._repository.GetAll().Any(x => x.Symbol == tosMarket.Symbol) ? this._repository.FirstOrDefault(x => x.Symbol == tosMarket.Symbol).MTT : 0;
                marketsSeedFile.WriteLine("{{Name = \"{0}\", Symbol = \"{1}\", TickSize = {2}m, TickValue = {3}m, InitialMargin = {4}m, MTT = {5}}},",
                    tosMarket.Name, tosMarket.Symbol, tosMarket.TickSize,
                    tosMarket.TickValue  == 0m ? "TD" : tosMarket.TickValue.ToString(),
                    tosMarket.InitialMargin == 0m ? "TD" : tosMarket.InitialMargin.ToString(), mtt);
            }

            marketsSeedFile.Close();
        }

        public async Task UpdateTosProperties()
        {
            List<TosMarketDto> tosMarkets = await LoadAndScrape(false);

            foreach(TosMarketDto tosMarket in tosMarkets)
            {
                Market m = this._repository.GetAll().Single(x => x.Symbol == tosMarket.Symbol);
                tosMarket.Id = m.Id;
                tosMarket.DailyWave = tosMarket.DailyWave * m.TickValue;
            }

            Decimal minDailyVolume = tosMarkets.Min(x => x.DailyVolume);
            Decimal maxDailyVolume = tosMarkets.Max(x => x.DailyVolume);

            Decimal minDailyWave = tosMarkets.Min(x => x.DailyWave);
            Decimal maxDailyWave = tosMarkets.Max(x => x.DailyWave);
            Decimal idx = 0m;
            Decimal totalMarkets = (Decimal)(tosMarkets.Count - 1);

            foreach (TosMarketDto tosMarket in tosMarkets.OrderBy(x => x.DailyVolume))
            {
                //tosMarket.VolumeScore = ((maxDailyVolume - tosMarket.DailyVolume) / (maxDailyVolume - minDailyVolume)) * 100m;
                tosMarket.VolumeScore = (idx / totalMarkets) * 100m;
                tosMarket.WaveScore = ((minDailyWave - tosMarket.DailyWave) / (minDailyWave - maxDailyWave)) *100m;
                tosMarket.CompositeScore = (tosMarket.VolumeScore + tosMarket.WaveScore) / 2;
                Save(tosMarket);
                idx += 1m;
            }
        }

        public async Task<List<TosMarketDto>> LoadAndScrape(bool scrape = true)
        {
            List<TosMarketDto> markets = new List<TosMarketDto>();

            try
            {
                using (TextReader reader = File.OpenText("Files\\TOS_AllFutures.csv"))
                {
                    var csv = new CsvReader(reader);
                    csv.Configuration.RegisterClassMap<TosMarketDtoMap>();
                    markets = csv.GetRecords<TosMarketDto>().ToList();
                    markets.RemoveAll(x => x.Symbol == "GLB");
                }

                if(scrape) await Task.WhenAll(markets.Select(x => Scrape(x)));
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
            }

            return markets;
        }

        public async Task Scrape(TosMarketDto dto)
        {
            using (var driver = new ChromeDriver())
            {
                driver.Manage().Timeouts().PageLoad = new TimeSpan(0, 0, 60);
                driver.Manage().Timeouts().AsynchronousJavaScript = new TimeSpan(0, 0, 60);
                driver.Navigate().GoToUrl($"https://institute.cmegroup.com/products/{dto.Symbol}");

                HtmlDocument doc = new HtmlDocument();

                doc.LoadHtml(driver.PageSource);
                //dollar-value-of-one-tick
                HtmlNode dollarValueOfOneTickPreviousNode = doc.DocumentNode.QuerySelector(".dollar-value-of-one-tick");
                if (dollarValueOfOneTickPreviousNode != null)
                {
                    dto.TickValue = Decimal.Parse(dollarValueOfOneTickPreviousNode.NextSibling.InnerText, System.Globalization.NumberStyles.Any);
                }

                //initial-margin-rate
                HtmlNode initialMarginRateNode = doc.DocumentNode.QuerySelector(".initial-margin-rate");
                if (initialMarginRateNode != null)
                {
                    dto.InitialMargin = Decimal.Parse(initialMarginRateNode.InnerText, System.Globalization.NumberStyles.Any);
                }
            }
        }
    }
}
