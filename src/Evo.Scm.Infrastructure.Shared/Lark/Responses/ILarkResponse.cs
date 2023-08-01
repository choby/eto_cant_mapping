namespace Evo.Scm.Lark.Responses;

public interface ILarkResponse
{
    public int Code { get; init; }
    public string Message { get; init; }

    public bool IsSuccess();
}

public interface ILarkResponse<T> : ILarkResponse where T : ILarkResponseData
{
    public T Data { get; init; }
}