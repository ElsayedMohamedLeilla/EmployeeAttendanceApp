﻿using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.RealTime.Firebase;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.DTOs.Dawem.RealTime.Firebase;
using Dawem.RealTime.Helper;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Core.Notifications
{
    public class NotificationHandleBL : INotificationHandleBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRepositoryManager repositoryManager;
        private readonly INotificationService notificationService;
        public NotificationHandleBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         INotificationService _notificationServiceByFireBaseAdmin,
         RequestInfo _requestHeaderContext)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            notificationService = _notificationServiceByFireBaseAdmin;
        }
        public async Task<bool> HandleNotifications(HandleNotificationModel model)
        {
            if (model.NotificationUsers == null || model.NotificationUsers.Count == 0)
                return true;

            #region Handle Short And Full Message

            var getActiveLanguages = model.ActiveLanguages.Any() ? model.ActiveLanguages :
                await repositoryManager.LanguageRepository.Get(l => !l.IsDeleted && l.IsActive).
                Select(l => new ActiveLanguageModel
                {
                    Id = l.Id,
                    ISO2 = l.ISO2
                }).ToListAsync();

            var notificationTranslations = new List<NotificationTranslationModel>();

            foreach (var language in getActiveLanguages)
            {
                notificationTranslations.Add(new NotificationTranslationModel
                {
                    LanguageId = language.Id,
                    Title = NotificationHelper.GetNotificationType(model.NotificationType, language.ISO2),
                    Body = model.NotificationDescriptions.Any() ?
                    model.NotificationDescriptions.FirstOrDefault(d => d.LanguageIso2 == language.ISO2).Description :
                    NotificationHelper.GetNotificationDescription(model.NotificationType, language.ISO2)
                });
            }

            #endregion

            #region Handle Insert In Notification

            repositoryManager.NotificationRepository.Insert(new Notification()
            {
                NotificationType = model.NotificationType,
                ScreenCode = NotificationHelper.GetNotificationScreenCode(model.NotificationType),
                NotificationTypeName = model.NotificationType.ToString(),
                CompanyId = requestInfo.CompanyId == 0 ? model.CompanyId : requestInfo.CompanyId,
                AddUserId = requestInfo.UserId,
                Status = model.NotificationStatus,
                IsActive = true,
                Priority = model.Priority,
                HelperNumber = model.HelperNumber,
                HelperDate = model.HelperDate,
                NotificationTranslations = notificationTranslations.
                    Select(nt => new NotificationTranslation
                    {
                        LanguageId = nt.LanguageId,
                        Title = nt.Title,
                        Body = nt.Body
                    }).ToList(),
                NotificationEmployees = model.EmployeeIds.
                    Select(employeeId => new NotificationEmployee
                    {
                        EmployeeId = employeeId
                    }).ToList()
            });

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Send Notifications And Emails

            var shortMessage = NotificationHelper.GetNotificationType(model.NotificationType, requestInfo.Lang);
            var fullMessage = model.NotificationDescriptions.Any() ?
            model.NotificationDescriptions.FirstOrDefault(d => d.LanguageIso2 == requestInfo.Lang).Description :
            NotificationHelper.GetNotificationDescription(model.NotificationType, requestInfo.Lang);

            if (model.NotificationUsers != null && model.NotificationUsers.Count > 0)
            {
                var sendNotificationsAndEmailsModel = new SendNotificationsAndEmailsModel
                {
                    Title = shortMessage,
                    Body = fullMessage,
                    NotificationUsers = model.NotificationUsers,
                    NotificationType = model.NotificationType,
                    NotificationStatus = model.NotificationStatus
                };
                await notificationService.SendNotificationsAndEmails(sendNotificationsAndEmailsModel);
            }

            #endregion

            return true;
        }
    }
}
