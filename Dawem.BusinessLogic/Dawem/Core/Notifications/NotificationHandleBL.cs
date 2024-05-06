using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.RealTime.Firebase;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Core.NotificationsStores
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
            #region Handle Insert In Notification

            foreach (var employeeId in model.EmployeeIds)
            {
                var getNotificationNextCode = await repositoryManager.NotificationRepository.
                    Get(e => e.CompanyId == requestInfo.CompanyId).
                    Select(e => e.Code).
                    DefaultIfEmpty().
                    MaxAsync() + 1;

                var notificationStore = new Notification()
                {
                    Code = getNotificationNextCode,
                    EmployeeId = employeeId,
                    CompanyId = requestInfo.CompanyId,
                    AddUserId = requestInfo.UserId,
                    Status = model.NotificationStatus,
                    NotificationType = model.NotificationType,
                    IsActive = true,
                    Priority = model.Priority
                };

                repositoryManager.NotificationRepository.Insert(notificationStore);
            }

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Send Notifications And Emails

            if (model.UserIds != null && model.UserIds.Count > 0)
            {
                await notificationService.SendNotificationsAndEmails(model.UserIds, model.NotificationType,
                    model.NotificationStatus);
            }

            #endregion

            return true;
        }
    }
}
