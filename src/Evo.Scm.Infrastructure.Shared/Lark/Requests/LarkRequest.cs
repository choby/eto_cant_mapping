using Evo.Scm.Lark.Responses;
using RestSharp;

namespace Evo.Scm.Lark.Requests;

public abstract class LarkRequest<T> : ILarkRequest<T> where T : ILarkResponse
{
    IDictionary<string, string> headers;
    object payload;

    public LarkRequest()
    {
        this.headers = new Dictionary<string, string>();
        this.AddHeaders();
        this.AddPayload();
    }

    public abstract string ApiUrl { get; }
    public abstract Method HttpMethod { get; }

    protected void AddHeader(string key, string value)
    {
        this.headers.Add(key, value);
    }

    protected void AddPayload(object payload)
    {
        this.payload = payload;
    }

    protected abstract void AddHeaders();

    protected abstract void AddPayload();


    public IDictionary<string, string> GetHeaders()
    {
        return headers;
    }

    public object GetPayload()
    {
        return this.payload;
    }
}