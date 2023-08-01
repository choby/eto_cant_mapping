using Evo.Scm.ExceptionHandling;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Evo.Scm.Suppliers
{
    public class SupplierPaymentInfo : Entity<Guid>, ISoftDelete
    {
        /// <summary>
        /// 供应商Id
        /// </summary>
        public Guid SupplierId { get; private set; }
        /// <summary>
        /// 账户类型 0：对公，1对私
        /// </summary>
        public AccountType AccountType { get;  set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string OpeningBank { get; set; }
        /// <summary>
        /// 账户名
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string AccountNo { get; set; }
        /// <summary>
        /// 开启状态
        /// </summary>
        public bool IsEnabled { get; private set; }
        /// <summary>
        /// 默认账户
        /// </summary>
        public bool IsDefault { get; private set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ApproveTime { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string ApproverName { get; set; }
        public bool IsDeleted { get; set; }
        internal void SetId(Guid id)
        {
            if (id == default(Guid))
            {
                throw new BusinessException(ExceptionCodes.请求数据校验失败, $"供应商账户Id不能为空");
            }
            this.Id = id;
        }
        internal void SetSupplierId(Guid supplierId)
        {
            if (supplierId == default(Guid))
            {
                throw new BusinessException(ExceptionCodes.请求数据校验失败, $"供应商Id不能为空");
            }
            this.SupplierId = supplierId;
        }
        
        internal void SetIsDefault(bool isDefault)
        {
            this.IsDefault = isDefault;
        }
        internal void SetIsEnabled(bool isEnabled)
        {
            this.IsEnabled = isEnabled;
        }
    }
}
