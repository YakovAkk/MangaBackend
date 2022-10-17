using Microsoft.AspNetCore.Mvc;
using Services.NotificationService.NotifyModels;
using Services.NotificationService.Service.Base;

namespace MangaBackend.Controllers;

[Route("api/notification")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;
    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [Route("send")]
    [HttpPost]
    public async Task<IActionResult> SendNotification(NotificationModel notificationModel)
    {
        var result = await _notificationService.SendNotification(notificationModel);
        return Ok(result);
    }
}
    

