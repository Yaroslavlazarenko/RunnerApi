using Microsoft.EntityFrameworkCore;

namespace RunnerApi.Middlewares;

/// <summary>
/// 
/// </summary>
public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="next"></param>
    /// <param name="logger"></param>
    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpContext"></param>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);  // Переход к следующему middleware
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Argument null exception occurred.");
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsync($"Bad Request: {ex.Message}");
        }
        
        
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred.");
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsync($"Database error: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsync($"Internal server error: {ex.Message}");
        }
    }
}