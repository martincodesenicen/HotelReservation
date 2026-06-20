using FluentValidation;

namespace HotelReservation.Application.Reservations;

public class CreateReservationRequestValidator : AbstractValidator<CreateReservationRequest>
{
    public CreateReservationRequestValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("El ID de la habitación es obligatorio.");

        RuleFor(x => x.CheckInDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("La fecha de Check-In no puede ser en el pasado.");

        RuleFor(x => x.CheckOutDate)
            .GreaterThan(x => x.CheckInDate)
            .WithMessage("La fecha de Check-Out debe ser posterior a la fecha de Check-In.");
    }
}