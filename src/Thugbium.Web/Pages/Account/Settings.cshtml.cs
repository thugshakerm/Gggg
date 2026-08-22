using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Thugbium.Web.Data;
using Thugbium.Web.Models;

namespace Thugbium.Web.Pages.Account;

public sealed class SettingsModel(ThugbiumDbContext database) : PageModel
{
    public AppUser SettingsUser { get; private set; } = null!;
    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        SettingsUser = await database.Users.FindAsync([id], cancellationToken) ?? throw new InvalidOperationException("User not found.");
    }
}
