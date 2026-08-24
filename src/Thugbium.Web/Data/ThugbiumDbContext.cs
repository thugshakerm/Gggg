using Microsoft.EntityFrameworkCore;
using Thugbium.Web.Models;

namespace Thugbium.Web.Data;

public sealed class ThugbiumDbContext(DbContextOptions<ThugbiumDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var users = modelBuilder.Entity<AppUser>();
        users.ToTable("users");
        users.HasKey(user => user.Id);
        users.Property(user => user.Id).HasColumnName("id");
        users.Property(user => user.ProfileId).HasColumnName("profile_id").ValueGeneratedOnAdd();
        users.Property(user => user.UserName).HasColumnName("user_name");
        users.Property(user => user.PasswordHash).HasColumnName("password_hash");
        users.Property(user => user.DiscordId).HasColumnName("discord_id");
        users.Property(user => user.DiscordUserName).HasColumnName("discord_user_name");
        users.Property(user => user.IsDiscordVerified).HasColumnName("is_discord_verified");
        users.Property(user => user.CreatedAt).HasColumnName("created_at");
        users.Property(user => user.UpdatedAt).HasColumnName("updated_at");
        users.HasIndex(user => user.UserName).IsUnique();
        users.HasIndex(user => user.ProfileId).IsUnique();
        users.HasIndex(user => user.DiscordId).IsUnique();
    }
}
