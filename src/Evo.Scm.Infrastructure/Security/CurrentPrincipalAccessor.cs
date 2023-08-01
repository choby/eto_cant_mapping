using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Volo.Abp;
using Volo.Abp.AspNetCore.Security.Claims;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Evo.Scm.Security;

// [Dependency(ReplaceServices = true)]
// [ExposeServices(typeof(ICurrentPrincipalAccessor))]
// public class CurrentPrincipalAccessor: HttpContextCurrentPrincipalAccessor, ISingletonDependency
// {
//     private ClaimsPrincipal? principal;
//     private readonly IHttpContextAccessor _httpContextAccessor;
//     
//
//     public CurrentPrincipalAccessor(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
//     {
//         _httpContextAccessor = httpContextAccessor;
//     }
//
//     protected override ClaimsPrincipal GetClaimsPrincipal()
//     {
//         if (_httpContextAccessor.HttpContext?.User is not null)
//             return _httpContextAccessor.HttpContext.User;
//         
//         return principal ?? base.GetClaimsPrincipal();
//     }
//     
//     public override IDisposable Change(ClaimsPrincipal principal)
//     {
//         this.principal = principal;
//         return new DisposeAction(() =>
//         {
//             this.principal = principal;
//         });
//     }
// }