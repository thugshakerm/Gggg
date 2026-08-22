using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Thugbium.Web.Services;
namespace Thugbium.Web.Pages.Account;
public sealed class DiscordModel(DiscordOAuthService discord) : PageModel { [TempData] public string? Message { get; set; } public void OnGet() { } public IActionResult OnGetStart() { if (!discord.IsConfigured) { Message = "Discord OAuth is not configured on this server yet."; return RedirectToPage(); } return Redirect(discord.CreateAuthorizationUrl(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!))); } }
