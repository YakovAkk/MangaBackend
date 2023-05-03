using MangaBackend.Middleware.MiddlewareClasses;

namespace MangaBackend.Middleware.Extension
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseTiming(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TimingMiddleware>();
        }

    }
}
