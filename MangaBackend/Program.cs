using Microsoft.EntityFrameworkCore;
using Repositories.Repositories;
using Repositories.Repositories.Base;
using Services.Services;
using Services.Services.Base;
using NLog;
using NLog.Web;
using Services.Storage;
using Services.Storage.Base;
using Data.Database;
using Services.FillerService.Base;
using Services.FillerService;
using Services.NotificationService.Service.Base;
using Services.NotificationService.Service;
using CorePush.Google;
using CorePush.Apple;
using MangaBackend.Middleware.Extension;
using ValidateService.Validate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddTransient<IMangaRepository, MangaRepository>();
    builder.Services.AddTransient<IMangaService, MangaService>();

    builder.Services.AddTransient<IGenreRepository, GenreRepository>();
    builder.Services.AddTransient<IGenreService, GenreService>();

    builder.Services.AddTransient<IUserRespository, UserRepository>();
    builder.Services.AddTransient<IUserService, UserService>();

    builder.Services.AddTransient<IAuthService, AuthService>();

    builder.Services.AddSingleton<ILocalStorage, LocalStorage>();

    builder.Services.AddTransient<IFillerService, FillerService>();

    builder.Services.AddTransient<INotificationService, NotificationService>();

    builder.Services.AddHttpClient<FcmSender>();
    builder.Services.AddHttpClient<ApnSender>();

    var validator = new ValidatorService(builder.Configuration, logger);

    if (await validator.ValidateAppsettingsJson())
    {
        try
        {
            var typeOfConnection = builder.Configuration.GetSection("Others")["TypeOfConnection"];

            switch (typeOfConnection)
            {
                case "LocalDatabaseMYSQL":
                    {
                        builder.Services.AddDbContext<AppDBContent>(options =>
                        options.UseMySQL(builder.Configuration.GetConnectionString(typeOfConnection)).EnableSensitiveDataLogging());
                        logger.Debug("Conected was successfully completed. Connection String : " + builder.Configuration.GetConnectionString(typeOfConnection));
                        break;
                    }
                case "LocalDatabaseMSSQL":
                    {
                        builder.Services.AddDbContext<AppDBContent>(options =>
                        options.UseSqlServer(builder.Configuration.GetConnectionString(typeOfConnection)).EnableSensitiveDataLogging());
                        logger.Debug("Conected was successfully completed. Connection String : " + builder.Configuration.GetConnectionString(typeOfConnection));
                        break;
                    }
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            logger.Error("Conected was intrerupted. Connection String : " + builder.Configuration.GetConnectionString("LocalDatabaseMYSQL") + "/n" + ex.Message);
            throw;
        }
    }

    builder.Services.AddControllers();

    builder.Services.AddControllersWithViews()
        .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>();
    });
    // Authentication setup

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
        options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("TokenSettings:Token").Value)),
                ValidateIssuer = false,
                ValidateAudience = false
            };  
        });


    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


    app.UseCors(options =>
    {
        options.
        AllowAnyMethod().
        AllowAnyHeader().
        SetIsOriginAllowed(origin => true).
        AllowCredentials();

    });

    app.UseTiming();

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();
   
    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}



