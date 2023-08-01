using Evo.Scm.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;

namespace Evo.Scm.Domain;

[Serializable]
public abstract class AuditedEntityWithUserName: AuditedEntity, IMayHaveCreatorName, IModificationAuditedObjectWithUserName
{
    /// <summary>
    /// 创建人名字
    /// </summary>
    public virtual string? CreatorName { get; protected set; }

    /// <summary>
    /// 修改人名字
    /// </summary>
    public virtual string? LastModifierName { get; set; }
}

public abstract class AuditedEntityWithUserName<TKey> : AuditedEntity<TKey>, IMayHaveCreatorName, IModificationAuditedObjectWithUserName
{
    

    /// <summary>
    /// 创建人名字
    /// </summary>
    public virtual string? CreatorName { get; protected set; }

    /// <summary>
    /// 修改人名字
    /// </summary>
    public virtual string? LastModifierName { get; set; }
}
