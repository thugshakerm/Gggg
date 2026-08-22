using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Thugbium.Web.Pages;
public sealed class IndexModel : PageModel { public IActionResult OnGet() => User.Identity?.IsAuthenticated == true ? RedirectToPage("/Account/Dashboard") : Page(); }
