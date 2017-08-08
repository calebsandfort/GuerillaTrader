using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Shared.SqlExecuter
{
    public interface ISqlExecuter
    {
        int Execute(string sql, params object[] parameters);
    }
}
