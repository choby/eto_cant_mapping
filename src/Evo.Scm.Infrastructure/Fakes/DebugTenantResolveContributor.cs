using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Evo.Scm.Fakes;

/// <summary>
/// 自定义租户解析器,避免开发人员debug时无租户信息
/// 仅用于开发环境下的debug模式, 严禁用于生产环境
/// </summary>
[Dependency(ReplaceServices = false, TryRegister = false)]

public class DebugTenantResolveContributor : TenantResolveContributorBase
{
    public override string Name => "Debug";

    public override Task ResolveAsync(ITenantResolveContext context)
    {
        context.TenantIdOrName = "3a04305b-6baa-273c-a186-fe463b66d012";
        return Task.CompletedTask;
    }
}
