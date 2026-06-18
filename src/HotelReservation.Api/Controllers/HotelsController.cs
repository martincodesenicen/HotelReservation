using System.Security.Claims;
using FluentValidation;
using HotelReservation.Application.Hotels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers;

[ApiController]
[Route("api/hotels")]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;
    private readonly IValidator<CreateHotelRequest> _validator;

    public HotelsController(IHotelService hotelService, IValidator<CreateHotelRequest> validator)
    {
        _hotelService = hotelService;
        _validator = validator;
    }

    [HttpPost]
    [Authorize(Roles = "HotelOwner,Administrator")] // Solo dueños o admins pueden crear hoteles
    public async Task<IActionResult> Create([FromBody] CreateHotelRequest request)
    {
        // 1. Validar la petición
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new { Property = e.PropertyName, Error = e.ErrorMessage }));
        }

        // 2. Extraer el UserId del Claim del Token JWT actual
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var ownerId))
        {
            return Unauthorized(new { message = "Usuario no válido en el token." });
        }

        // 3. Ejecutar servicio
        var response = await _hotelService.CreateAsync(request, ownerId);
        return CreatedAtAction(nameof(GetAll), new { id = response.Id }, response);
    }

    [HttpGet]
    [AllowAnonymous] // Cualquiera puede listar los hoteles, esté logueado o no
    public async Task<IActionResult> GetAll()
    {
        var hotels = await _hotelService.GetAllAsync();
        return Ok(hotels);
    }
}