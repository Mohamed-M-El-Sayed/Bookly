using Bookly.Domain.Entities.RoomTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations.RoomTypes;

public class BoardOptionConfiguration : IEntityTypeConfiguration<BoardOption>
{
    public void Configure(EntityTypeBuilder<BoardOption> builder)
    {

        builder.Property(x => x.ExtraPricePerNight)
            .HasPrecision(18, 2);

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasQueryFilter(x => x.DeletedAt == null);
    }
}