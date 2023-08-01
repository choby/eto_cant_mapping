using Evo.Scm.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;

namespace Evo.Scm.Dtos;

[Serializable]
public abstract class AuditedEntityWithUserNameDto: AuditedEntityDto
{
    /// <summary>
    /// 创建人名字
    /// </summary>
    public virtual string? CreatorName { get; set; }

    /// <summary>
    /// 修改人名字
    /// </summary>
    public virtual string? LastModifierName { get; set; }
}

public abstract class AuditedEntityWithUserNameDto<TKey> : AuditedEntityDto<TKey>
{
    

    /// <summary>
    /// 创建人名字
    /// </summary>
    public virtual string? CreatorName { get; set; }

    /// <summary>
    /// 修改人名字
    /// </summary>
    public virtual string? LastModifierName { get; set; }
}
