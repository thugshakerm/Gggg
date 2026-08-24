using Microsoft.EntityFrameworkCore;
using Thugbium.Web.Models;

namespace Thugbium.Web.Data;

public sealed class ThugbiumDbContext(DbContextOptions<ThugbiumDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<ThugbiumPlace> Places => Set<ThugbiumPlace>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var users = modelBuilder.Entity<AppUser>();
        users.ToTable("users");
        users.HasKey(user => user.Id);
        users.Property(user => user.Id).HasColumnName("id");
        users.Property(user => user.UserName).HasColumnName("user_name");
        users.Property(user => user.PasswordHash).HasColumnName("password_hash");
        users.Property(user => user.DiscordId).HasColumnName("discord_id");
        users.Property(user => user.DiscordUserName).HasColumnName("discord_user_name");
        users.Property(user => user.IsDiscordVerified).HasColumnName("is_discord_verified");
        users.Property(user => user.CreatedAt).HasColumnName("created_at");
        users.Property(user => user.UpdatedAt).HasColumnName("updated_at");
        users.HasIndex(user => user.UserName).IsUnique();
        users.HasIndex(user => user.DiscordId).IsUnique();

        var places = modelBuilder.Entity<ThugbiumPlace>();
        places.ToTable("places");
        places.HasKey(place => place.Id);
        places.Property(place => place.Id).HasColumnName("id");
        places.Property(place => place.OwnerUserId).HasColumnName("owner_user_id");
        places.Property(place => place.Name).HasColumnName("name");
        places.Property(place => place.Description).HasColumnName("description");
        places.Property(place => place.Visibility).HasColumnName("visibility");
        places.Property(place => place.CreatedAt).HasColumnName("created_at");
        places.Property(place => place.UpdatedAt).HasColumnName("updated_at");
        places.HasOne(place => place.Owner).WithMany().HasForeignKey(place => place.OwnerUserId);
        places.HasIndex(place => place.OwnerUserId);
    }
}
