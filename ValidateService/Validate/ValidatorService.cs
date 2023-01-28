using Microsoft.Extensions.Configuration;
using NLog;

namespace ValidateService.Validate
{
    public class ValidatorService
    {
        private readonly IConfiguration _configuration;
        private readonly NLog.ILogger _logger;

        public ValidatorService(IConfiguration configuration, NLog.ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
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
        public static bool IsValidPageAndPageSize(string pagesize, string page, out int pageSize, out int numberOfPage)
        {
            bool isValid = true;

            var IsCanParsePageSize = Int32.TryParse(pagesize, out pageSize);

            if (!IsCanParsePageSize && pageSize < 0)
            {
                isValid = false;
            }

            numberOfPage = 0;

            var IsCanParseNumberOfPage = Int32.TryParse(page, out numberOfPage);

            if (!IsCanParseNumberOfPage && numberOfPage < 0)
            {
                isValid = false;
            }

            return isValid;
        }
        public static bool IsValidYear(string year, out int yearnum)
        {
            bool isValid = true;
            var IsCanParsePageSize = Int32.TryParse(year, out yearnum);

            if (!IsCanParsePageSize && yearnum < 0)
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
