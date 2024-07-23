using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UserInterface.Pages;

public class SignOutCallbackModel : PageModel
{
    public IActionResult OnGet()
    {
        // Redirect to a page that confirms logout or handles further logic
        return RedirectToPage("/Index"); // Redirect to a specific page or URL
    }
}
