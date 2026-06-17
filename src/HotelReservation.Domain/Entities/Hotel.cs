namespace HotelReservation.Domain.Entities;

public class Hotel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    
    // Relación con el dueño
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    // Un hotel tiene muchas habitaciones
    public List<Room> Rooms { get; set; } = [];
}