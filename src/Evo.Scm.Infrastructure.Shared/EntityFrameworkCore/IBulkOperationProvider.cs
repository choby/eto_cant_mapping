using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Evo.Scm.EntityFrameworkCore;

/// <summary>
/// 自定义批量操作实现
/// </summary>
public interface IBulkOperationProvider 
{
    Task DeleteManyAsync<TEntity>(IRepository<TEntity> repository, IEnumerable<TEntity> entities, bool autoSave, CancellationToken cancellationToken) where TEntity : class, IEntity;
    Task InsertManyAsync<TEntity>(IRepository<TEntity> repository, IEnumerable<TEntity> entities, bool autoSave, CancellationToken cancellationToken) where TEntity : class, IEntity;
    Task UpdateManyAsync<TEntity>(IRepository<TEntity> repository, IEnumerable<TEntity> entities, bool autoSave, CancellationToken cancellationToken) where TEntity : class, IEntity;
}
