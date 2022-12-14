using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.NotificationService.NotifyModels;

public class GoogleNotification
{
    [JsonProperty("priority")]
    public string Priority { get; set; } = "high";
    [JsonProperty("data")]
    public DataPayload Data { get; set; }
    [JsonProperty("notification")]
    public DataPayload Notification { get; set; }
}
