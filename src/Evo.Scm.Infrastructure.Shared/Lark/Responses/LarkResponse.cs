using System.Text.Json.Serialization;

namespace Evo.Scm.Lark.Responses;

public class LarkResponse: ILarkResponse
{
    [JsonPropertyName("code")]
    public int Code { get; init; }
    [JsonPropertyName("msg")] public string Message { get; init; }

    public bool IsSuccess()
    {
        return this.Code == (int)LarkResponseStatus.SUCCESS;
    }
}

public class LarkResponse<T> :LarkResponse, ILarkResponse<T> where T:ILarkResponseData
{
    [JsonPropertyName("data")]
    public T Data { get; init; }
}

public enum LarkResponseStatus
{
    SUCCESS = 0,
    _ = -1,
}