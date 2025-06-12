using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Middleware;

public class FileValidationMiddlewareTest
{
    [Fact]
    public async Task InvokeAsync_Returns400_WhenFileIsNotImage()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var formFile = new FormFile(new MemoryStream(new byte[10]), 0, 10, "file", "test.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };
        var formCollection = new FormCollection(null, new FormFileCollection { formFile });
        context.Request.ContentType = "multipart/form-data";
        context.Request.Form = formCollection;

        var middleware = new FileValidationMiddleware((innerHttpContext) => Task.CompletedTask);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_CallsNext_WhenFileIsImageAndSizeIsValid()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var formFile = new FormFile(new MemoryStream(new byte[10]), 0, 10, "file", "test.jpg")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg"
        };
        var formCollection = new FormCollection(null, new FormFileCollection { formFile });
        context.Request.ContentType = "multipart/form-data";
        context.Request.Form = formCollection;

        var wasCalled = false;
        var middleware = new FileValidationMiddleware((innerHttpContext) =>
        {
            wasCalled = true;
            return Task.CompletedTask;
        });

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(wasCalled);
        Assert.NotEqual(StatusCodes.Status400BadRequest, context.Response.StatusCode);
    }
}
