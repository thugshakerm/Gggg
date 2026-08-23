using System.ComponentModel.DataAnnotations;

namespace Thugbium.Web.Models;

public sealed class ThugbiumPlace
{
    public long Id { get; set; }
    public Guid OwnerUserId { get; set; }
    public AppUser Owner { get; set; } = null!;

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(16)]
    public string Visibility { get; set; } = "Private";

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
