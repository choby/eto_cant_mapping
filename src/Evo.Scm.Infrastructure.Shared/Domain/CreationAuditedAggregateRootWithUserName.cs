using System;
using Evo.Scm.Auditing;
using Volo.Abp.Auditing;

namespace Evo.Scm.Domain;

/// <summary>
/// This class can be used to simplify implementing <see cref="ICreationAuditedObject"/> for aggregate roots.
/// </summary>
[Serializable]
public abstract class CreationAuditedAggregateRootWithUserName : CreationAuditedAggregateRoot, IMayHaveCreatorName
{
    /// <summary>
    /// 创建人名字
    /// </summary>
    public virtual string? CreatorName { get; protected set; }
}

/// <summary>
/// This class can be used to simplify implementing <see cref="ICreationAuditedObject"/> for aggregate roots.
/// </summary>
/// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
[Serializable]
public abstract class CreationAuditedAggregateRootWithUserName<TKey> : CreationAuditedAggregateRoot<TKey>, IMayHaveCreatorName
{
    /// <summary>
    /// 创建人名字
    /// </summary>
    public virtual string? CreatorName { get; protected set; }

    protected CreationAuditedAggregateRootWithUserName()
    {

    }

    protected CreationAuditedAggregateRootWithUserName(TKey id)
        : base(id)
    {

    }
}
