using Data.Database;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories;
using Repositories.Repositories.Base;
using Services.Services;
using Services.Services.Base;
using NLog;
using NLog.Web;
using Services.Storage;
using Services.Storage.Base;
using MangaBackend.Validate;
using Services.FillerService.Base;
using Services.FillerService;

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

    builder.Services.AddSingleton<ILocalStorage, LocalStorage>();


    builder.Services.AddTransient<IFillerSwervice, FillerService>();
    

    var validator = new Validator(builder.Configuration);

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
    builder.Services.AddSwaggerGen();

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

    app.UseHttpsRedirection();

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



