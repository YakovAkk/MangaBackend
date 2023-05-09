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

        public static bool IsValidPageAndPageSize(int pagesize, int page)
        {
            if (pagesize >= 0 && page >= 0)
                return true;

            return true;
        }
        public static bool IsValidYear(int year)
        {
            if (year < 0)            
                return false;
            return true;
        }
    }
}
