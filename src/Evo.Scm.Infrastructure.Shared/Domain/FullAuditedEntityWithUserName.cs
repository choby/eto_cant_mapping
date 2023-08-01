using Evo.Scm.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Evo.Scm.Domain
{
    public abstract class FullAuditedEntityWithUserName : FullAuditedEntity, IMayHaveCreatorName, IDeletionAuditedObjectWithUserName, IModificationAuditedObjectWithUserName
    {
        /// <summary>
        /// 删除人名字
        /// </summary>
        public virtual string? DeleterName { get; set; }

        /// <summary>
        /// 创建人名字
        /// </summary>
        public virtual string? CreatorName { get; protected set; }

        /// <summary>
        /// 修改人名字
        /// </summary>
        public virtual string? LastModifierName { get; set; }
    }

    public abstract class FullAuditedEntityWithUserName<TKey> : FullAuditedEntity<TKey>, IMayHaveCreatorName, IDeletionAuditedObjectWithUserName, IModificationAuditedObjectWithUserName
    {
        /// <summary>
        /// 删除人名字
        /// </summary>
        public virtual string? DeleterName { get; set; }

        /// <summary>
        /// 创建人名字
        /// </summary>
        public virtual string? CreatorName { get; protected set; }

        /// <summary>
        /// 修改人名字
        /// </summary>
        public virtual string? LastModifierName { get; set; }
    }
}
