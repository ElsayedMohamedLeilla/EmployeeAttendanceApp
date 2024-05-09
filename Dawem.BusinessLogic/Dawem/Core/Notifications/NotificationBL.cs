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
            var notificationStoreRepository = repositoryManager.NotificationRepository;
            var query = notificationStoreRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = notificationStoreRepository.OrderBy(query, nameof(Notification.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            var notificationsList = await queryPaged.Select(notification => new NotificationForGridDTO
            {
                Id = notification.Id,
                Title = notification.NotificationTranslations.
                FirstOrDefault(t=>t.Language.ISO2 == requestInfo.Lang).Title,
                Body = notification.NotificationTranslations.
                FirstOrDefault(t => t.Language.ISO2 == requestInfo.Lang).Body,
                IconUrl = NotificationHelper.GetNotificationImage(notification.Status, uploadBLC),
                Priority = notification.Priority,
                IsRead = notification.IsRead,
                Date = notification.AddedDate.AddHours((double?)notification.Company.Country.TimeZoneToUTC ?? 0),
                NotificationType = notification.NotificationType,
                Status = notification.Status
            }).ToListAsync();

            return new GetNotificationsResponseDTO()
            {
                Notifications = notificationsList,
                TotalCount = await queryOrdered.CountAsync()
            };

        }
        public async Task<bool> MarkAsViewed()
        {
            var employeeId = requestInfo.EmployeeId;

            var notifications = await repositoryManager.
                NotificationRepository.
                GetWithTracking(n => !n.IsViewed && !n.IsDeleted && n.NotificationEmployees.Any( ne => ne.EmployeeId == employeeId)).
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
            var notification = await repositoryManager.NotificationRepository.Get(n => !n.IsRead && !n.IsDeleted && n.NotificationEmployees.Any(ne => ne.EmployeeId == employeeId)).ToListAsync();
            return notification.Count;
        }
        public async Task<int> GetUnViewedNotificationCount()
        {
            var employeeId = requestInfo.EmployeeId;
            var notification = await repositoryManager.
                NotificationRepository.
                Get(n => !n.IsViewed && !n.IsDeleted && n.NotificationEmployees.Any(ne => ne.EmployeeId == employeeId)).
                ToListAsync();

            return notification.Count;
        }
    }
}
