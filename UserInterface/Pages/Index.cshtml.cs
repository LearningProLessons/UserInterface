using Microsoft.AspNetCore.Authorization;
using UserInterface.Services.Reasons;
using UserInterface.Services;

[Authorize]
public class IndexModel : BasePageModel
{
    public IndexModel(IHttpClientFactory httpClientFactory, ITokenService tokenService)
        : base(httpClientFactory, tokenService)
    {
    }

    public string ReasonList { get; private set; }

    public async Task OnGetAsync()
    {
        try
        {
            var client = await CreateAuthorizedClientAsync(); // Await to get HttpClient instance
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
        catch (UnauthorizedAccessException ex)
        {
            ReasonList = $"Error: {ex.Message}";
        }
    }

}
