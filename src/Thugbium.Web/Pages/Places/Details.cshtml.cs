using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Thugbium.Web.Data;
using Thugbium.Web.Models;
namespace Thugbium.Web.Pages.Places;
public sealed class DetailsModel(ThugbiumDbContext database) : PageModel { public ThugbiumPlace? Place { get; private set; } public async Task OnGetAsync(long id, CancellationToken cancellationToken) { var userId = User.Identity?.IsAuthenticated == true ? Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!) : Guid.Empty; Place = await database.Places.Include(place => place.Owner).SingleOrDefaultAsync(place => place.Id == id && (place.Visibility == "Public" || place.OwnerUserId == userId), cancellationToken); } }
