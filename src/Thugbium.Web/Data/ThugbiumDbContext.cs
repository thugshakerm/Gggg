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
        users.HasIndex(user => user.UserName).IsUnique();
        users.HasIndex(user => user.DiscordId).IsUnique();
    }
}
