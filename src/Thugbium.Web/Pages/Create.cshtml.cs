using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Thugbium.Web.Data;
using Thugbium.Web.Models;

namespace Thugbium.Web.Pages;

public sealed class CreateModel(ThugbiumDbContext database) : PageModel
{
    [BindProperty] public CreateInput Input { get; set; } = new();
    public List<ThugbiumPlace> Places { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken cancellationToken) => await LoadPlaces(cancellationToken);

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) { await LoadPlaces(cancellationToken); return Page(); }
        var ownerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var place = new ThugbiumPlace { OwnerUserId = ownerId, Name = Input.Name.Trim(), Description = Input.Description?.Trim() ?? string.Empty, Visibility = Input.Visibility };
        database.Places.Add(place);
        await database.SaveChangesAsync(cancellationToken);
        return RedirectToPage("/Places/Details", new { id = place.Id });
    }

    private async Task LoadPlaces(CancellationToken cancellationToken)
    {
        var ownerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        Places = await database.Places.Where(place => place.OwnerUserId == ownerId).OrderByDescending(place => place.UpdatedAt).ToListAsync(cancellationToken);
    }

    public sealed class CreateInput
    {
        [Required, StringLength(100, MinimumLength = 3)] public string Name { get; set; } = string.Empty;
        [StringLength(1000)] public string? Description { get; set; }
        [Required, RegularExpression("^(Private|Public)$")] public string Visibility { get; set; } = "Private";
    }
}
