using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UserInterface.Pages;

[Authorize(Policy = "AdminPolicy")]
public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }
    public string Message { get; set; }

    // This method is accessible for users with the UserPolicy
    public void OnGet()
    {
        Message = "Welcome to the Privacy Page!";
    }
}
