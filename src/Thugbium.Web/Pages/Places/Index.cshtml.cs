using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Thugbium.Web.Data;
using Thugbium.Web.Models;
namespace Thugbium.Web.Pages.Places;
public sealed class IndexModel(ThugbiumDbContext database) : PageModel { public List<ThugbiumPlace> Places { get; private set; } = []; public async Task OnGetAsync(CancellationToken cancellationToken) => Places = await database.Places.Include(place => place.Owner).Where(place => place.Visibility == "Public").OrderByDescending(place => place.UpdatedAt).ToListAsync(cancellationToken); }
