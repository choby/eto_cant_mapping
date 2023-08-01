using System.Globalization;
using System.Net;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Security.Claims;
using Evo.Scm.Extensions;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace Evo.Scm.Security.Disguise;

public class DisguiseUserMiddleware
{
    private readonly RequestDelegate _next;

    public DisguiseUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var currentUser = context.RequestServices.GetRequiredService<ICurrentUser>();
        var disguise = context.Request.Headers["Disguise"].ToString();
        if (currentUser.IsInRole(Authorization.Roles.AllPermissions) && !string.IsNullOrWhiteSpace(disguise)) //只有管理员才能模拟用户
        {
            var arrDisguise = disguise.Split(';');
            var disguiseUserId = Guid.Parse(arrDisguise[0]);
            var disguiseUserRealName = WebUtility.UrlDecode(arrDisguise[1]);
            var currentPrincipalAccessor = context.RequestServices.GetRequiredService<ICurrentPrincipalAccessor>();
            var currentTenant = context.RequestServices.GetRequiredService<ICurrentTenant>();//暂时只允许租户管理员模拟本租户内的用户，如果是宿主管理员需要模拟任意用户需要另外实现
            using (currentPrincipalAccessor.Change(disguiseUserId, disguiseUserRealName, currentTenant.Id))
            {
                await _next(context);
            }
        }
        else
        {
            await _next(context);
        }
    }
}