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
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Page("/SignOutCallback") // This should be the URI defined in PostLogoutRedirectUris
        };

        return SignOut(
            props,
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
    }
}
