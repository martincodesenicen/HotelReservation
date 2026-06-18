using HotelReservation.Application.Common.Interfaces;
using HotelReservation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Application.Hotels;

public class HotelService : IHotelService
{
    private readonly IApplicationDbContext _context;

    public HotelService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<HotelResponse> CreateAsync(CreateHotelRequest request, Guid ownerId)
    {
        var hotel = new Hotel
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            City = request.City,
            Address = request.Address,
            OwnerId = ownerId
        };

        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();

        return new HotelResponse(hotel.Id, hotel.Name, hotel.Description, hotel.City, hotel.Address, hotel.OwnerId);
    }

    public async Task<IEnumerable<HotelResponse>> GetAllAsync()
    {
        return await _context.Hotels
            .Select(h => new HotelResponse(h.Id, h.Name, h.Description, h.City, h.Address, h.OwnerId))
            .ToListAsync();
    }
}