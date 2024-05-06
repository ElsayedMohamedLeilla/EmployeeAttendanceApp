using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Core.Notifications;
using Dawem.RealTime.Helper;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Core.NotificationsStores
{
    public class NotificationBL : INotificationBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRepositoryManager repositoryManager;
        private readonly IUploadBLC uploadBLC;
        public NotificationBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager, IUploadBLC _uploadBLC,
         RequestInfo _requestHeaderContext)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            uploadBLC = _uploadBLC;
        }
        public async Task<GetNotificationsResponseDTO> Get(GetNotificationCriteria criteria)
        {
            criteria.EmployeeId = requestInfo.EmployeeId ?? 0;
            var NotificationStoreRepository = repositoryManager.NotificationRepository;
            var query = NotificationStoreRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = NotificationStoreRepository.OrderBy(query, nameof(Notification.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            var NotificationStoreList = await queryPaged.Select(notification => new NotificationForGridDTO
            {
                Id = notification.Id,
                FullMessege = NotificationHelper.GetNotificationDescription(notification.NotificationType, requestInfo.Lang),
                IconUrl = NotificationHelper.GetNotificationImage(notification.Status, uploadBLC),
                Priority = notification.Priority,
                IsRead = notification.IsRead,
                Date = notification.AddedDate,
                NotificationType = notification.NotificationType,
                ShortMessege = NotificationHelper.GetNotificationType(notification.NotificationType, requestInfo.Lang),
                Status = notification.Status,
                EmployeeId = notification.EmployeeId

            }).ToListAsync();

            return new GetNotificationsResponseDTO()
            {
                Notifications = NotificationStoreList.OrderBy(s => s.Date).ToList(),
                TotalCount = await queryOrdered.CountAsync()
            };

        }
        public async Task<bool> MarkAsViewed()
        {
            var employeeId = requestInfo.EmployeeId;

            var notifications = await repositoryManager.
                NotificationRepository.
                GetWithTracking(n => !n.IsViewed && !n.IsDeleted && n.EmployeeId == employeeId).
                ToListAsync();

            for (int i = 0; i < notifications.Count; i++)
            {
                notifications[i].MarkAsViewed();
            }
            await unitOfWork.SaveAsync();
            return true;

        }
        public async Task<bool> MarkAsRead(int notificationId)
        {
            var notification = await repositoryManager.NotificationRepository.
                GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && !d.IsRead && d.Id == notificationId) ??
                           throw new BusinessValidationException(AmgadKeys.SorryNotificationNotFound);
            notification.MarkAsRead();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<int> GetUnreadNotificationCount()
        {
            var employeeId = requestInfo.EmployeeId;
            var notification = await repositoryManager.NotificationRepository.Get(n => !n.IsRead && !n.IsDeleted && n.EmployeeId == employeeId).ToListAsync();
            return notification.Count;
        }
        public async Task<int> GetUnViewedNotificationCount()
        {
            var employeeId = requestInfo.EmployeeId;
            var notification = await repositoryManager.
                NotificationRepository.
                Get(n => !n.IsViewed && !n.IsDeleted && n.EmployeeId == employeeId).
                ToListAsync();

            return notification.Count;
        }
    }
}
