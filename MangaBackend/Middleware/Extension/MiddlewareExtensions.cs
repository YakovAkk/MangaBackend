using MangaBackend.Middleware.MiddlewareClasses;

namespace MangaBackend.Middleware.Extension
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseResponseTime(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ResponseTimeMiddleware>();
        }
    }
}
