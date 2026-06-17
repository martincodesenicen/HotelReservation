using HotelReservation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Infrastructure.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(res => res.Id);

        builder.Property(res => res.CheckInDate).IsRequired();
        builder.Property(res => res.CheckOutDate).IsRequired();
        builder.Property(res => res.TotalPrice).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(res => res.Status).HasConversion<int>().IsRequired();

        // Relación con Usuario
        builder.HasOne(res => res.User)
            .WithMany(u => u.Reservations)
            .HasForeignKey(res => res.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación con Habitación
        builder.HasOne(res => res.Room)
            .WithMany(r => r.Reservations)
            .HasForeignKey(res => res.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}