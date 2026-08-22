using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Thugbium.Web.Services;

namespace Thugbium.Web.Pages.Account;

public sealed class RegisterModel(AccountService accounts) : PageModel
{
    [BindProperty] public RegisterInput Input { get; set; } = new();

    public IActionResult OnGet() => User.Identity?.IsAuthenticated == true ? RedirectToPage("/Account/Dashboard") : Page();

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return Page();
        var user = await accounts.CreateAsync(Input.UserName, Input.Password, cancellationToken);
        if (user is null) { ModelState.AddModelError(string.Empty, "That username is already in use."); return Page(); }
        await HttpContext.SignInAsync(AccountService.CreatePrincipal(user));
        return RedirectToPage("/Account/Dashboard");
    }

    public sealed class RegisterInput
    {
        [Required, RegularExpression("^[A-Za-z0-9_]{3,20}$")] public string UserName { get; set; } = string.Empty;
        [Required, StringLength(128, MinimumLength = 8), DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
        [Required, Compare(nameof(Password)), DataType(DataType.Password)] public string ConfirmPassword { get; set; } = string.Empty;
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms to create an account.")] public bool AcceptTerms { get; set; }
    }
}
