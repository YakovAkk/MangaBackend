﻿using Newtonsoft.Json;

namespace Services.NotificationService.NotifyModels;

public class ResponseModel
{
    [JsonProperty("isSuccess")]
    public bool IsSuccess { get; set; }
    [JsonProperty("message")]
    public string Message { get; set; }
}
