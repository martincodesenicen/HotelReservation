using System.Security.Claims;
using FluentValidation;
using HotelReservation.Application.Reservations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers;

[ApiController]
[Route("api/reservations")]
[Authorize] // Todo el controlador requiere estar logueado
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IValidator<CreateReservationRequest> _validator;

    public ReservationsController(IReservationService reservationService, IValidator<CreateReservationRequest> validator)
    {
        _reservationService = reservationService;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new { Property = e.PropertyName, Error = e.ErrorMessage }));
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Usuario no válido." });
        }

        try
        {
            var response = await _reservationService.CreateAsync(request, userId);
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("my-history")]
    public async Task<IActionResult> GetMyHistory()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Usuario no válido." });
        }

        var history = await _reservationService.GetMyReservationsAsync(userId);
        return Ok(history);
    }
}