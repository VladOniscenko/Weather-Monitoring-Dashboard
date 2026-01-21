using System.Net;
using System.Text.Json;
using Weather.Application.Common.Models;

namespace Weather.API.Middleware;

public class GlobalExceptionMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = (int)HttpStatusCode.InternalServerError;
        var message = "An internal error occurred.";
        
        switch (exception)
        {
            // 401 Unauthorized
            case UnauthorizedAccessException: 
                statusCode = (int)HttpStatusCode.Unauthorized;
                message = exception.Message;
                break;

            // 404 Not Found
            case KeyNotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                message = "The requested resource was not found.";
                break;

            // 400 Bad Request
            case ArgumentException:
            case InvalidOperationException:
                statusCode = (int)HttpStatusCode.BadRequest;
                message = exception.Message;
                break;
        }

        context.Response.StatusCode = statusCode;

        // Wrap the error in your standard ApiResponse format
        var response = ApiResponse<object>.FailureResponse(message);
        
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}