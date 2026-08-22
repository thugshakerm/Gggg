using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Thugbium.Web.Services;
namespace Thugbium.Web.Pages.Account;
public sealed class DiscordCallbackModel(DiscordOAuthService discord) : PageModel { [TempData] public string? Message { get; set; } public async Task<IActionResult> OnGetAsync(string? code, string? state, string? error, CancellationToken cancellationToken) { if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state)) { Message = "Discord verification was cancelled or could not be completed."; return RedirectToPage("/Account/Discord"); } Message = await discord.LinkAsync(code, state, cancellationToken) ?? "Discord account linked successfully."; return RedirectToPage("/Account/Dashboard"); } }
