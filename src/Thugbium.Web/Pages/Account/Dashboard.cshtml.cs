using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Thugbium.Web.Data;
using Thugbium.Web.Models;
namespace Thugbium.Web.Pages.Account;
public sealed class DashboardModel(ThugbiumDbContext database) : PageModel
{
    public AppUser UserData { get; private set; } = null!;
    public List<ThugbiumPlace> Places { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        UserData = await database.Users.FindAsync([id], cancellationToken) ?? throw new InvalidOperationException("User not found.");
        Places = await database.Places.Where(place => place.OwnerUserId == id).OrderByDescending(place => place.UpdatedAt).Take(6).ToListAsync(cancellationToken);
    }
}
