using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CircuitYard.Server.Models;

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
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Chunk> Chunks { get; set; }
    public DbSet<Cell> Cells { get; set; }
}
