using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Entities.Dtos
{
    public class EntityDtoBase : EntityDto
    {
        public bool IsNew
        {
            get
            {
                return this.Id == 0;
            }
        }
    }
}
