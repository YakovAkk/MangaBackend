using Newtonsoft.Json;

namespace Services.NotifyService.NotifyModels;

public class ResponseModel
{
    [JsonProperty("isSuccess")]
    public bool IsSuccess { get; set; }
    [JsonProperty("message")]
    public string Message { get; set; }
}
