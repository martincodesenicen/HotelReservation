using HotelReservation.Application.Authentication;
using Microsoft.Extensions.DependencyInjection;
using HotelReservation.Application.Hotels;
using HotelReservation.Application.Rooms;
using System.Reflection;
using FluentValidation;

namespace HotelReservation.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IHotelService, HotelService>();
        services.AddScoped<IRoomService, RoomService>();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}