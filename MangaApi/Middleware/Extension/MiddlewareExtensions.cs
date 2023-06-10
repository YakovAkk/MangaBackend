using MangaApi.Middleware.MiddlewareClasses;
using MangaBackend.Middleware.MiddlewareClasses;

namespace MangaBackend.Middleware.Extension
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseTiming(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TimingMiddleware>();
        }
        public static IApplicationBuilder UseExceptionsHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HandlingExceptionsMiddleware>();
        }
    }
}
