using HotelReservation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Infrastructure.Persistence.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.Name).HasMaxLength(100).IsRequired();
        builder.Property(h => h.Description).HasMaxLength(500);
        builder.Property(h => h.City).HasMaxLength(50).IsRequired();
        builder.Property(h => h.Address).HasMaxLength(150).IsRequired();

        // Relación: Un Hotel tiene un Dueño (User)
        builder.HasOne(h => h.Owner)
            .WithMany(u => u.OwnedHotels)
            .HasForeignKey(h => h.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}