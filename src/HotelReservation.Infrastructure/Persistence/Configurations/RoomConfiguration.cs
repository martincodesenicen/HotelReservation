using HotelReservation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Infrastructure.Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.RoomNumber).HasMaxLength(10).IsRequired();
        builder.Property(r => r.Capacity).IsRequired();
        
        builder.Property(r => r.PricePerNight).HasColumnType("decimal(18,2)").IsRequired();
        
        builder.Property(r => r.RoomType).HasConversion<int>().IsRequired();
        builder.Property(r => r.IsAvailable).IsRequired();

        // Relación: Una Habitación pertenece a un Hotel. Si el Hotel se elimina, sus habitaciones sí se eliminan en cascada.
        builder.HasOne(r => r.Hotel)
            .WithMany(h => h.Rooms)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}