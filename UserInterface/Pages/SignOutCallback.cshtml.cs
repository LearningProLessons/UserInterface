using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UserInterface.Pages;

public class SignOutCallbackModel : PageModel
{
    public IActionResult OnGet()
    {
        // Redirect to a page that confirms logout or handles further logic
        return RedirectToAction("Index", "Home"); // Redirect to a home page or another location

    }
}
