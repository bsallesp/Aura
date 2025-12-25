using Aesthetic.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Aesthetic.API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception, "An error occurred: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Detail = exception.Message // In production, maybe hide this
            };

            switch (exception)
            {
                case ValidationException validationEx:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Validation Failure";
                    problemDetails.Extensions["errors"] = validationEx.Errors;
                    break;
                case KeyNotFoundException:
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Title = "Resource not found";
                    break;
                case ArgumentException:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Bad request";
                    break;
                case InvalidOperationException:
                    problemDetails.Status = StatusCodes.Status409Conflict;
                    problemDetails.Title = "Conflict";
                    break;
                case UnauthorizedAccessException:
                    problemDetails.Status = StatusCodes.Status401Unauthorized;
                    problemDetails.Title = "Unauthorized";
                    break;
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
