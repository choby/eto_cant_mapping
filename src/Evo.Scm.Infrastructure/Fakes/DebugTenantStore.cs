using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Evo.Scm.Fakes;

/// <summary>
/// 自定义租户存储,避免开发人员debug时无租户信息
/// 仅用于开发环境下的debug模式, 严禁用于生产环境
/// </summary>
[Dependency(TryRegister = false, ReplaceServices = false)]
public class DebugTenantStore : ITenantStore, ITransientDependency
{
    private TenantConfiguration[] tenants { get; set; }
    public DebugTenantStore()
    {
        this.tenants = new TenantConfiguration[]
        {
            new TenantConfiguration(Guid.Parse("3a04305b-6baa-273c-a186-fe463b66d012"),"广州致轩服饰")
        };
    }

    public Task<TenantConfiguration> FindAsync(string name)
    {
        return Task.FromResult(Find(name));
    }

    public Task<TenantConfiguration> FindAsync(Guid id)
    {
        return Task.FromResult(Find(id));
    }

    public TenantConfiguration Find(string name)
    {
        return this.tenants.FirstOrDefault(t => t.Name == name);
    }

    public TenantConfiguration Find(Guid id)
    {
        return this.tenants.FirstOrDefault(t => t.Id == id);
    }
}
