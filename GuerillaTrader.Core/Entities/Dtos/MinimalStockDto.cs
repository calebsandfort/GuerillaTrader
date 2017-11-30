using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Entities.Dtos
{
    [AutoMap(typeof(Stock))]
    public class MinimalStockDto : SecurityDto
    {
    }
}
