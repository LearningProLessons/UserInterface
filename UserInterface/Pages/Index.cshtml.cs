using Microsoft.AspNetCore.Authorization;
using UserInterface.Services;
using UserInterface.Models.Reasons;

public class IndexModel : BasePageModel
{
    private readonly ApiService _apiService;

    public IndexModel(IHttpClientFactory httpClientFactory, ITokenService tokenService, ApiService apiService)
        : base(httpClientFactory, tokenService)
    {
        _apiService = apiService;
    }

    public List<ReasonResponse> ReasonList { get; private set; }
    public string ErrorMessage { get; private set; } // Add an ErrorMessage property to handle errors

    public async Task OnGetAsync()
    {
        var response = await _apiService.GetAsync<List<ReasonResponse>>("/api/BasicInfo/GetReasonList");
        if (response.Success)
        {
            ReasonList = response.Data; // Assign the data to the Reasons property
        }
        else
        {
            ErrorMessage = response.ErrorMessage; // Handle errors and assign the error message
        }
    }
}
