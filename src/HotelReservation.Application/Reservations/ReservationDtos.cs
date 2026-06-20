namespace HotelReservation.Application.Reservations;

public record CreateReservationRequest(Guid RoomId, DateTime CheckInDate, DateTime CheckOutDate);

public record ReservationResponse(
    Guid Id, 
    Guid RoomId, 
    string RoomNumber, 
    string HotelName, 
    DateTime CheckInDate, 
    DateTime CheckOutDate, 
    decimal TotalPrice, 
    string Status);