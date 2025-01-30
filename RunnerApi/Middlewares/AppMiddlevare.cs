namespace RunnerApi.Middlewares;

/// <summary>
/// 
/// </summary>
public class AppMiddleware : IMiddleware
{
    private readonly ILogger<AppMiddleware> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    public AppMiddleware(ILogger<AppMiddleware> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);  // Переход к следующему middleware
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync($"Internal server error");
        }
    }
}