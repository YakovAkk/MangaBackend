using Services.NotificationService.NotifyModels;

namespace Services.NotificationService.Service.Base;

public interface INotificationService
{
    Task<ResponseModel> SendNotification(NotificationModel notificationModel);
}
