using AutoMapper;
using Dawem.Contract.BusinessLogic.Core;
using Dawem.Contract.BusinessLogicCore;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Core.NotificationsStores;
using Dawem.Translations;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Core.NotificationsStores
{
    public class NotificationStoreBL : INotificationStoreBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;


        public NotificationStoreBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
         RequestInfo _requestHeaderContext)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            mapper = _mapper;

        }
        public async Task<GetNotificationStoreResponseDTO> Get(GetNotificationStoreCriteria criteria)
        {
            var NotificationStoreRepository = repositoryManager.NotificationStoreRepository;
            var query = NotificationStoreRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = NotificationStoreRepository.OrderBy(query, nameof(NotificationStore.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response


            var NotificationStoreList = await queryPaged.Select(notificatioStore => new NotificationStoreForGridDTO
            {
                Id = notificatioStore.Id,
               // FullMessege = notificatioStore.FullMessege,
                IconUrl = notificatioStore.IconUrl,
                Priority = notificatioStore.Priority,
                IsRead = notificatioStore.IsRead,
                EmployeeId = notificatioStore.EmployeeId,
                //ShortMessege = notificatioStore.ShortMessege,
                Status = notificatioStore.Status

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
        public async Task<GetNotificationStoreResponseDTO> GetNotificationsByUserId()
        {
            var employeeId = repositoryManager.UserRepository.Get(e => e.Id == requestInfo.UserId).FirstOrDefault().EmployeeId;
            var notifications = await repositoryManager.NotificationStoreRepository
        .Get(n => n.EmployeeId == employeeId)
        .Select(notification => new NotificationStoreForGridDTO
        {
            Id = notification.Id,
            //ShortMessege = notification.ShortMessege,
            //FullMessege = notification.FullMessege,
            IsRead = notification.IsRead,
            Status = notification.Status,
            IconUrl = notification.IconUrl,
            Priority = notification.Priority,
            EmployeeId = notification.EmployeeId

        }).ToListAsync();

            return new GetNotificationStoreResponseDTO()
            {
                NotificationStores = notifications,
                TotalCount = notifications.Count()
            };

        }
        public async Task<int> GetUnreadNotificationCount()
        {
            var employeeId = repositoryManager.UserRepository.Get(e => e.Id == requestInfo.UserId).FirstOrDefault().EmployeeId;
            var notification = await repositoryManager.NotificationStoreRepository.Get(n => !n.IsRead && !n.IsDeleted && n.EmployeeId == employeeId).ToListAsync();
            return notification.Count;
        }
    }
}
