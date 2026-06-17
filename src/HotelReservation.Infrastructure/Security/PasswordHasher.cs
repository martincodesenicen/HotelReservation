using HotelReservation.Application.Common.Interfaces;

namespace HotelReservation.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password) => 
        BCrypt.Net.BCrypt.EnhancedHashPassword(password, workFactor: 12);

    public bool VerifyPassword(string password, string passwordHash) => 
        BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
}