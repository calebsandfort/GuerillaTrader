using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Entities.Dtos
{
    public class GenerateStockReportsInput
    {
        [DataType(DataType.Date)]
        [UIHint("MyDate")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [UIHint("MyDate")]
        public DateTime EndDate { get; set; }

        [UIHint("MyInt")]
        public int Lookback { get; set; }

        public int Period
        {
            get
            {
                return (this.EndDate - this.StartDate).Days;
            }
        }
    }
}
