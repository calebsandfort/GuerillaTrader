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
using Tesseract;
using System.IO;
using GuerillaTrader.Shared.Dtos;
using GuerillaTrader.BackgroundJobs;

namespace GuerillaTrader.Services
{
    public class ScreenshotAppService : AppServiceBase, IScreenshotAppService
    {
        public readonly IRepository<Screenshot> _repository;
        public readonly IRepository<MarketLogEntry> _marketLogEntryepository;
        public readonly IRepository<Trade> _tradeRepository;

        public ScreenshotAppService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, IBackgroundJobManager backgroundJobManager, IObjectMapper objectMapper,
            IRepository<Screenshot> repository, IRepository<MarketLogEntry> marketLogEntryepository, IRepository<Trade> tradeRepository)
            : base(sqlExecuter, consoleHubProxy, backgroundJobManager, objectMapper)
        {
            this._repository = repository;
            this._marketLogEntryepository = marketLogEntryepository;
            this._tradeRepository = tradeRepository;
        }

        public ScreenshotDto Get(int id)
        {
            return _repository.Get(id).MapTo<ScreenshotDto>();
        }

        public void Save(ScreenshotDto dto)
        {
            if (dto.IsNew)
            {
                Screenshot Screenshot = dto.MapTo<Screenshot>();
                this._repository.Insert(Screenshot);
            }
            else
            {
                Screenshot Screenshot = this._repository.Get(dto.Id);
                dto.MapTo(Screenshot);
            }
        }

        //[UnitOfWork(IsDisabled = true)]
        public ScreenshotDto SaveBase64(String base64)
        {
            Screenshot screenshot = new Screenshot { Data = Convert.FromBase64String(base64.Replace("data:image/png;base64,", "")) };
            int id = 0;
            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                id = this._repository.InsertAndGetId(screenshot);
                unitOfWork.Complete();
            }
            ScreenshotDto dto = new ScreenshotDto();
            screenshot.MapTo(dto);
            dto.Id = id;
            return dto;
        }


        public void RecognizeTextEnqueue(int id)
        {
            _backgroundJobManager.Enqueue<RunRecognizeTextBackgroundJob, int>(id);
        }

        public void RecognizeText(int id)
        {
            Screenshot screenshot = this._repository.Get(id);

            //var i = new System.Drawing.Bitmap(new MemoryStream(screenshot.Data));
            //i.Save("image.bmp");

            using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            {
                using (var image = new System.Drawing.Bitmap(new MemoryStream(screenshot.Data)))
                {
                    using (var pix = PixConverter.ToPix(image))
                    {
                        using (var page = engine.Process(pix))
                        {
                            var text = page.GetText();
                            Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

                            Console.WriteLine("Text (GetText): \r\n{0}", text);
                            Console.WriteLine("Text (iterator):");
                            using (var iter = page.GetIterator())
                            {
                                iter.Begin();

                                do
                                {
                                    do
                                    {
                                        do
                                        {
                                            do
                                            {
                                                if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                                {
                                                    Console.WriteLine("<BLOCK>");
                                                }

                                                var t = iter.GetText(PageIteratorLevel.Word);
                                                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create(t));
                                                Console.Write(" ");

                                                if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                                {
                                                    Console.WriteLine();
                                                }
                                            } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                                            if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                            {
                                                Console.WriteLine();
                                            }
                                        } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                                    } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                                } while (iter.Next(PageIteratorLevel.Block));
                            }
                        }
                    }
                }
            }
        }

        public void ConvertAll()
        {
            //foreach(MarketLogEntry marketLogEntry in this._marketLogEntryepository.GetAll().Where(x => !String.IsNullOrEmpty(x.Screenshot) && !x.ScreenshotDbId.HasValue).ToList())
            //{
            //    marketLogEntry.ScreenshotDb = new Screenshot { Data = Convert.FromBase64String(marketLogEntry.Screenshot.Replace("data:image/png;base64,", "")) };
            //}

            //foreach (Trade trade in this._tradeRepository.GetAll().Where(x => !String.IsNullOrEmpty(x.EntryScreenshot) && !x.EntryScreenshotDbId.HasValue).ToList())
            //{
            //    trade.EntryScreenshotDb = new Screenshot { Data = Convert.FromBase64String(trade.EntryScreenshot.Replace("data:image/png;base64,", "")) };
            //    trade.ExitScreenshotDb = new Screenshot { Data = Convert.FromBase64String(trade.ExitScreenshot.Replace("data:image/png;base64,", "")) };
            //}
        }
    }
}
