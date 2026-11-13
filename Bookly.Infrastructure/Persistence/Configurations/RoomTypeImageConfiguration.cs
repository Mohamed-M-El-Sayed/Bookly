using Bookly.Domain.Entities.RoomTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations;

public class RoomTypeImageConfiguration : IEntityTypeConfiguration<RoomTypeImage>
{
    public void Configure(EntityTypeBuilder<RoomTypeImage> builder)
    {
        builder.Property(x => x.Url)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne<RoomType>()
            .WithMany(x => x.Images)
            .HasForeignKey(x => x.RoomTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
