namespace Evo.Scm.Lark.Services;

public class JsApiAuthentication
{
    public string AppId { get; set; }
    public string NonceStr { get; set; }
    public string Signature { get; set; }
    public long TimeStamp { get; set; }
}