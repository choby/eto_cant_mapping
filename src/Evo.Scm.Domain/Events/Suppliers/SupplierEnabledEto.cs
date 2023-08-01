using Evo.Scm.Suppliers;
using System;
using System.Collections.Generic;

namespace Evo.Scm.Events
{
    /// <summary>
    /// 供应商账号开启
    /// </summary>
    public class SupplierEnabledEto
    {
        public SupplierEnabledEto(Supplier supplier,
            IEnumerable<Guid> roleIds)
        {
            Supplier = supplier;
            RoleIds = roleIds;
        }
        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier { get; private set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public IEnumerable<Guid> RoleIds { get; private set; }
    }
}
