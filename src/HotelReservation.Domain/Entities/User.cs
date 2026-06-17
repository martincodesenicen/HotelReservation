using HotelReservation.Domain.Enums;
namespace HotelReservation.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }

    // Un usuario tiene muchos hoteles
    public List<Hotel> OwnedHotels { get; set; } = [];
    // Un usuario tiene muchas reservas
    public List<Reservation> Reservations { get; set; } = [];
}