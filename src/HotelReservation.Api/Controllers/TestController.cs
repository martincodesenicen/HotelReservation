using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelReservationSystem.Api.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet("protected")]
    [Authorize] // Este atributo bloquea a cualquiera que no envíe un token válido
    public IActionResult GetProtectedData()
    {
        // Podemos leer los claims del usuario logueado directamente desde el HttpContext
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            Message = "¡Felicidades! Accediste a un endpoint protegido.",
            UserId = userId,
            Email = userEmail,
            Role = userRole
        });
    }
}