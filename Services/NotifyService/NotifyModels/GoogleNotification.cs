using Newtonsoft.Json;

namespace Services.NotifyService.NotifyModels;

public class GoogleNotification
{
    [JsonProperty("priority")]
    public string Priority { get; set; } = "high";
    [JsonProperty("data")]
    public DataPayload Data { get; set; }
    [JsonProperty("notification")]
    public DataPayload Notification { get; set; }
}
