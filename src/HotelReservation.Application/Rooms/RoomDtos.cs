using HotelReservation.Domain.Enums;

namespace HotelReservation.Application.Rooms;

// Request para crear la habitación (El HotelId vendrá por la URL en la API)
public record CreateRoomRequest(string RoomNumber, int Capacity, decimal PricePerNight, RoomType RoomType);

// Response de salida
public record RoomResponse(Guid Id, Guid HotelId, string RoomNumber, int Capacity, decimal PricePerNight, string RoomType, bool IsAvailable);