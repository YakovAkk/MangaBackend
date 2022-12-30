﻿using Microsoft.Extensions.Logging;
using Repositories.LogsTools.Base;

namespace Repositories.LogsTools
{
    public class Tool : ITool
    {
        private readonly ILogger<Tool> _logger;
        public string NameOfMethod { get; set; }

        public Tool(ILogger<Tool> logger)
        {
            _logger = logger;
        }

        public void WriteToLog(ILogger logger, LogPosition logPosition, string message = "")
        {
            if(logger == null)
            {
                _logger.LogError("Logger is null");
                throw new Exception(nameof(logger));
            }

            switch (logPosition)
            {
                case LogPosition.Begin:
                case LogPosition.End:
                    logger.LogDebug($"{NameOfMethod} {logPosition} with {message}");
                    break;
                case LogPosition.Error:
                    logger.LogError($"{NameOfMethod} ended with {logPosition.ToString()}: {message}");
                    break;
                default:
                    break;
            }
        }
    }
}