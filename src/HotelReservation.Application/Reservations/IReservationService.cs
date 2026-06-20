namespace HotelReservation.Application.Reservations;

public interface IReservationService
{
    Task<ReservationResponse> CreateAsync(CreateReservationRequest request, Guid userId);
    Task<IEnumerable<ReservationResponse>> GetMyReservationsAsync(Guid userId);
}