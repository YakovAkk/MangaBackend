using Newtonsoft.Json;

namespace Services.NotificationService.NotifyModels;

public class DataPayload
{
    [JsonProperty("title")]
    public string Title { get; set; }
    [JsonProperty("body")]
    public string Body { get; set; }
}
