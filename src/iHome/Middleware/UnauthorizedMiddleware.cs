using Microsoft.Extensions.Logging;

namespace iHome.Middleware;

public class UnauthorizedMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UnauthorizedMiddleware> _logger;

    public UnauthorizedMiddleware(RequestDelegate next, ILogger<UnauthorizedMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (UnauthorizedAccessException exception)
        {
            _logger.LogError(exception, "Unauthorized access: {Context}", httpContext.Request.Path.Value);
            var response = httpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = 401;
            await response.WriteAsync("Unauthorized access");
        }

    }
}
