using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;


[Authorize]
public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public string ReasonList { get; private set; }

    public async Task OnGetAsync()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        if (string.IsNullOrEmpty(accessToken))
        {
            throw new UnauthorizedAccessException("Access token is missing.");
        }

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await client.GetAsync("https://localhost:58862/api/BasicInfo/GetReasonList");

        if (response.IsSuccessStatusCode)
        {
            ReasonList = await response.Content.ReadAsStringAsync();
        }
        else
        {
            ReasonList = $"Error: {response.ReasonPhrase}";
        }
    }
}
