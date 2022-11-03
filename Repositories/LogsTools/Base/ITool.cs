using Microsoft.Extensions.Logging;

namespace Repositories.LogsTools.Base;

public interface ITool
{
    public void WriteToLog(ILogger logger, LogPosition logPosition, string message = "");
    public string NameOfMethod { get; set; }
}
