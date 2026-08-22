namespace Thugbium.Web.Models;

public sealed class DiscordOptions
{
    public const string SectionName = "Discord";
    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
    public string RedirectUri { get; init; } = string.Empty;
}
