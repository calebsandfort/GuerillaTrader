using GuerillaTrader.Entities.Dtos;
using GuerillaTrader.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using System.IO;
using GuerillaTrader.Shared;

namespace GuerillaTrader.Tests.Shared
{
    public class SharedTests : GuerillaTraderTestBase
    {
        #region ScoreColors
        [Fact]
        public void ScoreColors()
        {
            StreamWriter colorsJs = new StreamWriter("Colors.js", false);
            ColorCalculator cc = new ColorCalculator();
            cc.SetColorScale(ColorScale.Rainbow);

            String val = String.Empty;
            String color = String.Empty;

            colorsJs.WriteLine("GuerillaTrader.Utilities.getScoreColor = function (val){");
            colorsJs.WriteLine("    var color = '';");
            for (Double i = 0.0; i <= 100.0; i += 1.0)
            {
                val = i.ToString("N2");
                color = cc.SetPercentage(i / 100.0);

                colorsJs.WriteLine($"   else if (val <= {i.ToString("N2")}) color = '#{color}'");
            }
            colorsJs.WriteLine("    return color;");
            colorsJs.WriteLine("}");
            colorsJs.Close();
        }
        #endregion
    }
}
