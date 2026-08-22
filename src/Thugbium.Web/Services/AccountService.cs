using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Thugbium.Web.Data;
using Thugbium.Web.Models;

namespace Thugbium.Web.Services;

public sealed class AccountService(ThugbiumDbContext database)
{
    private readonly PasswordHasher<AppUser> _passwordHasher = new();

    public async Task<AppUser?> CreateAsync(string userName, string password, CancellationToken cancellationToken)
    {
        var normalized = userName.Trim();
        var exists = await database.Users.AnyAsync(user => user.UserName.ToLower() == normalized.ToLower(), cancellationToken);
        if (exists) return null;

        var user = new AppUser { UserName = normalized };
        user.PasswordHash = _passwordHasher.HashPassword(user, password);
        database.Users.Add(user);
        await database.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<AppUser?> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
    {
        var user = await database.Users.SingleOrDefaultAsync(
            candidate => candidate.UserName.ToLower() == userName.Trim().ToLower(), cancellationToken);
        if (user is null) return null;

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded ? user : null;
    }

    public static ClaimsPrincipal CreatePrincipal(AppUser user) => new(
        new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("discord_verified", user.IsDiscordVerified.ToString().ToLowerInvariant())
        ], "ThugbiumCookie"));
}
