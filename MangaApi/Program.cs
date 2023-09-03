using FileService;
using MangaBackend.Middleware.Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using System.Text;
using Services.Extensions.DIExtension;
using Services.Extensions.DbExtension;
using System.Reflection;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddConfigurations(builder.Configuration);
    builder.Services.AddServices();
    builder.Services.AddDB(builder.Configuration);

    // log version
    logger.Trace(new VersionFileConfig(logger).Version.ToString());

    builder.Services.AddControllers();

    builder.Services.AddControllersWithViews()
        .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerConfiguration(Assembly.GetExecutingAssembly().GetName().Name);

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

    app.UseExceptionsHandler();

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



