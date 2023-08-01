using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Evo.Scm.EntityFrameworkCore;

namespace Evo.Scm;

public class NoTrackingRepository<TEntity, TKey>
   : BasicNoTrackingRepository<ScmDbContext, TEntity, TKey>, INoTrackingRepository<TEntity, TKey>
     where TEntity : class, IEntity<TKey>
{
    public NoTrackingRepository(IDbContextProvider<ScmDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
        
    }
}

public class NoTrackingRepository<TEntity>
   : BasicNoTrackingRepository<ScmDbContext, TEntity>, INoTrackingRepository<TEntity>
     where TEntity : class, IEntity
{
    public NoTrackingRepository(IDbContextProvider<ScmDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }
}
