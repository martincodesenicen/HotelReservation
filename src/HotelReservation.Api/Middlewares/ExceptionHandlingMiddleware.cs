using System.Net;
using System.Text.Json;

namespace HotelReservation.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Continuar con el siguiente middleware en el pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            // Si algo falla en cualquier capa inferior, cae acá
            _logger.LogError(ex, "Ocurrió una excepción no controlada: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError; // Por defecto 500
        var result = string.Empty;

        // Evaluamos el tipo de excepción para asignar el status code correcto
        switch (exception)
        {
            case KeyNotFoundException:
                code = HttpStatusCode.NotFound; // 404
                result = JsonSerializer.Serialize(new { error = exception.Message });
                break;

            case UnauthorizedAccessException:
                code = HttpStatusCode.Forbidden; // 403
                result = JsonSerializer.Serialize(new { error = exception.Message });
                break;

            case InvalidOperationException:
                code = HttpStatusCode.BadRequest; // 400
                result = JsonSerializer.Serialize(new { error = exception.Message });
                break;

            default:
                // Error 500 genérico (no le mostramos el rastro del código al cliente por seguridad)
                result = JsonSerializer.Serialize(new { error = "Ocurrió un error interno en el servidor. Intente más tarde." });
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}