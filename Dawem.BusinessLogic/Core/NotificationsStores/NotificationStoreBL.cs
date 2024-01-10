using AutoMapper;
using Dawem.BusinessLogic.SignalR;
using Dawem.Contract.BusinessLogic.Core;
using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Shared;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Core.NotificationsStores;
using Dawem.Translations;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Core.NotificationsStores
{
    public class NotificationStoreBL : INotificationStoreBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IMailBL mailBL;
        private readonly IHubContext<SignalRHub, ISignalRHubClient> hubContext;



        public NotificationStoreBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
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
                FullMessege = SignalRHelper.GetNotificationDescription(notificatioStore.NotificationType, requestInfo.Lang),
                IconUrl = notificatioStore.ImageUrl,
                Priority = notificatioStore.Priority,
                IsRead = notificatioStore.IsRead,
                EmployeeId = notificatioStore.EmployeeId,
                ShortMessege = SignalRHelper.GetNotificationType(notificatioStore.NotificationType, requestInfo.Lang),
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
        public async Task<GetNotificationStoreResponseDTO> GetNotificationsByUserId(GetNotificationStoreCriteria criteria)
        {
            var employeeId = repositoryManager.UserRepository.Get(e => e.Id == requestInfo.UserId)
                .FirstOrDefault().EmployeeId;
            criteria.EmployeeID = employeeId;
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


            var NotificationStoreList = await queryPaged.Select(notificatioStore => new NotificationStoreForGridDTO
            {
                Id = notificatioStore.Id,
                FullMessege = SignalRHelper.GetNotificationDescription(notificatioStore.NotificationType, requestInfo.Lang),
                IconUrl = notificatioStore.ImageUrl,
                Priority = notificatioStore.Priority,
                IsRead = notificatioStore.IsRead,
                EmployeeId = notificatioStore.EmployeeId,
                ShortMessege = SignalRHelper.GetNotificationType(notificatioStore.NotificationType, requestInfo.Lang),
                Status = notificatioStore.Status

            }).ToListAsync();

            return new GetNotificationStoreResponseDTO()
            {
                NotificationStores = NotificationStoreList,
                TotalCount = await queryOrdered.CountAsync()
            };

        }
        public async Task<int> GetUnreadNotificationCountByUserId()
        {
            var employeeId = repositoryManager.UserRepository.Get(e => e.Id == requestInfo.UserId).FirstOrDefault().EmployeeId ?? 0;
            var notification = await repositoryManager.NotificationStoreRepository.Get(n => !n.IsRead && !n.IsDeleted && n.EmployeeId == employeeId).ToListAsync();
            return notification.Count;
        }
        public async Task<GetNotificationStoreResponseDTO> GetUnreadNotificationByUserId(GetNotificationStoreCriteria criteria)
        {
            var employeeId = repositoryManager.UserRepository.Get(e => e.Id == requestInfo.UserId)
             .FirstOrDefault().EmployeeId;
            criteria.EmployeeID = employeeId;
            criteria.IsRead = false;

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


            var NotificationStoreList = await queryPaged.Select(notificatioStore => new NotificationStoreForGridDTO
            {
                Id = notificatioStore.Id,
                FullMessege = SignalRHelper.GetNotificationDescription(notificatioStore.NotificationType, requestInfo.Lang),
                IconUrl = notificatioStore.ImageUrl,
                Priority = notificatioStore.Priority,
                IsRead = notificatioStore.IsRead,
                EmployeeId = notificatioStore.EmployeeId,
                ShortMessege = SignalRHelper.GetNotificationType(notificatioStore.NotificationType, requestInfo.Lang),
                Status = notificatioStore.Status

            }).ToListAsync();

            return new GetNotificationStoreResponseDTO()
            {
                NotificationStores = NotificationStoreList,
                TotalCount = await queryOrdered.CountAsync()
            };

        }

        public async Task<bool> SendNotificationAndEmail(NotificationType type, int groupEmployeeId, string EmployeeName, string employeeEmail)
        {
            #region Notification
            await hubContext.Clients.Group(AmgadKeys.EmployeeGroup + LeillaKeys.UnderScore + groupEmployeeId)
               .ReceiveNewNotification(SignalRHelper.TempNotificationModelDTO(await GetUnreadNotificationCountByUserId(), requestInfo.Lang, NotificationType.NewVacationRequest, EmployeeName));
            #endregion
            #region Email
            var verifyEmail = new VerifyEmailModel
            {
                Email = employeeEmail,
                Subject = SignalRHelper.GetNotificationType(type, requestInfo.Lang),
                Body = TranslationHelper.GetTranslation(AmgadKeys.Dear, requestInfo.Lang)  + "<h3>  " + EmployeeName + " </h3> " + SignalRHelper.GetNotificationDescription(type, requestInfo.Lang),

            };

            await mailBL.SendEmail(verifyEmail);
            #endregion
            return true;
        }
    }
}
