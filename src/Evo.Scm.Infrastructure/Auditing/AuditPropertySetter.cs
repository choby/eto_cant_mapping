using Evo.Scm.Domain;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace Evo.Scm.Auditing
{
    /// <summary>
    /// 替换abp的审计赋值功能,增加创建人名字, 修改人名字,删除人名字赋值
    /// </summary>
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IAuditPropertySetter))]
    public class AuditPropertySetter : Volo.Abp.Auditing.AuditPropertySetter, IAuditPropertySetter, ITransientDependency
    {
        public AuditPropertySetter(
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock):base(currentUser, currentTenant, clock)
        {
           
        }

        public override void SetCreationProperties(object targetObject)
        {
            SetCreationTime(targetObject);
            SetCreatorId(targetObject);
            SetCreatorName(targetObject);
        }

        public override void SetModificationProperties(object targetObject)
        {
            SetLastModificationTime(targetObject);
            SetLastModifierId(targetObject);
            SetLastModifierName(targetObject);
        }

        public override void SetDeletionProperties(object targetObject)
        {
            SetDeletionTime(targetObject);
            SetDeleterId(targetObject);
            SetDeleterName(targetObject);
        }

       

        private void SetCreatorName(object targetObject)
        {
            if (string.IsNullOrEmpty(CurrentUser.Name))
            {
                return;
            }

            if (targetObject is IMultiTenant multiTenantEntity)
            {
                if (multiTenantEntity.TenantId != CurrentUser.TenantId)
                {
                    return;
                }
            }

            /* TODO: The code below is from old ABP, not implemented yet
                if (tenantId.HasValue && MultiTenancyHelper.IsHostEntity(entity))
                {
                    //Tenant user created a host entity
                    return;
                }
                 */

            if (targetObject is IMayHaveCreatorName mayHaveCreatorObject)
            {
                if (!string.IsNullOrEmpty(mayHaveCreatorObject.CreatorName) && mayHaveCreatorObject.CreatorName != default)
                {
                    return;
                }

                ObjectHelper.TrySetProperty(mayHaveCreatorObject, x => x.CreatorName, () => CurrentUser.Name);
            }
        }


        private void SetLastModifierName(object targetObject)
        {
            if (!(targetObject is IModificationAuditedObjectWithUserName modificationAuditedObject))
            {
                return;
            }

            if (string.IsNullOrEmpty(CurrentUser.Name))
            {
                modificationAuditedObject.LastModifierName = null;
                return;
            }

            if (modificationAuditedObject is IMultiTenant multiTenantEntity)
            {
                if (multiTenantEntity.TenantId != CurrentUser.TenantId)
                {
                    modificationAuditedObject.LastModifierName = null;
                    return;
                }
            }

            /* TODO: The code below is from old ABP, not implemented yet
            if (tenantId.HasValue && MultiTenancyHelper.IsHostEntity(entity))
            {
                //Tenant user modified a host entity
                modificationAuditedObject.LastModifierId = null;
                return;
            }
             */

            modificationAuditedObject.LastModifierName = CurrentUser.Name;
        }


        private void SetDeleterName(object targetObject)
        {
            if (!(targetObject is IDeletionAuditedObjectWithUserName modificationAuditedObject))
            {
                return;
            }

            if (string.IsNullOrEmpty(CurrentUser.Name))
            {
                modificationAuditedObject.DeleterName = null;
                return;
            }

            if (modificationAuditedObject is IMultiTenant multiTenantEntity)
            {
                if (multiTenantEntity.TenantId != CurrentUser.TenantId)
                {
                    modificationAuditedObject.DeleterName = null;
                    return;
                }
            }

            /* TODO: The code below is from old ABP, not implemented yet
            if (tenantId.HasValue && MultiTenancyHelper.IsHostEntity(entity))
            {
                //Tenant user modified a host entity
                modificationAuditedObject.LastModifierId = null;
                return;
            }
             */

            modificationAuditedObject.DeleterName = CurrentUser.Name;
        }
    }
}
