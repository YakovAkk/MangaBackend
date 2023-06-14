using Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Shared.Configuration;

namespace Services.Extensions.DbExtension
{
    public static class DbContextExtension
    {
        public static IServiceCollection AddDB(this IServiceCollection services, IConfiguration configuration)
        {
            var othersConfig = configuration
                .GetSection("Others")
                .Get<OthersConfiguration>();
            services.AddSingleton(othersConfig);

            if (othersConfig.IsUseEnviromentVariables)
            {
                services.AddDbContext<AppDBContext>(options =>
                {
                    var server = configuration["DB_SERVER"] ?? "DESKTOP-EL7FVD8";
                    var dbName = configuration["DB_NAME"] ?? "MangaDB";
                    var password = configuration["PASSWORD"] ?? "2!@Fsfsdfaa";
                    var connectionString = $"Data Source={server};Initial Catalog={dbName};User Id=sa;Password={password};TrustServerCertificate=True;";

                    options.UseSqlServer(connectionString);
                });
            }
            else
            {
                if (othersConfig.TypeOfConnection.Contains("MSSQL"))
                {
                    services.AddDbContext<AppDBContext>(options =>
                    {
                        options.UseSqlServer(configuration.GetConnectionString(othersConfig.TypeOfConnection));
                    });
                }
                else if (othersConfig.TypeOfConnection.Contains("MYSQL"))
                {
                    services.AddDbContext<AppDBContext>(options =>
                    {
                        options.UseMySql(configuration.GetConnectionString(othersConfig.TypeOfConnection), new MySqlServerVersion(new Version("8.0.33")));
                    });
                }
            }
            return services;
        }
    }
}
