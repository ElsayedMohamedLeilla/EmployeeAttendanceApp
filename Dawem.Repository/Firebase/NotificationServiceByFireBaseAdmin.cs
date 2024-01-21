using Dawem.Contract.Firebase;
using Dawem.Enums.Generals;
using Dawem.Models.Firebase;
using FirebaseAdmin.Messaging;

public class NotificationServiceByFireBaseAdmin : INotificationServiceByFireBaseAdmin
{
    public async Task<ResponseModel> SendNotification(NotificationModel notificationModel)
    {
        ResponseModel response = new ResponseModel();
        if (notificationModel.DeviceType == ApplicationType.Web)
        {
            response = await SendWebNotification(notificationModel);
        }
        else if (notificationModel.DeviceType == ApplicationType.Android)
        {
            response = await SendAndroidNotification(notificationModel);
        }
        else if (notificationModel.DeviceType == ApplicationType.Ios)
        {
            response = await SendIOSNotification(notificationModel);
        }
        else
        {
            response.IsSuccess = false;
            response.Message = "Unsupported device type";
        }


        return response;
    }
    private static async Task<ResponseModel> SendWebNotification(NotificationModel notificationModel)
    {
        ResponseModel response = new ResponseModel();
        try
        {
            // Create a message
            var message = new MulticastMessage
            {
                Notification = new Notification
                {
                    Title = notificationModel.Title,
                    Body = notificationModel.Body,
                },
                Tokens = GetTokenClassificationByDeviceType(ApplicationType.Web, notificationModel.Tokens), // List of FCM tokens
                Data = notificationModel.Data
            };
            // Send the message
            var responseMessage = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            // Check if the message was sent successfully
            if (responseMessage != null && responseMessage.SuccessCount > 0)
            {
                response.IsSuccess = true;
                response.Message = $"Web Notification sent successfully to {responseMessage.SuccessCount} devices.";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Failed to send Web Notification";
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = "Error sending Web Notification: " + ex.Message;
        }

        return response;
    }
    private static async Task<ResponseModel> SendAndroidNotification(NotificationModel notificationModel)
    {
        ResponseModel response = new ResponseModel();
        try
        {
            var message = new MulticastMessage
            {
                Notification = new Notification
                {
                    Title = notificationModel.Title,
                    Body = notificationModel.Body,
                },
                Tokens = GetTokenClassificationByDeviceType(ApplicationType.Android, notificationModel.Tokens),
                Android = new AndroidConfig
                {
                    Priority = FirebaseAdmin.Messaging.Priority.High,
                    Notification = new AndroidNotification
                    {
                        Icon = "your_notification_icon",
                        Color = "#RRGGBB",
                    },
                },
                Data = notificationModel.Data
            };

            // Send the message
            var responseMessage = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            // Check if the message was sent successfully
            if (responseMessage != null && responseMessage.SuccessCount > 0)
            {
                response.IsSuccess = true;
                response.Message = $"Android Notification sent successfully to {responseMessage.SuccessCount} devices.";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Failed to send Android Notification";
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = "Error sending Android Notification: " + ex.Message;
        }

        return response;
    }
    private async Task<ResponseModel> SendIOSNotification(NotificationModel notificationModel)
    {
        ResponseModel response = new ResponseModel();
        try
        {
            // Create a message
            var message = new MulticastMessage
            {
                Notification = new Notification
                {
                    Title = notificationModel.Title,
                    Body = notificationModel.Body,
                },
                Tokens = GetTokenClassificationByDeviceType(ApplicationType.Ios, notificationModel.Tokens),
                Apns = new ApnsConfig
                {
                    Headers = new Dictionary<string, string>
                {
                    { "apns-priority", "10" },
                },
                    Aps = new Aps
                    {
                        Badge = 1,
                        Sound = "default",
                    },
                },
                Data = notificationModel.Data
            };

            // Send the message
            var responseMessage = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);

            if (responseMessage != null && responseMessage.SuccessCount > 0)
            {
                response.IsSuccess = true;
                response.Message = $"iOS Notification sent successfully to {responseMessage.SuccessCount} devices.";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Failed to send iOS Notification";
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = "Error sending iOS Notification: " + ex.Message;
        }
        return response;
    }
    private static List<string> GetTokenClassificationByDeviceType(ApplicationType deviceType, List<TokensModel> tokens)
    {
        switch (deviceType)
        {
            case ApplicationType.Web:
                return tokens
                    .Where(t => t.ApplicationType == ApplicationType.Web)
                    .Select(t => t.Token)
                    .ToList();
            case ApplicationType.Android:
                return tokens
                    .Where(t => t.ApplicationType == ApplicationType.Android)
                    .Select(t => t.Token)
                    .ToList();
            case ApplicationType.Ios:
                return tokens
                    .Where(t => t.ApplicationType == ApplicationType.Ios)
                    .Select(t => t.Token)
                    .ToList();
            default:
                // Handle unknown device type or return an empty list
                return new List<string>();
        }
    }
}
