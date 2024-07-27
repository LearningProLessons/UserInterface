using Microsoft.AspNetCore.Authentication;

namespace UserInterface.Services;
public interface ITokenService
{
    Task<string> GetAccessTokenAsync();
}

public class TokenService : ITokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

        if (string.IsNullOrEmpty(accessToken))
        {
            throw new UnauthorizedAccessException("Access token is missing.");
        }

        return accessToken;
    }
}
