using System.Net;
using System.Text.Json;
using static StockFlow.Common.Constants.Constant;

namespace StockFlow.API.Middlewares;
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
     
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ErrorMessages.UnhandledException);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        string message = exception switch
        {
            KeyNotFoundException => ErrorMessages.ResourceNotFound,
            _ => ErrorMessages.InternalServerError
        };

        string result = JsonSerializer.Serialize(new { error = message });
        return context.Response.WriteAsync(result);
    }
}
// Extension method used to add the middleware to the HTTP request pipeline.
public static class MiddlewareExtensions
{
    public static void UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionMiddleware>();
    }
}
