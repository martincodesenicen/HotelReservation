using HotelReservation.Application.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservation.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        return services;
    }
}