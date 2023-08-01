using System.Text.Json.Serialization;

namespace Evo.Scm.Lark.Responses;

public class JsSdkTicketResponseData : ILarkResponseData
{
    [JsonPropertyName("ticket")] public string Ticket { get; set; }
    [JsonPropertyName("expire_in")] public int Expire { get; set; }
}

public class JsSdkTicketResponse : LarkResponse<JsSdkTicketResponseData>
{
}