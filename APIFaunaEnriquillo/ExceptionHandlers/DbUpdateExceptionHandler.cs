using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIFaunaEnriquillo.ExceptionHandlers
{
    public class DbUpdateExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not DbUpdateException dbUpdateException) 
            {
                return true;
            
            }


            ProblemDetails problemDetails = new ProblemDetails()
            {
                Title = "An Error occurred",
                Status = StatusCodes.Status400BadRequest,
                Detail = "The request could not be processed due to a record already exists with the same ID.",
                Type = exception.GetType().Name,
                Instance = $"{httpContext.Request.Method}{httpContext.Request.Path}",
                Extensions = { ["errors"] = dbUpdateException.Message }


            };

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;



        }
    }

}
