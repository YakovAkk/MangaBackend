using CorePush.Google;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repositories.LogsTools;
using Repositories.LogsTools.Base;
using Services.NotificationService.NotifyModels;
using Services.NotificationService.Service.Base;
using Services.NotificationService.Setting;
using System.Net.Http.Headers;

namespace Services.NotificationService.Service;

public class NotificationService : INotificationService
{
    private readonly FcmNotificationSetting _fcmNotificationSetting;

    public NotificationService(IConfiguration configuration)
    {
        var SenderId = configuration.GetSection("FcmNotification")["SenderId"];
        var ServerKey = configuration.GetSection("FcmNotification")["ServerKey"];
        _fcmNotificationSetting = new FcmNotificationSetting()
        {
            SenderId = SenderId,
            ServerKey = ServerKey
        };
    }

    public async Task<ResponseModel> SendNotification(NotificationModel notificationModel)
    {
        ResponseModel response = new ResponseModel();
        try
        {
            if (notificationModel.IsAndroiodDevice)
            {
                /* FCM Sender (Android Device) */
                FcmSettings settings = new FcmSettings()
                {
                    SenderId = _fcmNotificationSetting.SenderId,
                    ServerKey = _fcmNotificationSetting.ServerKey
                };
                HttpClient httpClient = new HttpClient();

                string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
                string deviceToken = notificationModel.DeviceId;

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                httpClient.DefaultRequestHeaders.Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                DataPayload dataPayload = new DataPayload();
                dataPayload.Title = notificationModel.Title;
                dataPayload.Body = notificationModel.Body;

                GoogleNotification notification = new GoogleNotification();
                notification.Data = dataPayload;
                notification.Notification = dataPayload;

                var fcm = new FcmSender(settings, httpClient);
                var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);

                if (fcmSendResponse.IsSuccess())
                {
                    response.IsSuccess = true;
                    response.Message = "Notification sent successfully";
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    foreach (var result in fcmSendResponse.Results)
                    {
                        response.Message += result.Error;
                    }
                    return response;
                }
            }
            else
            {
                /* Code here for APN Sender (iOS Device) */
                //var apn = new ApnSender(apnSettings, httpClient);
                //await apn.SendAsync(notification, deviceToken);
            }
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = "Something went wrong";
            return response;
        }
    }
}

