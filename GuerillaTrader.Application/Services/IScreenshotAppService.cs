using Abp.Application.Services;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Entities.Dtos;

namespace GuerillaTrader.Services
{
    public interface IScreenshotAppService : IApplicationService
    {
        ScreenshotDto Get(int id);
        void Save(ScreenshotDto dto);
        ScreenshotDto SaveBase64(String base64);
        void ConvertAll();
    }
}
