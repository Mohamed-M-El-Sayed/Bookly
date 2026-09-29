using Bookly.Domain.Entities.Bookings;
using Bookly.Domain.Entities.Hotels;
using Bookly.Domain.Entities.RoomTypes;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Infrastructure.Persistence.Context;

public class AppDbContext : DbContext
{

    public DbSet<RoomType> RoomTypes => Set<RoomType>();
    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingRoom> BookingRooms => Set<BookingRoom>();
    public DbSet<RoomAvailability> RoomAvailabilities => Set<RoomAvailability>();
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
