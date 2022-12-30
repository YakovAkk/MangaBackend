using Newtonsoft.Json;

namespace Services.NotificationService.NotifyModels;

public class NotificationModel
{
    [JsonProperty("deviceId")]
    public string DeviceId { get; set; }
    [JsonProperty("isAndroiodDevice")]
    public bool IsAndroiodDevice { get; set; }
    [JsonProperty("title")]
    public string Title { get; set; }
    [JsonProperty("body")]
    public string Body { get; set; }

    public override string ToString()
    {
        return $"DeviceId = {DeviceId} IsAndroiodDevice = {IsAndroiodDevice} Title = {Title} Body = {Body}";
    }
}
