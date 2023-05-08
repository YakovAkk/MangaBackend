using Microsoft.Extensions.Configuration;
using NLog;

namespace ValidateService.Validate
{
    public class ValidatorService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public ValidatorService(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<bool> ValidateAppSettingsJson()
        {
            var relativePath = _configuration.GetSection("Others")["RelativePath"];

            var SenderId = _configuration.GetSection("FcmNotification")["SenderId"];
            var ServerKey = _configuration.GetSection("FcmNotification")["ServerKey"];


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

            return true;
        }
        public static bool IsValidPageAndPageSize(int pagesize, int page)
        {
            bool isValid = true;

            if (pagesize > 0 && page > 0)
                return true;

            return isValid;
        }
        public static bool IsValidYear(string year, out int yearnum)
        {
            bool isValid = true;
            var IsCanParseYear = Int32.TryParse(year, out yearnum);

            if (!IsCanParseYear || yearnum < 0)
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
