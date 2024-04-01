using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessLogic.Dawem.Provider;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.RealTime.Firebase;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Core.NotificationsStores;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.RealTime.Firebase;
using Dawem.RealTime.Helper;
using Dawem.Translations;
using FirebaseAdmin.Messaging;
using System.Text.Json;

public class NotificationServiceByFireBaseAdmin : INotificationServiceByFireBaseAdmin
{
    private readonly RequestInfo requestInfo;
    private readonly IUploadBLC uploadBLC;
    private readonly INotificationStoreBL notificationStoreBL;
    private readonly IRepositoryManager repositoryManager;
    private readonly IMailBL mailBL;
    public NotificationServiceByFireBaseAdmin(RequestInfo _requestInfo, IUploadBLC _uploadBLC,
        INotificationStoreBL _notificationStoreBL, IRepositoryManager _repositoryManager,
        IMailBL _mailBL)
    {
        requestInfo = _requestInfo;
        uploadBLC = _uploadBLC;
        notificationStoreBL = _notificationStoreBL;
        repositoryManager = _repositoryManager;
        mailBL = _mailBL;
    }
    public async Task<ResponseModel> Send_Notification_Email(List<int> UserIds, NotificationType notificationType, NotificationStatus type)
    {
        ResponseModel response = new();
        string Title = NotificationHelper.GetNotificationType(notificationType, requestInfo.Lang);
        string Body = NotificationHelper.GetNotificationDescription(notificationType, requestInfo.Lang);
        Dictionary<string, string> Data = await GetNotificationData(notificationType);
        string ImageUrl = NotificationHelper.GetNotificationImage(type, uploadBLC);
        List<TokensModel> UserToken = GetUserTokens(UserIds);
        var (webTokens, androidTokens, iosTokens) = GetTokenClassificationByDeviceType(UserToken);
        #region Send Notification
        if (webTokens.Count > 0)
        {
            NotificationModel webModel = new NotificationModel()
            {
                Body = Body,
                Title = Title,
                Data = Data,
                ImageUrl = ImageUrl,
                Tokens = webTokens
            };
            response = await Send_Web_Notification(webModel);
        }
        if (androidTokens.Count > 0)
        {
            NotificationModel androiodModel = new NotificationModel()
            {
                Body = Body,
                Title = Title,
                Data = Data,
                ImageUrl = ImageUrl,
                Tokens = androidTokens
            };
            response = await Send_Android_Notification(androiodModel);
        }
        if (iosTokens.Count > 0)
        {
            NotificationModel iosModel = new NotificationModel()
            {
                Body = Body,
                Title = Title,
                Data = Data,
                ImageUrl = ImageUrl,
                Tokens = iosTokens
            };
            response = await Send_Ios_Notification(iosModel);
        }
        #endregion
        #region Send Email
        await SendEmailByUserIds(UserIds, notificationType);
        #endregion
        return response;
    }
    private static async Task<ResponseModel> Send_Web_Notification(NotificationModel notificationModel)
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
                    ImageUrl = notificationModel.ImageUrl

                },

                Tokens = notificationModel.Tokens, // List of FCM tokens
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
    private static async Task<ResponseModel> Send_Android_Notification(NotificationModel notificationModel)
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
                    ImageUrl = notificationModel.ImageUrl

                },
                Tokens = notificationModel.Tokens,
                Android = new AndroidConfig
                {
                    Priority = FirebaseAdmin.Messaging.Priority.High,
                    Notification = new AndroidNotification
                    {
                        Icon = "your_notification_icon",
                        Color = "#1827b5",
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
    private async Task<ResponseModel> Send_Ios_Notification(NotificationModel notificationModel)
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
                    ImageUrl = notificationModel.ImageUrl

                },
                Tokens = notificationModel.Tokens,
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
    private static (List<string>, List<string>, List<string>) GetTokenClassificationByDeviceType(List<TokensModel> tokens)
    {
        return (tokens
            .Where(t => t.ApplicationType == ApplicationType.Web)
            .Select(t => t.Token)
            .ToList(), tokens
            .Where(t => t.ApplicationType == ApplicationType.Android)
            .Select(t => t.Token)
            .ToList(), tokens
            .Where(t => t.ApplicationType == ApplicationType.Ios)
            .Select(t => t.Token)
            .ToList());
    }
    private async Task<Dictionary<string, string>> GetNotificationData(NotificationType notificationType)
    {
        NotificationModelDTO nPM = new NotificationModelDTO()
        {
            NotificationType = notificationType,
            UnReadNotificationCount = await notificationStoreBL.GetUnreadNotificationCount(),
        };
        string jsonString = JsonSerializer.Serialize(nPM);


        Dictionary<string, string> myDict = new Dictionary<string, string>
        {
            {"NotificationData", jsonString},
        };
        return myDict;
    }
    private List<TokensModel> GetUserTokens(List<int> userids)
    {
        List<TokensModel> userTokens = repositoryManager.NotificationUserFCMTokenRepository
            .Get(s => !s.IsDeleted && userids.Contains(s.NotificationUser.UserId)).Select(c => new TokensModel()
            {
                ApplicationType = c.DeviceType,
                Token = c.FCMToken
            }).ToList();

        return userTokens;
    }
    public async Task<bool> SendEmailByUserIds(List<int> userIds, NotificationType notificationType)
    {
        List<string> emails = GetUserEmails(userIds);
        bool result = false;

        var verifyEmail = new VerifyEmailModel
        {
            Subject = NotificationHelper.GetNotificationType(notificationType, requestInfo.Lang),
            Body = @"<meta charset='UTF-8'>
                                            <title>عزيزي الموظف</title>
                                            <style>
                                            body { direction: rtl; }
                                            </style>
                                            </head>
                                            <body>
                                            <h1>" + NotificationHelper.GetNotificationDescription(notificationType, requestInfo.Lang) + @"</h1>
                                            </body>
                                            </html>",
            Emails = emails.Where(e => e != AmgadKeys.NoEmail).Distinct().ToList()
        };

        result = await mailBL.SendEmail(verifyEmail);
        return result;
    }
    public List<string> GetUserEmails(List<int> userIds)
    {
        List<string> userEmails = repositoryManager.UserRepository.Get(s => !s.IsDeleted & s.IsActive & userIds.Contains(s.Id)).Select(c => c.Email ?? AmgadKeys.NoEmail).ToList();
        return userEmails;
    }



}
