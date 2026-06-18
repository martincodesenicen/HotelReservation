namespace HotelReservation.Application.Hotels;

public interface IHotelService
{
    Task<HotelResponse> CreateAsync(CreateHotelRequest request, Guid ownerId);
    Task<IEnumerable<HotelResponse>> GetAllAsync();
}