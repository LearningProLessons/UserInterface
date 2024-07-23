using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UserInterface.Pages;

public class LogoutModel : PageModel
{
    public IActionResult OnGet()
    {
        // Initiate sign-out
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action("SignOutCallback", "Account") // Set the redirect URI here
        };

        return SignOut(
            props,
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
    }
}
