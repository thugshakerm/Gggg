using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Thugbium.Web.Services;

namespace Thugbium.Web.Pages;

public sealed class IndexModel(AccountService accounts) : PageModel
{
    [BindProperty] public RegisterInput Registration { get; set; } = new();
    [BindProperty] public LoginInput Credentials { get; set; } = new();
    public string ActiveTab { get; private set; } = "register";
    public string? ErrorMessage { get; private set; }

    public IActionResult OnGet() => User.Identity?.IsAuthenticated == true ? RedirectToPage("/Account/Dashboard") : Page();

    public async Task<IActionResult> OnPostRegisterAsync(CancellationToken cancellationToken)
    {
        ActiveTab = "register";
        if (!ModelState.IsValid) return Page();
        var user = await accounts.CreateAsync(Registration.UserName, Registration.Password, cancellationToken);
        if (user is null) { ErrorMessage = "That username is already in use."; return Page(); }
        await HttpContext.SignInAsync(AccountService.CreatePrincipal(user));
        return RedirectToPage("/Account/Dashboard");
    }

    public async Task<IActionResult> OnPostLoginAsync(CancellationToken cancellationToken)
    {
        ActiveTab = "login";
        if (!ModelState.IsValid) return Page();
        var user = await accounts.AuthenticateAsync(Credentials.UserName, Credentials.Password, cancellationToken);
        if (user is null) { ErrorMessage = "Username or password incorrect."; return Page(); }
        await HttpContext.SignInAsync(AccountService.CreatePrincipal(user));
        return RedirectToPage("/Account/Dashboard");
    }

    public sealed class RegisterInput
    {
        [Required, RegularExpression("^[A-Za-z0-9_]{3,20}$", ErrorMessage = "Username must use 3–20 letters, numbers, or underscores.")]
        public string UserName { get; set; } = string.Empty;
        [Required, StringLength(128, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        [DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
        [Required, Compare(nameof(Password), ErrorMessage = "Those passwords do not match.")]
        [DataType(DataType.Password)] public string ConfirmPassword { get; set; } = string.Empty;
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to the Terms and Privacy Policy.")]
        public bool AcceptTerms { get; set; }
    }

    public sealed class LoginInput
    {
        [Required] public string UserName { get; set; } = string.Empty;
        [Required, DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
    }
}
