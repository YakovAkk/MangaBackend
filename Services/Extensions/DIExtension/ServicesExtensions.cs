using CorePush.Apple;
using CorePush.Google;
using Microsoft.Extensions.DependencyInjection;
using Services.Services.Base;
using Services.Services;
using Services.NotifyService.Service.Base;
using Services.NotifyService.Service;
using EmailingService.Services.Base;
using EmailingService.Services;

namespace Services.Extensions.DIExtension
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IMangaService, MangaService>();
            services.AddTransient<IGenreService, GenreService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IFillerService, FillerService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddHttpClient<FcmSender>();
            services.AddHttpClient<ApnSender>();

            return services;
        }
    }
}
