using HotelReservation.Domain.Enums;

namespace HotelReservation.Application.Authentication;

// DTOs de Entrada (Requests)
public record RegisterRequest(string FirstName, string LastName, string Email, string Password, UserRole Role);
public record LoginRequest(string Email, string Password);

// DTO de Salida (Response)
public record AuthenticationResponse(Guid Id, string FirstName, string LastName, string Email, string Role, string Token);