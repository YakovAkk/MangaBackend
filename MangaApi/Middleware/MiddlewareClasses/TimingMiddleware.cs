namespace MangaBackend.Middleware.MiddlewareClasses;

public class TimingMiddleware 
{
    public ILogger<TimingMiddleware> _logger { get; }
    public RequestDelegate _next { get; }
    public TimingMiddleware(ILogger<TimingMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task Invoke(HttpContext content)
    {
        var start = DateTime.Now;
        await _next.Invoke(content);
        _logger.LogInformation($"Timing:  {content.Request.Path}: {(DateTime.Now - start).TotalMilliseconds} ms");
    }
}

