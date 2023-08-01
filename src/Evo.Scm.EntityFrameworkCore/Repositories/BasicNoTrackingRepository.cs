using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

namespace Evo.Scm;

public class BasicNoTrackingRepository<TDbContext,TEntity, TKey>
   : EfCoreRepository<TDbContext, TEntity, TKey>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity<TKey>
{
    public BasicNoTrackingRepository(IDbContextProvider<TDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
        
    }

    public async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
    {
        return (await FindAsync(id, GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false)) ?? throw new EntityNotFoundException(typeof(TEntity), id);
    }

    public async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return (await FindAsync(id, GetCancellationToken(cancellationToken), propertySelectors).ConfigureAwait(continueOnCapturedContext: false)) ?? throw new EntityNotFoundException(typeof(TEntity), id);
    }
    public async Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
    {
        return (await (await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking().SingleOrDefaultAsync(t => t.Id.Equals(id), cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
    }

    public async Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return !(propertySelectors != null && propertySelectors.Length > 0) ?
            (await (await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking().SingleOrDefaultAsync(t => t.Id.Equals(id)).ConfigureAwait(continueOnCapturedContext: false)) :
            (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync((await WithDetailsAsync(propertySelectors).ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking().OrderBy((TEntity e) => e.Id), (TEntity e) => e.Id.Equals(id), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false));
    }

    public async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        return (await EntityFrameworkQueryableExtensions.ToListAsync((await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking(), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false));
            
    }
    public async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return !(propertySelectors != null && propertySelectors.Length > 0) ?
            (await EntityFrameworkQueryableExtensions.ToListAsync((await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking(), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false)) :
            (await EntityFrameworkQueryableExtensions.ToListAsync((await WithDetailsAsync(propertySelectors).ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking(), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false));
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
    {
        return (await EntityFrameworkQueryableExtensions.ToListAsync(((await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking()).Where(predicate), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false)) ;
           
    }
    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return !(propertySelectors != null && propertySelectors.Length > 0) ?
            (await EntityFrameworkQueryableExtensions.ToListAsync(((await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking()).Where(predicate), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false)) :
            (await EntityFrameworkQueryableExtensions.ToListAsync((await WithDetailsAsync(propertySelectors).ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking().Where(predicate), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false));
    }

    public async Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default(CancellationToken))
    {
        IQueryable<TEntity> source =  (await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking() ;
        return await EntityFrameworkQueryableExtensions.ToListAsync(source.OrderBy(sorting).PageBy(skipCount, maxResultCount), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false);
    }
    public async Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        IQueryable<TEntity> source = (!(propertySelectors != null && propertySelectors.Length > 0) ?
            (await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking() :
            (await WithDetailsAsync(propertySelectors).ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking());
        return await EntityFrameworkQueryableExtensions.ToListAsync(source.OrderBy(sorting).PageBy(skipCount, maxResultCount), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
    {
        return (await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync((await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking().Where(predicate), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false)) ;
    }
    public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return !(propertySelectors != null && propertySelectors.Length > 0) ?
            (await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync((await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking().Where(predicate), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false)) :
            (await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync((await WithDetailsAsync(propertySelectors).ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking().Where(predicate), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false));
    }

    public async Task<IQueryable<TEntity>> GetQueryableAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return IncludeDetails((await base.GetQueryableAsync()).AsNoTracking(), propertySelectors);
    }

    public async Task<IQueryable<TEntity>> GetQueryableAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return IncludeDetails((await base.GetQueryableAsync()).Where(predicate).AsNoTracking(), propertySelectors);
    }

    private static IQueryable<TEntity> IncludeDetails(IQueryable<TEntity> query, Expression<Func<TEntity, object>>[] propertySelectors)
    {
        if (!propertySelectors.IsNullOrEmpty())
        {
            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }
        }

        return query;
    }

    
}


public class BasicNoTrackingRepository<TDbContext,TEntity>
   : EfCoreRepository<TDbContext, TEntity>
    where TDbContext : IEfCoreDbContext
     where TEntity : class, IEntity
{
    public BasicNoTrackingRepository(IDbContextProvider<TDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    //public async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
    //{
    //    return (await FindAsync(id, GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false)) ?? throw new EntityNotFoundException(typeof(TEntity), id);
    //}

    //public async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors)
    //{
    //    return (await FindAsync(id, GetCancellationToken(cancellationToken), propertySelectors).ConfigureAwait(continueOnCapturedContext: false)) ?? throw new EntityNotFoundException(typeof(TEntity), id);
    //}
    //public async Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
    //{
    //    return (await (await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking().SingleOrDefaultAsync(t => t.Id.Equals(id), cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
    //}

    //public async Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors)
    //{
    //    return !(propertySelectors != null && propertySelectors.Length > 0) ?
    //        (await (await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking().SingleOrDefaultAsync(t => t.Id.Equals(id)).ConfigureAwait(continueOnCapturedContext: false)) :
    //        (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync((await WithDetailsAsync(propertySelectors).ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking().OrderBy((TEntity e) => e.Id), (TEntity e) => e.Id.Equals(id), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false));
    //}

    public async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        return (await EntityFrameworkQueryableExtensions.ToListAsync((await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking(), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false));

    }
    public async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return !(propertySelectors != null && propertySelectors.Length > 0) ?
            (await EntityFrameworkQueryableExtensions.ToListAsync((await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking(), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false)) :
            (await EntityFrameworkQueryableExtensions.ToListAsync((await WithDetailsAsync(propertySelectors).ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking(), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false));
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
    {
        return (await EntityFrameworkQueryableExtensions.ToListAsync(((await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking()).Where(predicate), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false));

    }
    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return !(propertySelectors != null && propertySelectors.Length > 0) ?
            (await EntityFrameworkQueryableExtensions.ToListAsync(((await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking()).Where(predicate), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false)) :
            (await EntityFrameworkQueryableExtensions.ToListAsync((await WithDetailsAsync(propertySelectors).ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking().Where(predicate), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false));
    }

    public async Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default(CancellationToken))
    {
        IQueryable<TEntity> source = (await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking();
        return await EntityFrameworkQueryableExtensions.ToListAsync(source.OrderBy(sorting).PageBy(skipCount, maxResultCount), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false);
    }
    public async Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        IQueryable<TEntity> source = (!(propertySelectors != null && propertySelectors.Length > 0) ?
            (await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking() :
            (await WithDetailsAsync(propertySelectors).ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking());
        return await EntityFrameworkQueryableExtensions.ToListAsync(source.OrderBy(sorting).PageBy(skipCount, maxResultCount), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
    {
        return (await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync((await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking().Where(predicate), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false));
    }
    public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return !(propertySelectors != null && propertySelectors.Length > 0) ?
            (await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync((await GetDbSetAsync().ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking().Where(predicate), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false)) :
            (await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync((await WithDetailsAsync(propertySelectors).ConfigureAwait(continueOnCapturedContext: false)).AsNoTracking().Where(predicate), GetCancellationToken(cancellationToken)).ConfigureAwait(continueOnCapturedContext: false));
    }

    public async Task<IQueryable<TEntity>> GetQueryableAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return IncludeDetails((await base.GetQueryableAsync()).AsNoTracking(), propertySelectors);
    }

    public async Task<IQueryable<TEntity>> GetQueryableAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return IncludeDetails((await base.GetQueryableAsync()).Where(predicate).AsNoTracking(), propertySelectors);
    }

    private static IQueryable<TEntity> IncludeDetails(IQueryable<TEntity> query, Expression<Func<TEntity, object>>[] propertySelectors)
    {
        if (!propertySelectors.IsNullOrEmpty())
        {
            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }
        }

        return query;
    }


}



