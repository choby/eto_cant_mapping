namespace Evo.Scm.Lark.Core;

public class JsApiTicket
{
    public JsApiTicket(string ticket, DateTime expireTime)
    {
        this.Ticket = ticket;
        this.ExpireTime = expireTime;
    }

    public string Ticket { get; private set; }
    /// <summary>
    /// 超时时间
    /// </summary>
    public DateTime ExpireTime { get; private set; }
}


public static class JsSdkTicketExtensions
{
    public static bool IsExpired(this JsApiTicket tenantAccessToken)
    {
        return tenantAccessToken == null || DateTime.Now > tenantAccessToken.ExpireTime;
    }
}