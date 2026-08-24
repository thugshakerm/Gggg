using System.ComponentModel.DataAnnotations;

namespace Thugbium.Web.Models;

public sealed class AppUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long ProfileId { get; set;}

    [MaxLength(20)]
    public string UserName { get; set; } = string.Empty;

    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(32)]
    public string? DiscordId { get; set; }

    [MaxLength(100)]
    public string? DiscordUserName { get; set; }

    public bool IsDiscordVerified { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
