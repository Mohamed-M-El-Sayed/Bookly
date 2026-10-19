using Bookly.Domain.Entities.Amenities;
using Bookly.Domain.Entities.Bookings;
using Bookly.Domain.Entities.Hotels;
using Bookly.Domain.Entities.Identity;
using Bookly.Domain.Entities.RoomTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Infrastructure.Persistence.Context;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>

{

    public DbSet<RoomType> RoomTypes => Set<RoomType>();
    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingRoom> BookingRooms => Set<BookingRoom>();
    public DbSet<RoomAvailability> RoomAvailabilities => Set<RoomAvailability>();
    public DbSet<PasswordHistory> PasswordHistories => Set<PasswordHistory>();
    public DbSet<Amenity> Amenities => Set<Amenity>();
    public DbSet<AmenityCategory> AmenityCategories => Set<AmenityCategory>();
    public DbSet<HotelManager> HotelManagers => Set<HotelManager>();
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}
