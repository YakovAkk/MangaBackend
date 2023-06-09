using MangaBackend.Middleware.MiddlewareClasses;
using System.Text.Json;
using WrapperService.Wrapper;

namespace MangaApi.Middleware.MiddlewareClasses
{
    public class HandlingExceptionsMiddleware 
    {
        public ILogger<HandlingExceptionsMiddleware> _logger { get; }
        public RequestDelegate _next { get; }
        public HandlingExceptionsMiddleware(ILogger<HandlingExceptionsMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured {ex.Message}");
                context.Response.StatusCode = 500;
                var result = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);

                var json = JsonSerializer.Serialize(result);

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
        }
    }
}
