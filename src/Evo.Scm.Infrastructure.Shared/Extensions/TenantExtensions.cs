using Volo.Abp.MultiTenancy;

namespace Evo.Scm;

public static class TenantExtensions
{
    public static Guid GetIdWithDefaultValue(this ICurrentTenant currentTenant)
    {
        return currentTenant.Id ?? default(Guid);
    }
}
