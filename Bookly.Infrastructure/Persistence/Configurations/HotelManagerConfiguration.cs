using Bookly.Domain.Entities.Hotels;
using Bookly.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations;

public class HotelManagerConfiguration : IEntityTypeConfiguration<HotelManager>
{
    public void Configure(EntityTypeBuilder<HotelManager> builder)
    {
        builder.HasKey(x => new { x.HotelId, x.UserId });
        builder.HasOne(x => x.Hotel)
               .WithMany(h => h.Managers)
               .HasForeignKey(x => x.HotelId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.UserId);
    }
}
