using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace UserInterface.Pages;

[Authorize(Policy = "AdminRolesPolicy")]
public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public string Message { get; set; }

    public void OnGet()
    {
        // Retrieve the user's claims
        var userClaims = User.Claims.ToList();

        // Check if the user is authenticated
        if (User.Identity.IsAuthenticated)
        {
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            var organizationIdClaim = User.FindFirst("organizationId")?.Value; // Ensure the correct claim name

            // Build a message for display
            Message = $"Welcome! You are authenticated as an {string.Join(", ", roles)}. " +
                      (organizationIdClaim != null ? $"Your organization ID is: {organizationIdClaim}." : "");
        }
        else
        {
            Message = "You are not authenticated.";
        }
    }
}
