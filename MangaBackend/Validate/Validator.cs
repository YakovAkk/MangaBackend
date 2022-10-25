using NLog;
using NLog.Web;

namespace MangaBackend.Validate;

public class Validator
{
    private readonly IConfiguration _configuration;
    private readonly NLog.ILogger _logger;

    public Validator(IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
    }

    public async Task<bool> ValidateAppsettingsJson()
    {
        var connercionStringMsSql = _configuration.GetSection("ConnectionStrings")["LocalDatabaseMSSQL"];
        var connercionStringMySql = _configuration.GetSection("ConnectionStrings")["LocalDatabaseMYSQL"];

        var typeOfConnection = _configuration.GetSection("Others")["TypeOfConnection"];
        var relativePath = _configuration.GetSection("Others")["RelativePath"];

        var SenderId = _configuration.GetSection("FcmNotification")["SenderId"];
        var ServerKey = _configuration.GetSection("FcmNotification")["ServerKey"];



        if (connercionStringMsSql == null)
        {
            _logger.Error($"Section ConnercionStringMsSql should be existed at the json file");
        }

        if (connercionStringMySql == null)
        {
            _logger.Error($"Section ConnercionStringMySql should be existed at the json file");
        }

        if (typeOfConnection == null)
        {
            _logger.Error($"Section TypeOfConnection should be existed at the json file");
        }

        if (relativePath == null)
        {
            _logger.Error($"Section RelativePath should be existed at the json file");
        }
        if (SenderId == null)
        {
            _logger.Error($"Section SenderId should be existed at the json file");
        }
        if (ServerKey == null)
        {
            _logger.Error($"Section ServerKey should be existed at the json file");
        }

        if (connercionStringMsSql == null || connercionStringMySql == null
            || typeOfConnection == null || relativePath == null || SenderId == null || ServerKey == null)
        {
            return false;
        }

        return true;
    }
}
