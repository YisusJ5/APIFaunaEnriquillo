using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Middleware;

public class GlobalExceptioncsTest
{
    [Fact]
    public async Task InvokeAsync_Returns500_WhenExceptionIsThrown()
    {
        var context = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<GlobalExceptioncs>>();
        var middleware = new GlobalExceptioncs((ctx) => throw new Exception("Test error"), loggerMock.Object);

        var responseStream = new MemoryStream();
        context.Response.Body = responseStream;
        await middleware.InvokeAsync(context);

        
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        responseStream.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(responseStream).ReadToEndAsync();
        using var json = JsonDocument.Parse(responseBody);
        var title = json.RootElement.GetProperty("title").GetString();
        var detail = json.RootElement.GetProperty("detail").GetString();
        Assert.Equal("Ocurrió un error inesperado", title);
        Assert.Contains("Test error", detail);

    }
}
