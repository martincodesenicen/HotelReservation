using HotelReservation.Domain.Entities;
namespace HotelReservation.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}