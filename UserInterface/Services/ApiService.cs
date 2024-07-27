using System.Text;

namespace UserInterface.Services;    

public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;

    public ApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ITokenService tokenService)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _tokenService = tokenService;
    }

    private async Task<HttpClient> CreateAuthorizedClientAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var token = await _tokenService.GetAccessTokenAsync(); // Adjust this based on your actual method to get the token
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    public async Task<string> GetAsync(string endpoint)
    {
        var client = await CreateAuthorizedClientAsync();
        var baseUrl = _configuration["ApiBaseUrl"];

        // Use StringBuilder to construct the URL
        var urlBuilder = new StringBuilder(baseUrl);
        urlBuilder.Append(endpoint);

        var response = await client.GetAsync(urlBuilder.ToString());
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

}
