using CleanTeeth.Application.Exceptions;
using CleanTeeth.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CleanTeeth.API.ExceptionHandling;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is BadHttpRequestException badHttpRequestException)
        {
            _logger.LogWarning(
                badHttpRequestException,
                "Bad HTTP request.");

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad request",
                Detail = badHttpRequestException.Message,
                Instance = httpContext.Request.Path
            };
            httpContext.Response.StatusCode =
                StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken);

            return true;
        }
        if (exception is ApplicationValidationException validationException)
        {
            _logger.LogWarning(
                validationException,
                "Validation error occurred.");

            var validationProblemDetails =
                new ValidationProblemDetails(validationException.Errors)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation failed",
                    Instance = httpContext.Request.Path
                };

            httpContext.Response.StatusCode =
                StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(
                validationProblemDetails,
                cancellationToken);

            return true;
        }
        if (exception is BusinessRuleException businessRuleException)
        {
            _logger.LogWarning(
                businessRuleException,
                "Business rule violation.");

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Business Rule Violation",
                Detail = businessRuleException.Message,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode =
                StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken);

            return true;
        }

        if (exception is NotFoundException notFoundException)
        {
            _logger.LogWarning(
                notFoundException,
                "Resource not found.");

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Detail = notFoundException.Message,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode =
                StatusCodes.Status404NotFound;

            await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken);

            return true;
        }
        _logger.LogError(
            exception,
            "An unexpected error occurred.");

        var unexpectedProblemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred.",
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            unexpectedProblemDetails,
            cancellationToken);

        return true;
    }
}