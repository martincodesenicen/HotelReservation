using FluentValidation;

namespace HotelReservation.Application.Rooms;

public class CreateRoomRequestValidator : AbstractValidator<CreateRoomRequest>
{
    public CreateRoomRequestValidator()
    {
        RuleFor(x => x.RoomNumber)
            .NotEmpty().WithMessage("El número de habitación es obligatorio.")
            .MaximumLength(10).WithMessage("El número de habitación no puede superar los 10 caracteres.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("La capacidad debe ser de al menos 1 persona.");

        RuleFor(x => x.PricePerNight)
            .GreaterThan(0).WithMessage("El precio por noche debe ser mayor a 0.");

        RuleFor(x => x.RoomType)
            .IsInEnum().WithMessage("El tipo de habitación seleccionado no es válido.");
    }
}