using System.Security.Claims;
using Volo.Abp.Security.Claims;

namespace Evo.Scm.Extensions;

public static class CurrentPrincipalAccessorExtensions
{
    /// <summary>
    /// 改变用户
    /// </summary>
    /// <param name="currentPrincipalAccessor"></param>
    /// <param name="userRealName">用户真实姓名，因为scm系统的业务并不依赖 username</param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public static IDisposable Change(this ICurrentPrincipalAccessor currentPrincipalAccessor, string userRealName, Guid? tenantId)
    {
       return currentPrincipalAccessor.Change(new ClaimsIdentity(
            new List<Claim>
            {
                new Claim(AbpClaimTypes.UserId, default(Guid).ToString()),
                new Claim(AbpClaimTypes.Role, "admin"),
                new Claim(AbpClaimTypes.Name, userRealName),
                new Claim(AbpClaimTypes.TenantId, tenantId?.ToString()),
            }
        ));
    }
    /// <summary>
    /// 改变用户
    /// </summary>
    /// <param name="currentPrincipalAccessor"></param>
    /// <param name="userId">用户 id</param>
    /// <param name="userRealName">用户真实姓名，因为scm系统的业务并不依赖 username</param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public static IDisposable Change(this ICurrentPrincipalAccessor currentPrincipalAccessor, Guid userId, string userRealName, Guid? tenantId)
    {
        return currentPrincipalAccessor.Change(new ClaimsIdentity(
            new List<Claim>
            {
                new Claim(AbpClaimTypes.UserId, userId.ToString()),
                new Claim(AbpClaimTypes.Name, userRealName),
                new Claim(AbpClaimTypes.TenantId, tenantId?.ToString()),
            }
        ));
    }
}