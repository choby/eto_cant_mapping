using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace Evo.Scm;

public static class UserExtensions
{
    public static Guid GetIdWithDefaultValue(this ICurrentUser currentUser)
    {
        return currentUser.TenantId ?? default(Guid);
    }

    public static bool HasRole(this ICurrentUser currentUser,string role)
    {
        return currentUser.Roles.Contains(role);
    }
}
