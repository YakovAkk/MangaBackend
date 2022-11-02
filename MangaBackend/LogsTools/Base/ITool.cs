namespace MangaBackend.LogsTools.Base
{
    public interface ITool
    {
        public void WriteToLog(ILogger logger, LogPosition logPosition, string result = "",string parameters = "");
        public string NameOfMethod { get; set; }
    }
}
