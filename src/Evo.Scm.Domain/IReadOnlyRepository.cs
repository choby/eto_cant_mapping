using Volo.Abp.Domain.Entities;


namespace Evo.Scm;

public interface IReadOnlyRepository<TEntity, TKey> : INoTrackingRepository<TEntity, TKey>
    where TEntity : class, IEntity
{
    

}

public interface IReadOnlyRepository<TEntity> : INoTrackingRepository<TEntity>
    where TEntity : class, IEntity
{
    
}
