using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;
using Volo.Abp.Caching;
using Evo.Scm.Positions;
using Microsoft.Extensions.Caching.Distributed;
using Evo.Scm.BrandIsolation;

namespace Evo.Scm.Fakes;

/// <summary>
/// 自定义品牌访问器,避免开发人员debug时无品牌信息
/// 仅用于开发环境下的debug模式, 严禁用于生产环境
/// </summary>
[Dependency(ReplaceServices = false, TryRegister = false)]
public class DebugCurrentUserBrandsAccessor : ICurrentUserBrandsAccessor, IScopedDependency
{
    public DebugCurrentUserBrandsAccessor()
    {

       
    }
  
    public Guid[] BrandIds => Array.Empty<Guid>();
}

