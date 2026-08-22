using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Thugbium.Web.Data;
using Thugbium.Web.Models;

namespace Thugbium.Web.Services;

public sealed class DiscordOAuthService(
    IHttpClientFactory httpClientFactory,
    IDataProtectionProvider dataProtectionProvider,
    IOptions<DiscordOptions> discordOptions,
    ThugbiumDbContext database)
{
    private readonly DiscordOptions _options = discordOptions.Value;
    private readonly IDataProtector _protector = dataProtectionProvider.CreateProtector("Thugbium.DiscordOAuth.v1");

    public bool IsConfigured => !string.IsNullOrWhiteSpace(_options.ClientId)
        && !string.IsNullOrWhiteSpace(_options.ClientSecret)
        && !string.IsNullOrWhiteSpace(_options.RedirectUri);

    public string CreateAuthorizationUrl(Guid userId)
    {
        var state = Convert.ToBase64String(_protector.Protect(
            System.Text.Encoding.UTF8.GetBytes($"{userId}|{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}|{RandomNumberGenerator.GetHexString(16)}")));
        var parameters = new Dictionary<string, string>
        {
            ["client_id"] = _options.ClientId,
            ["redirect_uri"] = _options.RedirectUri,
            ["response_type"] = "code",
            ["scope"] = "identify",
            ["state"] = state,
            ["prompt"] = "consent"
        };
        return "https://discord.com/api/oauth2/authorize?" + string.Join("&", parameters.Select(pair =>
            $"{Uri.EscapeDataString(pair.Key)}={Uri.EscapeDataString(pair.Value)}"));
    }

    public async Task<string?> LinkAsync(string code, string state, CancellationToken cancellationToken)
    {
        Guid userId;
        try
        {
            var payload = System.Text.Encoding.UTF8.GetString(_protector.Unprotect(Convert.FromBase64String(state))).Split('|');
            if (payload.Length != 3 || !Guid.TryParse(payload[0], out userId) ||
                DateTimeOffset.UtcNow.ToUnixTimeSeconds() - long.Parse(payload[1]) > 600) return "This Discord link has expired. Try again.";
        }
        catch (Exception) { return "This Discord link is invalid. Try again."; }

        var client = httpClientFactory.CreateClient();
        using var tokenResponse = await client.PostAsync("https://discord.com/api/oauth2/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = _options.ClientId,
            ["client_secret"] = _options.ClientSecret,
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["redirect_uri"] = _options.RedirectUri
        }), cancellationToken);
        if (!tokenResponse.IsSuccessStatusCode) return "Discord could not complete verification. Try again.";

        using var token = JsonDocument.Parse(await tokenResponse.Content.ReadAsStringAsync(cancellationToken));
        var accessToken = token.RootElement.GetProperty("access_token").GetString();
        using var profileRequest = new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/users/@me");
        profileRequest.Headers.Authorization = new("Bearer", accessToken);
        using var profileResponse = await client.SendAsync(profileRequest, cancellationToken);
        if (!profileResponse.IsSuccessStatusCode) return "Discord profile lookup failed. Try again.";

        using var profile = JsonDocument.Parse(await profileResponse.Content.ReadAsStringAsync(cancellationToken));
        var discordId = profile.RootElement.GetProperty("id").GetString()!;
        var discordName = profile.RootElement.GetProperty("username").GetString()!;
        var alreadyLinked = await database.Users.AnyAsync(user => user.DiscordId == discordId && user.Id != userId, cancellationToken);
        if (alreadyLinked) return "That Discord account is already linked to another Thugbium account.";

        var user = await database.Users.FindAsync([userId], cancellationToken);
        if (user is null) return "Your Thugbium account could not be found.";
        user.DiscordId = discordId;
        user.DiscordUserName = discordName;
        user.IsDiscordVerified = true;
        user.UpdatedAt = DateTimeOffset.UtcNow;
        await database.SaveChangesAsync(cancellationToken);
        return null;
    }
}
