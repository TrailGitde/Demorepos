namespace Framework.Authentication.Domain.Services;
public interface IAuthenticationQuery
{
    public Task<ProviderData> Authorize(string appId, string appKey);
    public Task<ProviderData> AuthorizeToken(string token, string appId, string appKey);
    public string GenerateToken(ProviderData validateInput,DateTime TokenIssuedOn, DateTime TokenExpires);
}
