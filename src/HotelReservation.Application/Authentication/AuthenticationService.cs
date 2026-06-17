using HotelReservation.Application.Common.Interfaces;
using HotelReservation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Application.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthenticationService(
        IApplicationDbContext context, 
        IPasswordHasher passwordHasher, 
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest request)
    {
        // 1. Validar si el email ya existe
        var userExists = await _context.Users.AnyAsync(u => u.Email == request.Email);
        if (userExists)
        {
            throw new Exception("Email is already registered."); // Temporal: Luego usaremos Global Exception Handling
        }

        // 2. Hashear password
        var passwordHash = _passwordHasher.HashPassword(request.Password);

        // 3. Crear entidad de dominio
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = passwordHash,
            Role = request.Role
        };

        // 4. Persistir en Base de Datos
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // 5. Generar Token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResponse(user.Id, user.FirstName, user.LastName, user.Email, user.Role.ToString(), token);
    }

    public async Task<AuthenticationResponse> LoginAsync(LoginRequest request)
    {
        // 1. Buscar usuario por email
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
        {
            throw new Exception("Invalid credentials.");
        }

        // 2. Verificar password
        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new Exception("Invalid credentials.");
        }

        // 3. Generar Token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResponse(user.Id, user.FirstName, user.LastName, user.Email, user.Role.ToString(), token);
    }
}