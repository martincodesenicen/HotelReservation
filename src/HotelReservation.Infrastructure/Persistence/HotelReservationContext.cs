using HotelReservation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HotelReservation.Infrastructure.Persistence;

public class HotelReservationContext : DbContext
{
    public HotelReservationContext(DbContextOptions<HotelReservationContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}