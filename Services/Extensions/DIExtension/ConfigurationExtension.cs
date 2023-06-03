using EmailingService.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Model.Configuration;

namespace Services.Extensions.DIExtension
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenConfig = configuration
                .GetSection("TokenConfiguration")
                .Get<TokenConfiguration>();
            services.AddSingleton(tokenConfig);

            var appConfig = configuration
                .GetSection("ApplicationConfiguration")
                .Get<ApplicationConfiguration>();
            services.AddSingleton(appConfig);

            var emailConfig = configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            return services;
        }
    }
}
