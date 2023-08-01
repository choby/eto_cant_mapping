using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;

namespace Evo.Scm;

public interface INoTrackingRepository<TEntity, TKey> : INoTrackingRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken));
    Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors);

    Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
    Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors);

    Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken));
    Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors);

    Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default(CancellationToken));
    Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors);



    Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default(CancellationToken));
    Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors);



    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertySelectors);


}

public interface INoTrackingRepository<TEntity> : Volo.Abp.Domain.Repositories.IReadOnlyRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<IQueryable<TEntity>> GetQueryableAsync(params Expression<Func<TEntity, object>>[] propertySelectors);
    Task<IQueryable<TEntity>> GetQueryableAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors);
}
