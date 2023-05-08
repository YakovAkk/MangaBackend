using CorePush.Apple;
using CorePush.Google;
using Data.Database;
using EmailingService.Model;
using EmailingService.Services;
using EmailingService.Services.Base;
using FileService;
using MangaBackend.Middleware.Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using Services.FillerService;
using Services.FillerService.Base;
using Services.Model.Configuration;
using Services.NotificationService.Service;
using Services.NotificationService.Service.Base;
using Services.Services;
using Services.Services.Base;
using Services.Storage;
using Services.Storage.Base;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddTransient<IMangaService, MangaService>();
    builder.Services.AddTransient<IGenreService, GenreService>();
    builder.Services.AddTransient<IUserService, UserService>();
    builder.Services.AddTransient<IAuthService, AuthService>();
    builder.Services.AddSingleton<ILocalStorage, LocalStorage>();
    builder.Services.AddTransient<IFillerService, FillerService>();
    builder.Services.AddTransient<INotificationService, NotificationService>();

    builder.Services.AddHttpClient<FcmSender>();
    builder.Services.AddHttpClient<ApnSender>();

    // Add email Conf
    builder.Services.AddScoped<IEmailService, EmailService>();

    var tokenConfig = builder.Configuration
        .GetSection("TokenConfiguration")
        .Get<TokenConfiguration>();
    builder.Services.AddSingleton(tokenConfig);

    var appConfig = builder.Configuration
        .GetSection("ApplicationConfiguration")
        .Get<ApplicationConfiguration>();
    builder.Services.AddSingleton(appConfig);

    var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
    builder.Services.AddSingleton(emailConfig);

    // log version
    logger.Trace(new VersionFileConfig(logger).Version.ToString());

    builder.Services.AddDbContext<AppDBContext>(options =>
    {
        var configuration = builder.Configuration;

        var server = configuration["DB_SERVER"] ?? "DESKTOP-EL7FVD8";
        var dbName = configuration["DB_NAME"] ?? "MangaDB";
        var password = configuration["PASSWORD"] ?? "2!@Fsfsdfaa";

        var connectionString = $"Data Source={server};Initial Catalog={dbName};User Id=sa;Password={password};TrustServerCertificate=True;";

        Console.WriteLine(connectionString);

        options.UseSqlServer(connectionString);
    });

    builder.Services.AddControllers();

    builder.Services.AddControllersWithViews()
        .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Manga application API", Version = "v1" });

        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>();

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        options.IncludeXmlComments(xmlPath);
    });

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
        options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("TokenConfiguration:Token").Value)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    var app = builder.Build();

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
    LogManager.Shutdown();
}



