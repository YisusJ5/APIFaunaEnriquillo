using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace APIFaunaEnriquillo.Middleware
{
    public class GlobalExceptioncs
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptioncs> _logger;

        public GlobalExceptioncs(RequestDelegate next, ILogger<GlobalExceptioncs> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción no controlada");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    title = "Ocurrió un error inesperado",
                    status = context.Response.StatusCode,
                    detail = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
