using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CircuitYard.Server.Models;

namespace CircuitYard.Server.Data.Configurations;

public sealed class PlacedObjectConfiguration : IEntityTypeConfiguration<PlacedObject>
{
    public void Configure(EntityTypeBuilder<PlacedObject> builder)
    {
        builder.HasKey(p => new { p.X, p.Y });
    }
}
