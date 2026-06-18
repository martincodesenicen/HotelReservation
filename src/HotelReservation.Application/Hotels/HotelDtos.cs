namespace HotelReservation.Application.Hotels;

// Request para crear un hotel
public record CreateHotelRequest(string Name, string Description, string City, string Address);

// Response para retornar la información del hotel
public record HotelResponse(Guid Id, string Name, string Description, string City, string Address, Guid OwnerId);