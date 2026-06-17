using HotelReservation.Domain.Enums;
namespace HotelReservation.Domain.Entities;

public class Room
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public Hotel Hotel { get; set; } = null!;
    
    public string RoomNumber { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
    public RoomType RoomType { get; set; }
    public bool IsAvailable { get; set; } = true;
    // Una habitación puede ser reservada muchas veces
    public List<Reservation> Reservations { get; set; } = [];
}