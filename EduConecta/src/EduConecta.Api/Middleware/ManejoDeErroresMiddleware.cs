using EduConecta.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace EduConecta.Api.Middleware;

public class ManejoDeErroresMiddleware
{
    private readonly RequestDelegate _next;

    public ManejoDeErroresMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ReglaDeNegocioException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            var respuesta = JsonSerializer.Serialize(new { error = ex.Message });
            await context.Response.WriteAsync(respuesta);
        }
    }
}