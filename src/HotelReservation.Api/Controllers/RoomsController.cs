using System.Security.Claims;
using FluentValidation;
using HotelReservation.Application.Rooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers;

[ApiController]
[Route("api/hotels/{hotelId:guid}/rooms")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly IValidator<CreateRoomRequest> _validator;

    public RoomsController(IRoomService roomService, IValidator<CreateRoomRequest> validator)
    {
        _roomService = roomService;
        _validator = validator;
    }

    [HttpPost]
    [Authorize(Roles = "HotelOwner,Administrator")]
    public async Task<IActionResult> Create([FromRoute] Guid hotelId, [FromBody] CreateRoomRequest request)
    {
        // 1. Validar request body
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new { Property = e.PropertyName, Error = e.ErrorMessage }));
        }

        // 2. Extraer el id del usuario desde el token
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var ownerId))
        {
            return Unauthorized(new { message = "Usuario no válido." });
        }

        try
        {
            // 3. Ejecutar servicio pasándole tanto el hotelId de la ruta como el ownerId del token
            var response = await _roomService.CreateAsync(hotelId, request, ownerId);
            return CreatedAtAction(nameof(GetRoomsByHotel), new { hotelId = hotelId }, response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            // Código de estado 403 Forbidden (Autenticado pero no tienes acceso a ese recurso específico)
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetRoomsByHotel([FromRoute] Guid hotelId)
    {
        try
        {
            var rooms = await _roomService.GetRoomsByHotelIdAsync(hotelId);
            return Ok(rooms);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}