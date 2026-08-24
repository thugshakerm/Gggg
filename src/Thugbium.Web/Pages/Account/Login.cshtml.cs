using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Thugbium.Web.Services;

namespace Thugbium.Web.Pages.Account;

public sealed class LoginModel(AccountService accounts) : PageModel
{
    [BindProperty] public LoginInput Input { get; set; } = new();
    public IActionResult OnGet() => User.Identity?.IsAuthenticated == true ? RedirectToPage("/Account/Dashboard") : RedirectToPage("/Index");
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return Page();
        var user = await accounts.AuthenticateAsync(Input.UserName, Input.Password, cancellationToken);
        if (user is null) { ModelState.AddModelError(string.Empty, "That username or password is not correct."); return Page(); }
        await HttpContext.SignInAsync(AccountService.CreatePrincipal(user));
        return LocalRedirect(Url.Page("/Account/Dashboard")!);
    }
    public sealed class LoginInput { [Required] public string UserName { get; set; } = string.Empty; [Required, DataType(DataType.Password)] public string Password { get; set; } = string.Empty; }
}
