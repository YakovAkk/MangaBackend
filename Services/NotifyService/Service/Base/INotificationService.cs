
using Services.NotifyService.NotifyModels;

namespace Services.NotifyService.Service.Base;

public interface INotificationService
{
    Task<ResponseModel> SendNotification(NotificationModel notificationModel);
}
