using System.Text;
using UserInterface.Models.Common;

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

    private string BuildUrl(string endpoint)
    {
        var baseUrl = _configuration["ApiBaseUrl"];
        var urlBuilder = new StringBuilder(baseUrl);
        urlBuilder.Append(endpoint);
        return urlBuilder.ToString();
    }

    public async Task<ApiResponse<T>> GetAsync<T>(string endpoint) where T : class
    {
        try
        {
            var client = await CreateAuthorizedClientAsync();
            var url = BuildUrl(endpoint);
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<T>();
            return new ApiResponse<T> { Success = true, Data = data };
        }
        catch (HttpRequestException ex)
        {
            return new ApiResponse<T> { Success = false, ErrorMessage = ex.Message };
        }
    }

    public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, T data)
    {
        try
        {
            var client = await CreateAuthorizedClientAsync();
            var url = BuildUrl(endpoint);
            var response = await client.PostAsJsonAsync(url, data);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadFromJsonAsync<T>();
            return new ApiResponse<T> { Success = true, Data = responseData };
        }
        catch (HttpRequestException ex)
        {
            return new ApiResponse<T> { Success = false, ErrorMessage = ex.Message };
        }
    }

    public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, T data)
    {
        try
        {
            var client = await CreateAuthorizedClientAsync();
            var url = BuildUrl(endpoint);
            var response = await client.PutAsJsonAsync(url, data);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadFromJsonAsync<T>();
            return new ApiResponse<T> { Success = true, Data = responseData };
        }
        catch (HttpRequestException ex)
        {
            return new ApiResponse<T> { Success = false, ErrorMessage = ex.Message };
        }
    }

    public async Task<ApiResponse<string>> DeleteAsync(string endpoint)
    {
        try
        {
            var client = await CreateAuthorizedClientAsync();
            var url = BuildUrl(endpoint);
            var response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            return new ApiResponse<string> { Success = true, Data = "Deleted successfully" };
        }
        catch (HttpRequestException ex)
        {
            return new ApiResponse<string> { Success = false, ErrorMessage = ex.Message };
        }
    }
}
