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
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            Message = "Endpoint protegido.",
            UserId = userId,
            Email = userEmail,
            Role = userRole
        });
    }
}