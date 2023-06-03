using Newtonsoft.Json;
using NLog;

namespace FileService
{
    public class VersionFileConfig
    {
        private readonly ILogger _logger;
        private readonly string _path;
        public VersionViewModel Version { get; private set; }
        public VersionFileConfig(ILogger logger)
        {
            _logger = logger;
            _path = "./Version.json";
            Version = SetData();
        }

        private VersionViewModel SetData()
        {
            try
            {
                string text = File.ReadAllText(_path);
                var data = JsonConvert.DeserializeObject<VersionViewModel>(text);
                return data;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
        }
    }

    public class VersionViewModel
    {
        public int Version { get; set; }
        public DateTime Data { get; set; }

        public override string ToString()
        {
            return $"Version of the application is {Version} and the data of the last update is {Data}";
        }
    }
}