namespace Evo.Scm.Lark.Services;

public interface IAuthenticationService
{
    Task<JsApiAuthentication> GetJsApiAuthenticationAsync(string url);
}