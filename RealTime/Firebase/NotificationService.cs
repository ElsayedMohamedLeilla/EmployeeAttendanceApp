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
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public class NotificationService : INotificationService
{
    private readonly RequestInfo requestInfo;
    private readonly IUploadBLC uploadBLC;
    private readonly INotificationBL notificationBL;
    private readonly IRepositoryManager repositoryManager;
    private readonly IMailBL mailBL;
    public NotificationService(RequestInfo _requestInfo, IUploadBLC _uploadBLC,
        INotificationBL _notificationStoreBL, IRepositoryManager _repositoryManager,
        IMailBL _mailBL)
    {
        requestInfo = _requestInfo;
        uploadBLC = _uploadBLC;
        notificationBL = _notificationStoreBL;
        repositoryManager = _repositoryManager;
        mailBL = _mailBL;
    }
    public async Task<ResponseModel> SendNotificationsAndEmails(SendNotificationsAndEmailsModel model)
    {
        var notificationType = model.NotificationType;
        var notificationStatus = model.NotificationStatus;
        var userIds = model.UserIds;

        ResponseModel response = new();

        var notificationData = await GetNotificationData(notificationType);
        var imageUrl = NotificationHelper.GetNotificationImage(notificationStatus, uploadBLC);
        var userToken = GetUserTokens(userIds);
        var (webTokens, androidTokens, iosTokens) = GetTokenClassificationByDeviceType(userToken);

        #region Send Notification

        NotificationModel notificationModel = new()
        {
            Title = model.Title,
            Body = model.Body,
            Data = notificationData,
            ImageUrl = imageUrl
        };

        if (webTokens.Count > 0)
        {
            notificationModel.Tokens = webTokens;
            response = await SendWebNotification(notificationModel);
        }
        if (androidTokens.Count > 0)
        {
            notificationModel.Tokens = androidTokens;
            response = await SendAndroidNotification(notificationModel);
        }
        if (iosTokens.Count > 0)
        {
            notificationModel.Tokens = iosTokens;
            response = await SendIosNotification(notificationModel);
        }

        #endregion

        #region Send Email

        try
        {
            await SendEmailByUserIds(model);
        }
        catch (Exception ec)
        {
        }

        #endregion

        return response;
    }
    private static async Task<ResponseModel> SendWebNotification(NotificationModel notificationModel)
    {
        var response = new ResponseModel();
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
    private async Task<ResponseModel> SendIosNotification(NotificationModel notificationModel)
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
        NotificationDataModel model = new()
        {
            NotificationType = notificationType,
            UnReadNotificationCount = await notificationBL.GetUnreadNotificationCount()
        };
        string jsonString = JsonSerializer.Serialize(model);

        var dataDictionary = new Dictionary<string, string>
        {
            {"NotificationData", jsonString},
        };
        return dataDictionary;
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
    public async Task<bool> SendEmailByUserIds(SendNotificationsAndEmailsModel model)
    {
        var emails = await GetUsersEmails(model.UserIds);
        bool result = false;

        var verifyEmail = new VerifyEmailModel
        {
            Subject = model.Title,
            Body = @"<meta charset='UTF-8'>
                                            <title>عزيزي الموظف</title>
                                            <style>
                                            body { direction: rtl; }
                                            </style>
                                            </head>
                                            <body>
                                            <h1>" + model.Body + @"</h1>
                                            </body>
                                            </html>",
            Emails = emails.Where(e => e != AmgadKeys.NoEmail).Distinct().ToList()
        };

        result = await mailBL.SendEmail(verifyEmail);
        return result;
    }
    public async Task<List<string>> GetUsersEmails(List<int> userIds)
    {
        var userEmails = await repositoryManager.UserRepository.
            Get(s => !s.IsDeleted & s.IsActive & userIds.Contains(s.Id) && !string.IsNullOrEmpty(s.Email)).
            Select(c => c.Email).
            ToListAsync();

        return userEmails;
    }



}
