using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Entities
{
    public class EntityBase : Entity<int>
    {
        [NotMapped]
        public bool IsNew
        {
            get
            {
                return this.Id == 0;
            }
        }
    }
}
