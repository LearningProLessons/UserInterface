using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;

namespace UserInterface.Pages;

[Authorize(Roles = "admin")] // Only allow access to users with the "admin" role
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

        // Check if the user has any specific claims or roles to display
        if (User.Identity.IsAuthenticated)
        {
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            var organizationIdClaim = User.FindFirst("organization_id")?.Value;

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
