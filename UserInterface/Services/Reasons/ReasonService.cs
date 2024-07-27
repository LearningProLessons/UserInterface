using System.Net.Http.Headers;

namespace UserInterface.Services.Reasons;
public class ReasonService : IReasonService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenService _tokenService;

    public ReasonService(IHttpClientFactory httpClientFactory, ITokenService tokenService)
    {
        _httpClientFactory = httpClientFactory;
        _tokenService = tokenService;
    }

    public async Task<string> GetReasonListAsync()
    {
        var accessToken = await _tokenService.GetAccessTokenAsync();

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await client.GetAsync("https://localhost:58862/api/BasicInfo/GetReasonList");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            return $"Error: {response.ReasonPhrase}";
        }
    }
}
