using System.Security.Claims;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Evo.Scm.Fakes;

/// <summary>
/// 自定义用户身份访问器,避免开发人员debug时无用户信息
/// 仅用于开发环境下的debug模式, 严禁用于生产环境
/// </summary>

[Dependency(ReplaceServices = false, TryRegister = false)]

public class DebugCurrentPrincipalAccessor : CurrentPrincipalAccessorBase, ICurrentPrincipalAccessor
{
    protected override ClaimsPrincipal GetClaimsPrincipal()
    {
        return GetPrincipal();
    }
    
    public override IDisposable Change(ClaimsPrincipal principal)
    {
        this._principal = principal;
        return new DisposeAction(() =>
        {
            this._principal = principal;
        });
    }
    

    private ClaimsPrincipal _principal;

    private ClaimsPrincipal GetPrincipal()
    {
        if (_principal == null)
        {
            lock (this)
            {
                if (_principal == null)
                {
                    _principal = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new List<Claim>
                            {
                                    new Claim(AbpClaimTypes.UserId,"51e7a9d5-6bc3-47fa-9ba8-c505de427d8b"),
                                    new Claim(AbpClaimTypes.Role,"admin"),
                                    new Claim(AbpClaimTypes.UserName,"admin"),
                                    new Claim(AbpClaimTypes.Email,"email@email.com"),
                                    new Claim(AbpClaimTypes.Name,"admin"),
                                    new Claim(AbpClaimTypes.TenantId,"6e69e588-2686-43f2-a441-367b3b1b7c2e"),
                                    new Claim(AbpClaimTypes.Role,"admin")
                            }
                        )
                    );
                }
            }
        }

        return _principal;
    }
}
