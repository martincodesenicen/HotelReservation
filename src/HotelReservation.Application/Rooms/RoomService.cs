using HotelReservation.Application.Common.Interfaces;
using HotelReservation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Application.Rooms;

public class RoomService : IRoomService
{
    private readonly IApplicationDbContext _context;

    public RoomService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RoomResponse> CreateAsync(Guid hotelId, CreateRoomRequest request, Guid ownerId)
    {
        // 1. Validar que el hotel exista y que el ownerId coincida con el dueño real del hotel
        var hotel = await _context.Hotels
            .FirstOrDefaultAsync(h => h.Id == hotelId);

        if (hotel == null)
        {
            throw new KeyNotFoundException("El hotel especificado no existe.");
        }

        if (hotel.OwnerId != ownerId)
        {
            throw new UnauthorizedAccessException("No tienes permisos para agregar habitaciones a este hotel.");
        }

        // 2. Crear la entidad Room
        var room = new Room
        {
            Id = Guid.NewGuid(),
            HotelId = hotelId,
            RoomNumber = request.RoomNumber,
            Capacity = request.Capacity,
            PricePerNight = request.PricePerNight,
            RoomType = request.RoomType,
            IsAvailable = true
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return new RoomResponse(room.Id, room.HotelId, room.RoomNumber, room.Capacity, room.PricePerNight, room.RoomType.ToString(), room.IsAvailable);
    }

    public async Task<IEnumerable<RoomResponse>> GetRoomsByHotelIdAsync(Guid hotelId)
    {
        // Validar si el hotel existe antes de listar habitaciones
        var hotelExists = await _context.Hotels.AnyAsync(h => h.Id == hotelId);
        if (!hotelExists)
        {
            throw new KeyNotFoundException("El hotel especificado no existe.");
        }

        return await _context.Rooms
            .Where(r => r.HotelId == hotelId)
            .Select(r => new RoomResponse(r.Id, r.HotelId, r.RoomNumber, r.Capacity, r.PricePerNight, r.RoomType.ToString(), r.IsAvailable))
            .ToListAsync();
    }
}