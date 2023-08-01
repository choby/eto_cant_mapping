
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.EntityHistory;
using Volo.Abp.Guids;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Reflection;
using Volo.Abp.Uow;

namespace Evo.Scm.EntityFrameworkCore;

/// <summary>
/// 自定义批量操作实现
/// </summary>
public class BulkOperationProvider : IBulkOperationProvider, ITransientDependency
{
    IAuditPropertySetter AuditPropertySetter;
    IAuditingManager auditingManager;
    IJsonSerializer jsonSerializer;
    IEntityHistoryHelper entityHistoryHelper;
    IGuidGenerator guidGenerator;
    IEntityChangeEventHelper entityChangeEventHelper;
    IUnitOfWorkManager unitOfWorkManager;
    public BulkOperationProvider(IAuditPropertySetter auditPropertySetter, IJsonSerializer jsonSerializer, IAuditingManager auditingManager, IEntityHistoryHelper entityHistoryHelper, IGuidGenerator guidGenerator, IEntityChangeEventHelper entityChangeEventHelper, IUnitOfWorkManager unitOfWorkManager)
    {
        AuditPropertySetter = auditPropertySetter;
        this.jsonSerializer = jsonSerializer;
        this.auditingManager = auditingManager;

        this.entityHistoryHelper = entityHistoryHelper;
        this.guidGenerator = guidGenerator;
        this.entityChangeEventHelper = entityChangeEventHelper;
        this.unitOfWorkManager = unitOfWorkManager;
    }

    public async Task DeleteManyAsync<TEntity>(IRepository<TEntity> repository, IEnumerable<TEntity> entities, bool autoSave, CancellationToken cancellationToken) where  TEntity : class, IEntity
    {
        var context = await repository.GetDbContextAsync();

        var auditLog = auditingManager?.Current?.Log;
        List<EntityEntry<TEntity>> entries = new();
        List<EntityChangeInfo> entityChangeList = new List<EntityChangeInfo>();

        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            foreach (var  entity in entities)
            {
                //((ISoftDelete)entity).IsDeleted = true;
                ObjectHelper.TrySetProperty(entity.As<ISoftDelete>(), x => x.IsDeleted, () => true);
                var entry = context.Entry(entity);
                entries.Add(entry);
                PublishEventsForTrackedEntity(entry, EntityState.Modified);

                if (auditLog != null)
                {
                    entityChangeList.Add(CreateEntityChangeOrNull(entry, EntityState.Modified));
                }
            }
            await context.BulkUpdateAsync(entities.ToList());
        }
        else
        {

            if (auditLog != null)
            {
                foreach (var entity in entities)
                {
                    var entry = context.Entry(entity);
                    entries.Add(entry);
                    PublishEventsForTrackedEntity(entry, EntityState.Deleted);
                    entityChangeList.Add(CreateEntityChangeOrNull(entry, EntityState.Deleted));
                }
            }
            await context.BulkDeleteAsync(entities.ToList());
        }

        var eventReport = CreateEventReport(entries);

        if (autoSave)
            await context.SaveChangesAsync(cancellationToken: cancellationToken);

        PublishEntityEvents(eventReport);

        if (entityChangeList.Count > 0 && auditLog != null)
        {
            entityHistoryHelper.UpdateChangeList(entityChangeList);
            auditLog.EntityChanges.AddRange(entityChangeList);

        }
    }

    public async Task InsertManyAsync<TEntity>(IRepository<TEntity> repository, IEnumerable<TEntity> entities, bool autoSave, CancellationToken cancellationToken) where TEntity : class, IEntity
    {
        var context = await repository.GetDbContextAsync();
        var auditLog = auditingManager?.Current?.Log;

        List<EntityChangeInfo> entityChangeList = new List<EntityChangeInfo>();
        List<EntityEntry<TEntity>> entries = new();
        foreach (var entitie in entities)
        {
            var entry = context.Entry(entitie);
            entries.Add(entry);
            PublishEventsForTrackedEntity(entry, EntityState.Added);
            if (auditLog != null)
            {
                entityChangeList.Add(CreateEntityChangeOrNull(entry, EntityState.Added));
            }
        }


        await context.BulkInsertAsync(entities.ToList());

        var eventReport = CreateEventReport(entries);

        if (autoSave)
            await context.SaveChangesAsync(cancellationToken: cancellationToken);

        PublishEntityEvents(eventReport);

        if (entityChangeList.Count > 0 && auditLog != null)
        {
            entityHistoryHelper.UpdateChangeList(entityChangeList);
            auditLog.EntityChanges.AddRange(entityChangeList);
        }


    }


    public async Task UpdateManyAsync<TEntity>(IRepository<TEntity> repository, IEnumerable<TEntity> entities, bool autoSave, CancellationToken cancellationToken) where TEntity : class, IEntity
    {
        var context = await repository.GetDbContextAsync();
        var auditLog = auditingManager?.Current?.Log;
        List<EntityEntry<TEntity>> entries = new();
        List<EntityChangeInfo> entityChangeList = new List<EntityChangeInfo>();
        foreach (var entity in entities)
        {
            var entry = context.Entry(entity);
            entries.Add(entry);
            if (auditLog != null)
            {
                entityChangeList.Add(CreateEntityChangeOrNull(entry, EntityState.Modified));
            }
            PublishEventsForTrackedEntity(entry, EntityState.Modified);

        }

        await context.BulkUpdateAsync(entities.ToList());

        var eventReport = CreateEventReport(entries);

        if (autoSave)
            await context.SaveChangesAsync(cancellationToken: cancellationToken);

        PublishEntityEvents(eventReport);

        if (entityChangeList.Count > 0 && auditLog != null)
        {
            entityHistoryHelper.UpdateChangeList(entityChangeList);
            auditLog.EntityChanges.AddRange(entityChangeList);

        }
    }

    #region 事件
    EntityEventReport CreateEventReport(IEnumerable<EntityEntry> entries)
    {
        var eventReport = new EntityEventReport();

        foreach (var entry in entries.ToList())
        {
            var generatesDomainEventsEntity = entry.Entity as IGeneratesDomainEvents;
            if (generatesDomainEventsEntity == null)
            {
                continue;
            }

            var localEvents = generatesDomainEventsEntity.GetLocalEvents()?.ToArray();
            if (localEvents != null && localEvents.Any())
            {
                eventReport.DomainEvents.AddRange(
                    localEvents.Select(
                        eventRecord => new DomainEventEntry(
                            entry.Entity,
                            eventRecord.EventData,
                            eventRecord.EventOrder
                        )
                    )
                );
                generatesDomainEventsEntity.ClearLocalEvents();
            }

            var distributedEvents = generatesDomainEventsEntity.GetDistributedEvents()?.ToArray();
            if (distributedEvents != null && distributedEvents.Any())
            {
                eventReport.DistributedEvents.AddRange(
                    distributedEvents.Select(
                        eventRecord => new DomainEventEntry(
                            entry.Entity,
                            eventRecord.EventData,
                            eventRecord.EventOrder)
                    )
                );
                generatesDomainEventsEntity.ClearDistributedEvents();
            }
        }

        return eventReport;
    }

    private void PublishEntityEvents(EntityEventReport changeReport)
    {
        foreach (var localEvent in changeReport.DomainEvents)
        {
            unitOfWorkManager.Current?.AddOrReplaceLocalEvent(
                new UnitOfWorkEventRecord(localEvent.EventData.GetType(), localEvent.EventData, localEvent.EventOrder)
            );
        }

        foreach (var distributedEvent in changeReport.DistributedEvents)
        {
            unitOfWorkManager.Current?.AddOrReplaceDistributedEvent(
                new UnitOfWorkEventRecord(distributedEvent.EventData.GetType(), distributedEvent.EventData, distributedEvent.EventOrder)
            );
        }
    }

    private void PublishEventsForTrackedEntity(EntityEntry entry, EntityState entityState)
    {
        switch (entityState)
        {
            case EntityState.Added:
                ApplyAbpConceptsForAddedEntity(entry);
                //entityChangeEventHelper.PublishEntityCreatingEvent(entry.Entity);
                entityChangeEventHelper.PublishEntityCreatedEvent(entry.Entity);
                break;
            case EntityState.Modified:
                if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
                {
                    ApplyAbpConceptsForDeletedEntity(entry);
                }
                else
                {
                    ApplyAbpConceptsForModifiedEntity(entry);
                }

                if (entry.Properties.Any(x => x.IsModified && x.Metadata.ValueGenerated == ValueGenerated.Never))
                {
                    if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
                    {
                        //entityChangeEventHelper.PublishEntityDeletingEvent(entry.Entity);
                        entityChangeEventHelper.PublishEntityDeletedEvent(entry.Entity);
                    }
                    else
                    {
                        //entityChangeEventHelper.PublishEntityUpdatingEvent(entry.Entity);
                        entityChangeEventHelper.PublishEntityUpdatedEvent(entry.Entity);
                    }
                }

                break;
            case EntityState.Deleted:
                //entityChangeEventHelper.PublishEntityDeletingEvent(entry.Entity);
                entityChangeEventHelper.PublishEntityDeletedEvent(entry.Entity);
                break;
        }
    }

    #endregion

    #region 默认值
    private void ApplyAbpConceptsForAddedEntity(EntityEntry entry)
    {
        CheckAndSetId(entry);
        SetConcurrencyStampIfNull(entry);
        SetCreationAuditProperties(entry);
    }

    void SetConcurrencyStampIfNull(EntityEntry entry)
    {
        var entity = entry.Entity as IHasConcurrencyStamp;
        if (entity == null)
        {
            return;
        }

        if (entity.ConcurrencyStamp != null)
        {
            return;
        }

        entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    void SetCreationAuditProperties(EntityEntry entry)
    {
        AuditPropertySetter?.SetCreationProperties(entry.Entity);
    }

    private void CheckAndSetId(EntityEntry entry)
    {
        if (entry.Entity is IEntity<Guid> entityWithGuidId)
        {
            TrySetGuidId(entry, entityWithGuidId);
        }
    }
    private void TrySetGuidId(EntityEntry entry, IEntity<Guid> entity)
    {
        if (entity.Id != default)
        {
            return;
        }

        var idProperty = entry.Property("Id").Metadata.PropertyInfo;

        //Check for DatabaseGeneratedAttribute
        var dbGeneratedAttr = ReflectionHelper
            .GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(
                idProperty
            );

        if (dbGeneratedAttr != null && dbGeneratedAttr.DatabaseGeneratedOption != DatabaseGeneratedOption.None)
        {
            return;
        }

        EntityHelper.TrySetId(
            entity,
            () => guidGenerator.Create(),
            true
        );
    }

    private void ApplyAbpConceptsForModifiedEntity(EntityEntry entry)
    {
        if (entry.State == EntityState.Modified && entry.Properties.Any(x => x.IsModified && x.Metadata.ValueGenerated == ValueGenerated.Never))
        {
            SetModificationAuditProperties(entry);

            if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
            {
                SetDeletionAuditProperties(entry);
            }
        }
    }

    private void SetModificationAuditProperties(EntityEntry entry)
    {
        AuditPropertySetter?.SetModificationProperties(entry.Entity);
    }

    private void SetDeletionAuditProperties(EntityEntry entry)
    {
        AuditPropertySetter?.SetDeletionProperties(entry.Entity);
    }


    private void ApplyAbpConceptsForDeletedEntity(EntityEntry entry)
    {
        if (!(entry.Entity is ISoftDelete))
        {
            return;
        }

        //if (IsHardDeleted(entry))
        //{
        //    return;
        //}

        entry.Reload();
        //entry.Entity.As<ISoftDelete>().IsDeleted = true;
        ObjectHelper.TrySetProperty(entry.Entity.As<ISoftDelete>(), x => x.IsDeleted, () => true);
        SetDeletionAuditProperties(entry);
    }
    #endregion

    #region 审计相关
    private EntityChangeInfo CreateEntityChangeOrNull(EntityEntry entityEntry, EntityState entityState)
    {
        var entity = entityEntry.Entity;

        EntityChangeType changeType;
        switch (entityState)
        {
            case EntityState.Added:
                changeType = EntityChangeType.Created;
                break;
            case EntityState.Deleted:
                changeType = EntityChangeType.Deleted;
                break;
            case EntityState.Modified:
                changeType = IsSoftDeleted(entityEntry) ? EntityChangeType.Deleted : EntityChangeType.Updated;
                break;
            case EntityState.Detached:
            case EntityState.Unchanged:
            default:
                return null;
        }

        var entityId = GetEntityId(entity);
        if (entityId == null && changeType != EntityChangeType.Created)
        {
            return null;
        }

        var entityType = entity.GetType();
        var entityChange = new EntityChangeInfo
        {
            ChangeType = changeType,
            EntityEntry = entityEntry,
            EntityId = entityId,
            EntityTypeFullName = entityType.FullName,
            PropertyChanges = GetPropertyChanges(entityEntry, entityState),
            EntityTenantId = GetTenantId(entity)
        };

        return entityChange;
    }

    private bool IsSoftDeleted(EntityEntry entityEntry)
    {
        var entity = entityEntry.Entity;
        return entity is ISoftDelete && entity.As<ISoftDelete>().IsDeleted;
    }

    private string GetEntityId(object entityAsObj)
    {
        if (!(entityAsObj is IEntity entity))
        {
            throw new AbpException($"Entities should implement the {typeof(IEntity).AssemblyQualifiedName} interface! Given entity does not implement it: {entityAsObj.GetType().AssemblyQualifiedName}");
        }

        var keys = entity.GetKeys();
        if (keys.All(k => k == null))
        {
            return null;
        }

        return keys.JoinAsString(",");
    }

    private List<EntityPropertyChangeInfo> GetPropertyChanges(EntityEntry entityEntry, EntityState entityState)
    {
        var propertyChanges = new List<EntityPropertyChangeInfo>();
        var properties = entityEntry.Metadata.GetProperties();
        var isCreated = IsCreated(entityState);
        var isDeleted = IsDeleted(entityEntry, entityState);

        foreach (var property in properties)
        {
            var propertyEntry = entityEntry.Property(property.Name);
            if (ShouldSavePropertyHistory(propertyEntry, isCreated || isDeleted) && !IsSoftDeleted(entityEntry))
            {
                propertyChanges.Add(new EntityPropertyChangeInfo
                {
                    NewValue = isDeleted ? null : jsonSerializer.Serialize(propertyEntry.CurrentValue).TruncateWithPostfix(EntityPropertyChangeInfo.MaxValueLength),
                    OriginalValue = isCreated ? null : jsonSerializer.Serialize(propertyEntry.OriginalValue).TruncateWithPostfix(EntityPropertyChangeInfo.MaxValueLength),
                    PropertyName = property.Name,
                    PropertyTypeFullName = property.ClrType.GetFirstGenericArgumentIfNullable().FullName
                });
            }
        }

        return propertyChanges;
    }

    private bool ShouldSavePropertyHistory(PropertyEntry propertyEntry, bool defaultValue)
    {
        if (propertyEntry.Metadata.Name == "Id")
        {
            return false;
        }

        var propertyInfo = propertyEntry.Metadata.PropertyInfo;
        if (propertyInfo != null && propertyInfo.IsDefined(typeof(DisableAuditingAttribute), true))
        {
            return false;
        }

        var entityType = propertyEntry.EntityEntry.Entity.GetType();
        if (entityType.IsDefined(typeof(DisableAuditingAttribute), true))
        {
            if (propertyInfo == null || !propertyInfo.IsDefined(typeof(AuditedAttribute), true))
            {
                return false;
            }
        }

        if (propertyInfo != null && IsBaseAuditProperty(propertyInfo, entityType))
        {
            return false;
        }

        if (propertyEntry.OriginalValue is ExtraPropertyDictionary originalValue && propertyEntry.CurrentValue is ExtraPropertyDictionary currentValue)
        {
            if (originalValue.IsNullOrEmpty() && currentValue.IsNullOrEmpty())
            {
                return false;
            }

            if (!originalValue.Select(x => x.Key).SequenceEqual(currentValue.Select(x => x.Key)))
            {
                return true;
            }

            if (!originalValue.Select(x => x.Value).SequenceEqual(currentValue.Select(x => x.Value)))
            {
                return true;
            }

            return defaultValue;
        }

        var isModified = !(propertyEntry.OriginalValue?.Equals(propertyEntry.CurrentValue) ?? propertyEntry.CurrentValue == null);
        if (isModified)
        {
            return true;
        }

        return defaultValue;
    }

    private Guid? GetTenantId(object entity)
    {
        if (!(entity is IMultiTenant multiTenantEntity))
        {
            return null;
        }

        return multiTenantEntity.TenantId;
    }

    private bool IsBaseAuditProperty(PropertyInfo propertyInfo, Type entityType)
    {
        if (entityType.IsAssignableTo<IHasCreationTime>()
            && propertyInfo.Name == nameof(IHasCreationTime.CreationTime))
        {
            return true;
        }

        if (entityType.IsAssignableTo<IMayHaveCreator>()
            && propertyInfo.Name == nameof(IMayHaveCreator.CreatorId))
        {
            return true;
        }

        if (entityType.IsAssignableTo<IMustHaveCreator>()
            && propertyInfo.Name == nameof(IMustHaveCreator.CreatorId))
        {
            return true;
        }

        if (entityType.IsAssignableTo<IHasModificationTime>()
            && propertyInfo.Name == nameof(IHasModificationTime.LastModificationTime))
        {
            return true;
        }

        if (entityType.IsAssignableTo<IModificationAuditedObject>()
            && propertyInfo.Name == nameof(IModificationAuditedObject.LastModifierId))
        {
            return true;
        }

        if (entityType.IsAssignableTo<ISoftDelete>()
            && propertyInfo.Name == nameof(ISoftDelete.IsDeleted))
        {
            return true;
        }

        if (entityType.IsAssignableTo<IHasDeletionTime>()
            && propertyInfo.Name == nameof(IHasDeletionTime.DeletionTime))
        {
            return true;
        }

        if (entityType.IsAssignableTo<IDeletionAuditedObject>()
            && propertyInfo.Name == nameof(IDeletionAuditedObject.DeleterId))
        {
            return true;
        }

        return false;
    }

    private bool IsCreated(EntityState entityState)
    {
        return entityState == EntityState.Added;
    }

    private bool IsDeleted(EntityEntry entityEntry, EntityState entityState)
    {
        return entityState == EntityState.Deleted || IsSoftDeleted(entityEntry);
    }

    #endregion
}
