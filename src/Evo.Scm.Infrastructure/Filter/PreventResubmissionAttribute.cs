using System.Net;
using System.Text;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;

namespace Evo.Scm.Filter;

public class PreventResubmissionAttribute : Attribute, IActionFilter
{
    private static MemoryCache cache = new MemoryCache(new MemoryCacheOptions());

    
    public void OnActionExecuting(ActionExecutingContext context)
    {
        string httpMethod = WebUtility.HtmlEncode(context.HttpContext.Request.Method);
        if (httpMethod != "GET")
        {
            var service = context.HttpContext.RequestServices.GetRequiredService<ICachedServiceProvider>();
            var options = service.GetService<IOptions<AbpAntiForgeryOptions>>();
            var tokenFormName = options.Value.TokenCookie.Name;
            string hiddenToken;
            if (context.HttpContext.Request.Headers.ContainsKey(tokenFormName))
                hiddenToken = context.HttpContext.Request.Headers[tokenFormName].ToString();
            else
                return;

            var path = context.HttpContext.Request.QueryString;
            StreamReader reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8);
            var str = reader.ReadToEndAsync().Result;
            var bodyMD5 = str.ToMd5();
           
            string cacheToken = $"{hiddenToken}_{path}_{bodyMD5}";
            string keyValue = new Guid().ToString() + DateTime.Now.Ticks;
            if (path != null)
            {
                var cv = cache.Get(cacheToken);
                if (cv == null)
                {
                    var abpDistributedLock = service.GetRequiredService<IAbpDistributedLock>();
                    var handle = abpDistributedLock.TryAcquireAsync(cacheToken).ConfigureAwait(false).GetAwaiter().GetResult();

                    if (cv == null)
                        cache.Set(cacheToken, keyValue, new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromSeconds(1) });
                    handle.DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }
                else
                {
                    ResultMessage<string> result = new ResultMessage<string>();
                    result.Code = "-100";
                    result.Message = "1秒内请不要重复提交";
                    context.Result = new JsonResult(result);
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Accepted;
                }
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}