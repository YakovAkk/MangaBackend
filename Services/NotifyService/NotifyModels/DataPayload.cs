using Newtonsoft.Json;

namespace Services.NotifyService.NotifyModels;

public class DataPayload
{
    [JsonProperty("title")]
    public string Title { get; set; }
    [JsonProperty("body")]
    public string Body { get; set; }
}
