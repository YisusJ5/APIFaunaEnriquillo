using Microsoft.AspNetCore.Http;

namespace APIFaunaEnriquillo.Middleware
{
    public class FileValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private const long MaxFileSize = 2 * 1024 * 1024; 

        public FileValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.HasFormContentType && context.Request.Form.Files.Count > 0)
            {
                foreach (var file in context.Request.Form.Files)
                {
                    if (!file.ContentType.StartsWith("image/"))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Solo se permiten archivos de imagen.");
                        return;
                    }
                    if (file.Length > MaxFileSize)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("El archivo excede el tamaño máximo permitido (2 MB).");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
