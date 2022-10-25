using Microsoft.AspNetCore.Mvc;
using Services.NotificationService.NotifyModels;
using Services.NotificationService.Service.Base;

namespace MangaBackend.Controllers;

[Route("api/notification")]
[ApiController]
public class NotificationController : ControllerBase
{
    public record notificationTempDTO
    {
        public string deviceToken { get; set; }
    }
    private readonly INotificationService _notificationService;
    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [Route("send")]
    [HttpPost]
    public async Task<IActionResult> SendNotification([FromBody] notificationTempDTO notificationTempDTO )
    {
        var notificationModel = new NotificationModel()
        {
            DeviceId = notificationTempDTO.deviceToken,
            IsAndroiodDevice = true,
            Title = "Test Notification from Manga api",
            Body = "Hi Sergey, sent me message in telegramm if you get the notification"
        };
        var result = await _notificationService.SendNotification(notificationModel);
        return Ok(result);
    }
}
    

