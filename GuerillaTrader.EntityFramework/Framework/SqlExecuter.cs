using Abp.Dependency;
using Abp.EntityFramework;
using GuerillaTrader.EntityFramework;
using GuerillaTrader.Shared.SqlExecuter;

namespace GuerillaTrader.Framework
{
    public class SqlExecuter : ISqlExecuter, ITransientDependency
    {
        private readonly IDbContextProvider<GuerillaTraderDbContext> _dbContextProvider;

        public SqlExecuter(IDbContextProvider<GuerillaTraderDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public int Execute(string sql, params object[] parameters)
        {
            return _dbContextProvider.GetDbContext().Database.ExecuteSqlCommand(sql, parameters);
        }
    }
}
