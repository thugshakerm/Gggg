using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Thugbium.Web.Pages.Account;
public sealed class LogoutModel : PageModel { public async Task<IActionResult> OnPostAsync() { await HttpContext.SignOutAsync(); return RedirectToPage("/Index"); } }
