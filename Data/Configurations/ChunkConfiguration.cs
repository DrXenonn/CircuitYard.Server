using CircuitYard.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CircuitYard.Server.Data.Configurations;

public class ChunkConfiguration : IEntityTypeConfiguration<Chunk>
{
    public void Configure(EntityTypeBuilder<Chunk> builder)
    {
        builder.HasKey(c => new { c.X, c.Y });
        builder.HasMany(c => c.Cells).WithOne();
    }
}

