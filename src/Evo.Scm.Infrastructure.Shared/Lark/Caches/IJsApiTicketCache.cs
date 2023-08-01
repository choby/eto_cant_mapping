using Evo.Scm.Lark.Core;
using Microsoft.Extensions.Caching.Memory;

namespace Evo.Scm.Lark.Caches;

public interface IJsApiTicketCache
{
    Task<JsApiTicket> GetOrCreateAsync(Func<ICacheEntry, Task<JsApiTicket>> factory);

    Task SetAsync(JsApiTicket tenantAccessToken);

    void Remove();
}