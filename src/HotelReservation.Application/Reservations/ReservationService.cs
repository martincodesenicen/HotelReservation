using HotelReservation.Application.Common.Interfaces;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Application.Reservations;

public class ReservationService : IReservationService
{
    private readonly IApplicationDbContext _context;

    public ReservationService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ReservationResponse> CreateAsync(CreateReservationRequest request, Guid userId)
    {
        // 1. Obtener la habitación y el hotel asociado
        var room = await _context.Rooms
            .Include(r => r.Hotel)
            .FirstOrDefaultAsync(r => r.Id == request.RoomId);

        if (room == null)
        {
            throw new KeyNotFoundException("La habitación especificada no existe.");
        }

        // 2. ALGORITMO: Verificar si hay superposición de fechas
        var hasOverlap = await _context.Reservations
            .AnyAsync(res => res.RoomId == request.RoomId &&
                             res.Status != ReservationStatus.Cancelled &&
                             res.CheckInDate < request.CheckOutDate &&
                             res.CheckOutDate > request.CheckInDate);

        if (hasOverlap)
        {
            throw new InvalidOperationException("La habitación no está disponible para las fechas seleccionadas.");
        }

        // 3. Calcular noches y precio total
        int totalNights = (request.CheckOutDate.Date - request.CheckInDate.Date).Days;
        decimal totalPrice = totalNights * room.PricePerNight;

        // 4. Crear la reserva
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            RoomId = request.RoomId,
            CheckInDate = request.CheckInDate.Date,
            CheckOutDate = request.CheckOutDate.Date,
            TotalPrice = totalPrice,
            Status = ReservationStatus.Confirmed // La confirmamos directamente para el MVP
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        return new ReservationResponse(
            reservation.Id,
            room.Id,
            room.RoomNumber,
            room.Hotel.Name,
            reservation.CheckInDate,
            reservation.CheckOutDate,
            reservation.TotalPrice,
            reservation.Status.ToString());
    }

    public async Task<IEnumerable<ReservationResponse>> GetMyReservationsAsync(Guid userId)
    {
        return await _context.Reservations
            .Include(res => res.Room)
            .ThenInclude(r => r.Hotel)
            .Where(res => res.UserId == userId)
            .Select(res => new ReservationResponse(
                res.Id,
                res.RoomId,
                res.Room.RoomNumber,
                res.Room.Hotel.Name,
                res.CheckInDate,
                res.CheckOutDate,
                res.TotalPrice,
                res.Status.ToString()))
            .ToListAsync();
    }
}