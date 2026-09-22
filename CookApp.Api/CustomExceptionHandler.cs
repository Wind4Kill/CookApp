using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Model.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CookApp.Api
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, message) = exception switch
            {
                EntityNotFoundException => (StatusCodes.Status400BadRequest, "Resource Not Found."),
                UserNotFound => (StatusCodes.Status404NotFound, "User Not Found."),
                UserWrongDataException => (StatusCodes.Status400BadRequest, "User Wrong Input Data."),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error.")
            };

            ProblemDetails details = new ProblemDetails()
            {
                Title = message,
                Status = statusCode,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);

            return true;
            
        }
    }
}