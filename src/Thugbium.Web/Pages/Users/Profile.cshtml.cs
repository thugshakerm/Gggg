using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Thugbium.Web.Data;
using Thugbium.Web.Models;
namespace Thugbium.Web.Pages.Users;
public sealed class ProfileModel(ThugbiumDbContext database) : PageModel { public AppUser? Profile { get; private set; } public async Task OnGetAsync(string username, CancellationToken cancellationToken) { Profile = await database.Users.SingleOrDefaultAsync(user => user.UserName.ToLower() == username.ToLower(), cancellationToken); } }
