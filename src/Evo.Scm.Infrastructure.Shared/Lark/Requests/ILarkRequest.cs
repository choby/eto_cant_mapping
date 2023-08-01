using Evo.Scm.Lark.Responses;
using RestSharp;

namespace Evo.Scm.Lark.Requests;

public interface ILarkRequest
{
    public string ApiUrl { get; }
    public Method HttpMethod { get; }

    /// <summary>
    /// 获取自定义HTTP请求头参数。
    /// </summary>
    IDictionary<string, string> GetHeaders();

    // <summary>
    /// 获取自定义HTTP请求头参数
    /// </summary>
    object GetPayload();
}

public interface ILarkRequest<out T> : ILarkRequest where T : ILarkResponse
{
}