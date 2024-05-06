using AutoMapper;
using Dawem.BusinessLogic.Dawem.RealTime.SignalR;
using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessLogic.Dawem.Provider;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Core.NotificationsStores;
using Dawem.RealTime.Helper;
using Dawem.Translations;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Core.NotificationsStores
{
    public class NotificationBL : INotificationBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IMailBL mailBL;
        private readonly IUploadBLC uploadBLC;
        private readonly IHubContext<SignalRHub, ISignalRHubClient> hubContext;
        public NotificationBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper, IUploadBLC _uploadBLC,
         RequestInfo _requestHeaderContext, IMailBL _mailBL,
         IHubContext<SignalRHub, ISignalRHubClient> _hubContext
         )
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            mapper = _mapper;
            mailBL = _mailBL;
            hubContext = _hubContext;
            uploadBLC = _uploadBLC;
        }
        public async Task<GetNotificationStoreResponseDTO> Get(GetNotificationStoreCriteria criteria)
        {
            var NotificationStoreRepository = repositoryManager.NotificationStoreRepository;
            var query = NotificationStoreRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = NotificationStoreRepository.OrderBy(query, nameof(NotificationStore.AddedDate), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var NotificationStoreList = await queryPaged.Select(notification => new NotificationStoreForGridDTO
            {
                Id = notification.Id,
                FullMessege = NotificationHelper.GetNotificationDescription(notification.NotificationType, requestInfo.Lang),
                IconUrl = NotificationHelper.GetNotificationImage(notification.Status, uploadBLC),
                Priority = notification.Priority,
                IsRead = notification.IsRead,
                EmployeeId = notification.EmployeeId,
                Date = notification.AddedDate,
                ShortMessege = NotificationHelper.GetNotificationType(notification.NotificationType, requestInfo.Lang),
                Status = notification.Status

            }).ToListAsync();

            return new GetNotificationStoreResponseDTO
            {
                NotificationStores = NotificationStoreList,
                TotalCount = await query.CountAsync()
            };

            #endregion
        }
        public async Task<bool> Delete(int NotificationStoreId)
        {
            var notification = await repositoryManager.NotificationStoreRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == NotificationStoreId) ??
                throw new BusinessValidationException(AmgadKeys.SorryNotificationNotFound);

            notification.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Enable(int NotificationStoreId)
        {
            var notification = await repositoryManager.NotificationStoreRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == NotificationStoreId) ??
                throw new BusinessValidationException(AmgadKeys.SorryNotificationNotFound);
            notification.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var notification = await repositoryManager.NotificationStoreRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(AmgadKeys.SorryNotificationNotFound);
            notification.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> MarkAsRead(int notificationStoreId)
        {
            var notification = await repositoryManager.NotificationStoreRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && !d.IsRead && d.Id == notificationStoreId) ??
                           throw new BusinessValidationException(AmgadKeys.SorryNotificationNotFound);
            notification.MarkAsRead();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetNotificationStoreResponseDTO> GetNotifications(GetNotificationStoreCriteria criteria)
        {
            criteria.EmployeeId = requestInfo.EmployeeId ?? 0;
            var NotificationStoreRepository = repositoryManager.NotificationStoreRepository;
            var query = NotificationStoreRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = NotificationStoreRepository.OrderBy(query, nameof(NotificationStore.AddedDate), LeillaKeys.Asc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            var NotificationStoreList = await queryPaged.Select(notification => new NotificationStoreForGridDTO
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

            return new GetNotificationStoreResponseDTO()
            {
                NotificationStores = NotificationStoreList.OrderBy(s => s.Date).ToList(),
                TotalCount = await queryOrdered.CountAsync()
            };

        }
        public async Task<int> GetUnreadNotificationCount()
        {
            var employeeId = repositoryManager.UserRepository.Get(e => e.Id == requestInfo.UserId).FirstOrDefault().EmployeeId;
            var notification = await repositoryManager.NotificationStoreRepository.Get(n => !n.IsRead && !n.IsDeleted && n.EmployeeId == employeeId).ToListAsync();
            return notification.Count;
        }
        public async Task<GetNotificationStoreResponseDTO> GetUnreadNotification(GetNotificationStoreCriteria criteria)
        {
            var employeeId = repositoryManager.UserRepository.Get(e => e.Id == requestInfo.UserId)
             .FirstOrDefault().EmployeeId;
            criteria.EmployeeId = employeeId;
            criteria.IsRead = false;

            var NotificationStoreRepository = repositoryManager.NotificationStoreRepository;
            var query = NotificationStoreRepository.GetAsQueryable(criteria);


            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = NotificationStoreRepository.OrderBy(query, nameof(NotificationStore.AddedDate), LeillaKeys.Asc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion


            var NotificationStoreList = await queryPaged.Select(notification => new NotificationStoreForGridDTO
            {
                Id = notification.Id,
                FullMessege = NotificationHelper.GetNotificationDescription(notification.NotificationType, requestInfo.Lang),
                IconUrl = NotificationHelper.GetNotificationImage(notification.Status, uploadBLC),
                Priority = notification.Priority,
                IsRead = notification.IsRead,
                Date = notification.AddedDate,
                EmployeeId = notification.EmployeeId,
                ShortMessege = NotificationHelper.GetNotificationType(notification.NotificationType, requestInfo.Lang),
                Status = notification.Status

            }).ToListAsync();

            return new GetNotificationStoreResponseDTO()
            {
                NotificationStores = NotificationStoreList,
                TotalCount = await queryOrdered.CountAsync()
            };

        }
        public async Task<int> GetUnViewedNotificationCount()
        {
            var employeeId = repositoryManager.UserRepository.Get(e => e.Id == requestInfo.UserId).FirstOrDefault().EmployeeId;
            var notification = await repositoryManager.NotificationStoreRepository.Get(n => !n.IsViewed && !n.IsDeleted && n.EmployeeId == employeeId).ToListAsync();
            return notification.Count;
        }
        public async Task<bool> MarkAsViewed()
        {
            var employeeId = repositoryManager.UserRepository.Get(e => e.Id == requestInfo.UserId).FirstOrDefault().EmployeeId;
            var notifications = await repositoryManager.NotificationStoreRepository.GetWithTracking(n => !n.IsViewed && !n.IsDeleted && n.EmployeeId == employeeId).ToListAsync();
            for (int i = 0; i < notifications.Count; i++)
            {
                notifications[i].MarkAsViewed();
            }
            await unitOfWork.SaveAsync();
            return true;

        }
        public async Task<GetNotificationStoreResponseDTO> GetUnreadNotifications(GetNotificationStoreCriteria criteria)
        {
            var employeeId = repositoryManager.UserRepository.Get(e => e.Id == requestInfo.UserId)
                .FirstOrDefault().EmployeeId;
            criteria.EmployeeId = employeeId;
            var NotificationStoreRepository = repositoryManager.NotificationStoreRepository;
            var query = NotificationStoreRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = NotificationStoreRepository.OrderBy(query, nameof(NotificationStore.AddedDate), LeillaKeys.Asc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            var NotificationStoreList = await queryPaged.Select(notification => new NotificationStoreForGridDTO
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

            return new GetNotificationStoreResponseDTO()
            {
                NotificationStores = NotificationStoreList.OrderBy(s => s.Date).ToList(),
                TotalCount = await queryOrdered.CountAsync()
            };
        }
    }
}
