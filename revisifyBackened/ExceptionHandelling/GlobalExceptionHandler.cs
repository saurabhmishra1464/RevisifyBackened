using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using revisifyBackened.ExceptionHandelling.CustomException;

namespace revisifyBackened.ExceptionHandelling;

internal sealed class GlobalExceptionHandler : IExceptionHandler
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
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        ApiResponse<string> response = exception switch
        {
            UnauthorizedAccessException => new ApiResponse<string>("Unauthorized access.", StatusCodes.Status401Unauthorized),
            KeyNotFoundException => new ApiResponse<string>("Resource not found.", StatusCodes.Status404NotFound),
            ArgumentException => new ApiResponse<string>("Invalid argument.", StatusCodes.Status400BadRequest),
            InvalidOperationException => new ApiResponse<string>("A user with this email address already exists.", StatusCodes.Status409Conflict),
            EmailNotConfirmedException => new ApiResponse<string>("Email not confirmed. Please confirm your email to proceed.", StatusCodes.Status400BadRequest),
            UserNotFoundException => new ApiResponse<string>("User Not Found", StatusCodes.Status404NotFound),
            QuestionNotFoundException => new ApiResponse<string>("Question Not Found", StatusCodes.Status404NotFound),
            _ => new ApiResponse<string>("An unexpected error occurred. Please try again later.", StatusCodes.Status500InternalServerError)
        };

        httpContext.Response.StatusCode = response.StatusCode;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}
