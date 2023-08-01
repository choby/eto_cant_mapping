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
public abstract class CreationAuditedEntityWithUserName: CreationAuditedEntity, IMayHaveCreatorName
{
    /// <summary>
    /// 创建人名字
    /// </summary>
    public virtual string? CreatorName { get; protected set; }
}

public abstract class CreationAuditedEntityWithUserName<TKey> : CreationAuditedEntity<TKey>, IMayHaveCreatorName
{
    

    /// <summary>
    /// 创建人名字
    /// </summary>
    public virtual string? CreatorName { get; protected set; }
}
