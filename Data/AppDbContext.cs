using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CircuitYard.Server.Models;
using CircuitYard.Server.Data.Configurations;

namespace CircuitYard.Server.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // builder.Entity<ApplicationUser>(entity =>
        //         entity.Property(e => e.EnableNotifications).HasDefaultValue(true));
        builder.HasDefaultSchema("identity");
        builder.Entity<RefreshToken>().ToTable("RefreshTokens", "identity");

        builder.ApplyConfiguration(new CellConfiguration());
        builder.ApplyConfiguration(new ChunkConfiguration());
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<PlacedObject> PlacedObjects { get; set; }
}
