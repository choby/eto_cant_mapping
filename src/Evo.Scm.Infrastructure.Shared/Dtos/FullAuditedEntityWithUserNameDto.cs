using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;

namespace Evo.Scm.Dtos;

/// <summary>
/// Implements <see cref="IFullAuditedObject{TUser}"/> to be a base class for full-audited entities.
/// </summary>
/// <typeparam name="TUser">Type of the user</typeparam>
[Serializable]
public abstract class FullAuditedEntityWithUserNameDto : FullAuditedEntityDto, IFullAuditedObject
{
    /// <summary>
    /// 删除人名字
    /// </summary>
    public virtual string? DeleterName { get; set; }

    /// <summary>
    /// 创建人名字
    /// </summary>
    public virtual string? CreatorName { get; set; }

    /// <summary>
    /// 修改人名字
    /// </summary>
    public virtual string? LastModifierName { get; set; }
}

/// <summary>
/// Implements <see cref="IFullAuditedObjectObject{TUser}"/> to be a base class for full-audited entities.
/// </summary>
/// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
/// <typeparam name="TUser">Type of the user</typeparam>
[Serializable]
public abstract class FullAuditedEntityWithUserNameDto<TKey> : FullAuditedEntityDto<TKey>, IFullAuditedObject
{
    /// <summary>
    /// 删除人名字
    /// </summary>
    public virtual string? DeleterName { get; set; }

    /// <summary>
    /// 创建人名字
    /// </summary>
    public virtual string? CreatorName { get; set; }

    /// <summary>
    /// 修改人名字
    /// </summary>
    public virtual string? LastModifierName { get; set; }
}
