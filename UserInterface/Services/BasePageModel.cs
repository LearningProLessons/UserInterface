using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace UserInterface.Services;
public abstract class BasePageModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenService _tokenService;

    protected BasePageModel(IHttpClientFactory httpClientFactory, ITokenService tokenService)
    {
        _httpClientFactory = httpClientFactory;
        _tokenService = tokenService;
    }

    protected async Task<HttpClient> CreateAuthorizedClientAsync()
    {
        var accessToken = await _tokenService.GetAccessTokenAsync();
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return client;
    }
}
