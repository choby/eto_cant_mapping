using Microsoft.AspNetCore.Builder;

namespace Evo.Scm.Security.Disguise;

public static class DisguiseUserMiddlewareExtensions
{
    public static IApplicationBuilder UseDisguiseUserMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DisguiseUserMiddleware>();
    }
}