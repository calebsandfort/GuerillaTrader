using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Shared.Dtos
{
    public class ConsoleWriteLineInput
    {
        public String Line { get; set; }

        public static ConsoleWriteLineInput Create(String line)
        {
            return new ConsoleWriteLineInput() { Line = line };
        }
    }
}
