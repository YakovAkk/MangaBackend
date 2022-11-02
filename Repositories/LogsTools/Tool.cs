using Microsoft.Extensions.Logging;
using Repositories.LogsTools.Base;

namespace Repositories.LogsTools
{
    public class Tool : ITool
    {
        private readonly ILogger<Tool> _logger;
        public string NameOfMethod
        {
            get
            {
                return NameOfMethod;
            }

            set
            {
                if (value == null)
                {
                    _logger.LogError("Name is null");
                    throw new ArgumentNullException("Name can't be null");
                }
                NameOfMethod = value;
            } }
        public Tool(ILogger<Tool> logger)
        {
            _logger = logger;
        }

        public void WriteToLog(ILogger logger, LogPosition logPosition, string result = "", string parameters="")
        {
            if(logger == null)
            {
                _logger.LogError("Logger is null");
                throw new Exception(nameof(logger));
            }
            if(logPosition == null)
            {
                _logger.LogError("LogPosition is null");
                throw new Exception("LogPosition is null");
            }
            switch (logPosition)
            {
                case LogPosition.Begin:
                    logger.LogDebug($"{NameOfMethod} was begun with parameters: {parameters}");
                    break;
                case LogPosition.End:
                    logger.LogDebug($"{NameOfMethod} was ended with result: {result}");
                    break;
                default:
                    break;
            }
        }
    }
}
