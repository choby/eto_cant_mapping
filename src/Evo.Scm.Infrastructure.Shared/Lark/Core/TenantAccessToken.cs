namespace Evo.Scm.Lark.Core;

public class TenantAccessToken
{
    public TenantAccessToken(string accessToken, DateTime expireTime)
    {
        this.AccessToken = accessToken;
        this.ExpireTime = expireTime;
    }

    public string AccessToken { get; private set; }
    /// <summary>
    /// 超时时间
    /// </summary>
    public DateTime ExpireTime { get; private set; }
    
}


public static class TenantAccessTokenExtensions
{
    public static bool IsExpired(this TenantAccessToken tenantAccessToken)
    {
        return tenantAccessToken == null || DateTime.Now > tenantAccessToken.ExpireTime;
    }
}