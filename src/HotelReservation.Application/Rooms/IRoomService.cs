namespace HotelReservation.Application.Rooms;

public interface IRoomService
{
    Task<RoomResponse> CreateAsync(Guid hotelId, CreateRoomRequest request, Guid ownerId);
    Task<IEnumerable<RoomResponse>> GetRoomsByHotelIdAsync(Guid hotelId);
}