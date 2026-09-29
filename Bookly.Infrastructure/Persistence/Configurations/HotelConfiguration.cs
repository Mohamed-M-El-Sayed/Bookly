using Bookly.Domain.Entities.Hotels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property(x => x.Country)
                .HasMaxLength(100)
                .IsRequired();

            address.Property(x => x.City)
                .HasMaxLength(100)
                .IsRequired();

            address.Property(x => x.Street)
                .HasMaxLength(200)
                .IsRequired();
        });
        builder.OwnsOne(x => x.ContactInfo, contact =>
        {
            contact.Property(x => x.Phone)
                .HasMaxLength(20)
                .IsRequired();

            contact.Property(x => x.Email)
                .HasMaxLength(200)
                .IsRequired();
        });

        builder.HasMany(x => x.RoomTypes)
        .WithOne(x => x.Hotel)
        .HasForeignKey(x => x.HotelId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
