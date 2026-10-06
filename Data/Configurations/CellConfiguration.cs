using CircuitYard.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CircuitYard.Server.Data.Configurations;

public class CellConfiguration : IEntityTypeConfiguration<Cell>
{
    public void Configure(EntityTypeBuilder<Cell> builder)
    {
        builder.HasKey(c => new { c.X, c.Y });
    }
}

