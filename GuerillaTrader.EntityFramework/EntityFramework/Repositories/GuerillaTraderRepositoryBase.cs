using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace GuerillaTrader.EntityFramework.Repositories
{
    public abstract class GuerillaTraderRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<GuerillaTraderDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected GuerillaTraderRepositoryBase(IDbContextProvider<GuerillaTraderDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class GuerillaTraderRepositoryBase<TEntity> : GuerillaTraderRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected GuerillaTraderRepositoryBase(IDbContextProvider<GuerillaTraderDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
