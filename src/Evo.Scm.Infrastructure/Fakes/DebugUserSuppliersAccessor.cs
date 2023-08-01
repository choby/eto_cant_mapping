using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;
using Volo.Abp.Caching;
using Evo.Scm.Positions;
using Microsoft.Extensions.Caching.Distributed;
using Evo.Scm.BrandIsolation;
using Evo.Scm.SupplierIsolation;

namespace Evo.Scm.Fakes;

/// <summary>
/// 自定义供应商访问器,避免开发人员debug时无品牌信息
/// 仅用于开发环境下的debug模式, 严禁用于生产环境
/// </summary>
[Dependency(ReplaceServices = false, TryRegister = false)]
public class DebugUserSuppliersAccessor : ICurrentUserSuppliersAccessor, IScopedDependency
{
    

    public DebugUserSuppliersAccessor()
    {
    }
   


    public Guid[] SupplierIds => Array.Empty<Guid>();
}

