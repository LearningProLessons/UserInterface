using Microsoft.AspNetCore.Authorization;
using UserInterface.Services;

 
[Authorize]
public class IndexModel : BasePageModel
{
    private readonly ApiService _apiService;

    public IndexModel(IHttpClientFactory httpClientFactory, ITokenService tokenService, ApiService apiService)
        : base(httpClientFactory, tokenService)
    {
        _apiService = apiService;
    }

    public string ReasonList { get; private set; }

    public async Task OnGetAsync()
    {
        try
        {
           ReasonList = await _apiService.GetAsync("/api/BasicInfo/GetReasonList");
        }
        catch (HttpRequestException ex)
        {
            ReasonList = $"Error: {ex.Message}";
        }
    }
}
