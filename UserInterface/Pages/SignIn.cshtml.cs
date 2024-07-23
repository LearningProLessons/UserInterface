using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UserInterface.Pages;

public class SignInModel : PageModel
{
    public async Task<IActionResult> OnGet()
    {
        // Initiate sign-in
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Page("/Index") // Set the redirect URI here
        };

        // Sign in with OpenID Connect
        return Challenge(props, OpenIdConnectDefaults.AuthenticationScheme);
    }
}
