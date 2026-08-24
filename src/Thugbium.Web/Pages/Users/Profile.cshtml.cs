using Microsoft.AspNetCore.Mvc.RazorPages;
using Thugbium.Web.Data;
using Thugbium.Web.Models;

namespace Thugbium.Web.Pages.Users;

public sealed class ProfileModel(ThugbiumDbContext database) : PageModel
{
    public AppUser? Profile { get; private set; }
    public async Task OnGetAsync(Guid userId, CancellationToken cancellationToken)
    {
        Profile = await database.Users.FindAsync([userId], cancellationToken);
    }
}
