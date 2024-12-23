using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessLogic.Dawem.Requests;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.BusinessValidation.Dawem.Requests;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Others;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.DTOs.Dawem.RealTime.Firebase;
using Dawem.Models.Requests;
using Dawem.Models.Requests.Justifications;
using Dawem.Models.Response.Dawem.Requests;
using Dawem.Models.Response.Dawem.Requests.Permissions;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Dawem.Requests.Justifications;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Requests
{
    public class RequestOvertimeBL : IRequestOvertimeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRequestOvertimeBLValidation requestOvertimeBLValidation;
        private readonly IRequestBLValidation requestBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;
        private readonly INotificationHandleBL notificationHandleBL;

        public RequestOvertimeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IRequestBLValidation _requestBLValidation,
            IMapper _mapper,
            IUploadBLC _uploadBLC,
           RequestInfo _requestHeaderContext,
           IRequestOvertimeBLValidation _requestOvertimeBLValidation,
           INotificationHandleBL _notificationHandleBL)
        {
            unitOfWork = _unitOfWork;
            requestBLValidation = _requestBLValidation;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            requestOvertimeBLValidation = _requestOvertimeBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;
            notificationHandleBL = _notificationHandleBL;

        }
        public async Task<int> Create(CreateRequestOvertimeDTO model)
        {
            #region Model Validation

            var validator = new CreateRequestOvertimeDTOValidator();
            var validatorResult = validator.Validate(model);
            if (!validatorResult.IsValid)
            {
                var error = validatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            var employeeId = await requestOvertimeBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Upload Files

            List<string> fileNames = null;

            if (model.Attachments != null && model.Attachments.Count > 0)
            {
                fileNames = new List<string>();

                foreach (var attachment in model.Attachments)
                {
                    if (attachment != null && attachment.Length > 0)
                    {
                        var result = await uploadBLC.UploadFile(attachment, LeillaKeys.OvertimeRequests)
                            ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadRequestAttachements); ;
                        fileNames.Add(result.FileName);
                    }
                }
                model.AttachmentsNames = fileNames;
            }

            #endregion

            #region Insert Request Overtime

            #region Set Request Overtime Code

            var getRequestNextCode = await repositoryManager.RequestRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            var getRequestOvertimeNextCode = await repositoryManager.RequestRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var request = mapper.Map<Request>(model);
            request.CompanyId = requestInfo.CompanyId;
            request.AddUserId = requestInfo.UserId;
            request.EmployeeId = employeeId ?? 0;
            request.Code = getRequestNextCode;
            request.RequestOvertime.Code = getRequestOvertimeNextCode;
            request.Status = requestInfo.ApplicationType == ApplicationType.Web ? RequestStatus.Accepted : RequestStatus.Pending;
            request.IsActive = true;
            request.RequestOvertime.IsActive = true;

            repositoryManager.RequestRepository.Insert(request);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Notifications

            var requestEmployee = await repositoryManager
                .EmployeeRepository.Get(r => r.Id == employeeId && r.DirectManagerId > 0)
                .Select(e => new
                {
                    DirectManagerId = e.DirectManagerId.Value
                }).FirstOrDefaultAsync();

            var notificationUsers = await repositoryManager.UserRepository.
                Get(s => !s.IsDeleted && s.IsActive &
                s.EmployeeId == requestEmployee.DirectManagerId).
                Select(u => new NotificationUserModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserTokens = u.NotificationUsers.
                    Where(nu => !nu.IsDeleted && nu.NotificationUserFCMTokens.
                    Any(f => !f.IsDeleted)).
                    SelectMany(nu => nu.NotificationUserFCMTokens.Where(f => !f.IsDeleted).
                    Select(f => new NotificationUserTokenModel
                    {
                        ApplicationType = f.DeviceType,
                        Token = f.FCMToken
                    })).ToList()
                }).ToListAsync();

            var employeeIds = new List<int>() { requestEmployee.DirectManagerId };

            var handleNotificationModel = new HandleNotificationModel
            {
                NotificationUsers = notificationUsers,
                EmployeeIds = employeeIds,
                NotificationType = NotificationType.NewOvertimeRequest,
                NotificationStatus = NotificationStatus.Info,
                Priority = NotificationPriority.Medium
            };

            await notificationHandleBL.HandleNotifications(handleNotificationModel);

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return request.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateRequestOvertimeDTO model)
        {
            #region Model Validation

            var validator = new UpdateRequestOvertimeModelDTOValidator();
            var validatorResult = validator.Validate(model);
            if (!validatorResult.IsValid)
            {
                var error = validatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            var employeeId = await requestOvertimeBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Upload Files

            var newFileNames = new List<string>();

            if (model.Attachments != null && model.Attachments.Count > 0)
            {
                newFileNames = new List<string>();

                foreach (var attachment in model.Attachments)
                {
                    if (attachment != null && attachment.Length > 0)
                    {
                        var result = await uploadBLC.UploadFile(attachment, LeillaKeys.OvertimeRequests)
                            ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadRequestAttachements); ;
                        newFileNames.Add(result.FileName);
                    }
                }
            }

            #endregion

            #region Update Request Overtime

            var getRequest = await repositoryManager.RequestRepository
                 .GetEntityByConditionWithTrackingAsync(request => !request.IsDeleted
                 && request.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            var getRequestOvertime = await repositoryManager.RequestOvertimeRepository
                 .GetEntityByConditionWithTrackingAsync(requestOvertime => !requestOvertime.Request.IsDeleted
                 && requestOvertime.Request.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            getRequest.EmployeeId = employeeId ?? 0;
            getRequest.ForEmployee = model.ForEmployee;
            getRequest.IsNecessary = model.IsNecessary;
            getRequest.Date = model.OvertimeDate;            
            getRequest.ModifiedDate = DateTime.Now;
            getRequest.ModifyUserId = requestInfo.UserId;
            getRequest.Notes = model.Notes;


            getRequestOvertime.OvertimeTypeId = model.OvertimeTypeId;
            getRequestOvertime.ModifiedDate = DateTime.Now;
            getRequestOvertime.ModifyUserId = requestInfo.UserId;
            getRequestOvertime.DateFrom = model.DateFrom;
            getRequestOvertime.DateTo = model.DateTo;
            getRequestOvertime.Notes = model.Notes;

            await unitOfWork.SaveAsync();

            #region Update Attachements 

            var existAttachementsDbList = await repositoryManager.RequestAttachmentRepository
                    .Get(e => e.RequestId == getRequest.Id)
                    .ToListAsync();

            var existingFileNames = existAttachementsDbList.Select(e => e.FileName).ToList();

            var addedAttachements = newFileNames
                .Select(fileName => new RequestAttachment
                {
                    RequestId = getRequest.Id,
                    FileName = fileName,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList();

            var removedFileNames = existAttachementsDbList
                .Where(ge => model.AttachmentsNames == null || !model.AttachmentsNames.Contains(ge.FileName))
                .Select(ge => ge.FileName)
                .ToList();

            var removedAttachments = await repositoryManager.RequestAttachmentRepository
                .Get(e => e.RequestId == getRequest.Id && removedFileNames.Contains(e.FileName))
                .ToListAsync();

            if (removedAttachments.Count > 0)
                repositoryManager.RequestAttachmentRepository.BulkDeleteIfExist(removedAttachments);
            if (addedAttachements.Count > 0)
                repositoryManager.RequestAttachmentRepository.BulkInsert(addedAttachements);

            await unitOfWork.SaveAsync();

            #endregion

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();

            return true;

            #endregion
        }
        public async Task<GetRequestOvertimesResponse> Get(GetRequestOvertimeCriteria criteria)
        {
            var requestOvertimeRepository = repositoryManager.RequestOvertimeRepository;
            var query = requestOvertimeRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = requestOvertimeRepository.OrderBy(query, nameof(RequestOvertime.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestOvertimesList = await queryPaged.IgnoreQueryFilters().
                Select(requestOvertime => new GetRequestOvertimesResponseModel
            {
                Id = requestOvertime.Request.Id,
                Code = requestOvertime.Request.Code,
                Employee = new RequestEmployeeModel
                {
                    EmployeeNumber = requestOvertime.Request.Employee.EmployeeNumber,
                    Name = requestOvertime.Request.Employee.Name,
                    ProfileImagePath = uploadBLC.GetFilePath(requestOvertime.Request.Employee.ProfileImageName, LeillaKeys.Employees)
                },
                OvertimeTypeName = requestOvertime.OvertimeType.Name,
                DateFrom = requestOvertime.Request.Date,
                DateTo = requestOvertime.DateTo,
                Status = requestOvertime.Request.Status,
                StatusName = TranslationHelper.GetTranslation(requestOvertime.Request.Status.ToString(), requestInfo.Lang)

            }).ToListAsync();

            return new GetRequestOvertimesResponse
            {
                OvertimeRequests = requestOvertimesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<EmployeeGetRequestOvertimesResponseDTO> EmployeeGet(EmployeeGetRequestOvertimeCriteria criteria)
        {
            #region Is Employee Validation

            await requestBLValidation.IsEmployeeValidation();

            #endregion

            var requestOvertimeRepository = repositoryManager.RequestOvertimeRepository;
            var query = requestOvertimeRepository.EmployeeGetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = requestOvertimeRepository.OrderBy(query, nameof(RequestOvertime.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestOvertimesList = await queryPaged.IgnoreQueryFilters().
                Select(requestOvertime => new EmployeeGetRequestOvertimesResponseModelDTO
                {
                    Id = requestOvertime.Request.Id,
                    Code = requestOvertime.Request.Code,
                    AddedDate = requestOvertime.Request.AddedDate,
                    DirectManagerName = requestOvertime.Request.Employee.DirectManager != null ?
                    requestOvertime.Request.Employee.DirectManager.Name : null,
                    OvertimeTypeName = requestOvertime.OvertimeType.Name,
                    DateFrom = requestOvertime.Request.Date,
                    DateTo = requestOvertime.DateTo,
                    Status = requestOvertime.Request.Status,
                    StatusName = TranslationHelper.GetTranslation(requestOvertime.Request.Status.ToString(), requestInfo.Lang)
                }).ToListAsync();

            return new EmployeeGetRequestOvertimesResponseDTO
            {
                OvertimeRequests = requestOvertimesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetRequestOvertimesForDropDownResponse> GetForDropDown(GetRequestOvertimeCriteria criteria)
        {
            criteria.IsActive = true;
            var requestOvertimeRepository = repositoryManager.RequestOvertimeRepository;
            var query = requestOvertimeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = requestOvertimeRepository.OrderBy(query, nameof(RequestOvertime.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestOvertimesList = await queryPaged.Select(e => new GetRequestOvertimesForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Request.Employee.Name
            }).ToListAsync();

            return new GetRequestOvertimesForDropDownResponse
            {
                OvertimeRequests = requestOvertimesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetRequestOvertimeInfoResponseModel> GetInfo(int requestId)
        {
            var requestOvertime = await repositoryManager.RequestOvertimeRepository.
                Get(e => e.Request.Id == requestId && e.Request.Type == RequestType.Overtime && !e.Request.IsDeleted).IgnoreQueryFilters()
                .Select(requestOvertime => new GetRequestOvertimeInfoResponseModel
                {
                    Code = requestOvertime.Request.Code,
                    Employee = new RequestEmployeeModel
                    {
                        EmployeeNumber = requestOvertime.Request.Employee.EmployeeNumber,
                        Name = requestOvertime.Request.Employee.Name,
                        ProfileImagePath = uploadBLC.GetFilePath(requestOvertime.Request.Employee.ProfileImageName, LeillaKeys.Employees)
                    },
                    OvertimeTypeName = requestOvertime.OvertimeType.Name,
                    DateFrom = requestOvertime.Request.Date,
                    DateTo = requestOvertime.DateTo,
                    IsActive = requestOvertime.Request.IsActive,
                    IsNecessary = requestOvertime.Request.IsNecessary,
                    ForEmployee = requestOvertime.Request.ForEmployee,
                    Attachments = requestOvertime.Request.RequestAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, LeillaKeys.OvertimeRequests),
                    }).ToList(),
                    Status = requestOvertime.Request.Status,
                    StatusName = TranslationHelper.GetTranslation(requestOvertime.Request.Status.ToString(), requestInfo.Lang),
                    Notes = requestOvertime.Request.Notes,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            return requestOvertime;
        }
        public async Task<GetRequestOvertimeByIdResponseModel> GetById(int RequestOvertimeId)
        {
            var requestOvertime = await repositoryManager.RequestOvertimeRepository.
                Get(e => e.Request.Id == RequestOvertimeId && e.Request.Type == RequestType.Overtime && !e.IsDeleted)
                .Select(requestOvertime => new GetRequestOvertimeByIdResponseModel
                {
                    Id = requestOvertime.Request.Id,
                    Code = requestOvertime.Request.Code,

                    EmployeeId = requestOvertime.Request.EmployeeId,
                    OvertimeTypeId = requestOvertime.OvertimeTypeId,
                    DateFrom = requestOvertime.Request.Date,
                    DateTo = requestOvertime.DateTo,
                    Status = requestOvertime.Request.Status,
                    IsActive = requestOvertime.Request.IsActive,
                    IsNecessary = requestOvertime.Request.IsNecessary,
                    ForEmployee = requestOvertime.Request.ForEmployee,
                    Attachments = requestOvertime.Request.RequestAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, LeillaKeys.OvertimeRequests)
                    }).ToList(),
                    Notes = requestOvertime.Request.Notes
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            return requestOvertime;

        }
        public async Task<bool> Delete(int requestId)
        {
            var requestOvertime = await repositoryManager.RequestRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == requestId) ??
                throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);
            requestOvertime.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Accept(int requestId)
        {
            var request = await repositoryManager.RequestRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == requestId) ??
               throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            if (request.Status == RequestStatus.Accepted)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestAlreadyAccepted);
            }
            else if (request.Status == RequestStatus.Rejected)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestAlreadyRejected);
            }

            request.Status = RequestStatus.Accepted;
            request.DecisionUserId = requestInfo.UserId;
            request.DecisionDate = DateTime.UtcNow;

            await unitOfWork.SaveAsync();

            #region Handle Notifications

            var notificationUsers = await repositoryManager.UserRepository.
                Get(s => !s.IsDeleted && s.IsActive &
                s.EmployeeId == request.EmployeeId).
                Select(u => new NotificationUserModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserTokens = u.NotificationUsers.
                    Where(nu => !nu.IsDeleted && nu.NotificationUserFCMTokens.
                    Any(f => !f.IsDeleted)).
                    SelectMany(nu => nu.NotificationUserFCMTokens.Where(f => !f.IsDeleted).
                    Select(f => new NotificationUserTokenModel
                    {
                        ApplicationType = f.DeviceType,
                        Token = f.FCMToken
                    })).ToList()
                }).ToListAsync();

            var employeeIds = new List<int>() { request.EmployeeId };

            var handleNotificationModel = new HandleNotificationModel
            {
                NotificationUsers = notificationUsers,
                EmployeeIds = employeeIds,
                NotificationType = NotificationType.AcceptingOvertimeRequest,
                NotificationStatus = NotificationStatus.Info,
                Priority = NotificationPriority.Medium
            };

            await notificationHandleBL.HandleNotifications(handleNotificationModel);

            #endregion

            return true;
        }
        public async Task<bool> Reject(RejectModelDTO rejectModelDTO)
        {
            var request = await repositoryManager.RequestRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == rejectModelDTO.Id) ??
               throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            if (request.Status == RequestStatus.Accepted)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestAlreadyAccepted);
            }
            else if (request.Status == RequestStatus.Rejected)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestAlreadyRejected);
            }

            request.Status = RequestStatus.Rejected;
            request.DecisionUserId = requestInfo.UserId;
            request.DecisionDate = DateTime.UtcNow;
            request.RejectReason = rejectModelDTO.RejectReason;

            await unitOfWork.SaveAsync();

            #region Handle Notifications

            var notificationUsers = await repositoryManager.UserRepository.
                Get(s => !s.IsDeleted && s.IsActive &
                s.EmployeeId == request.EmployeeId).
                Select(u => new NotificationUserModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserTokens = u.NotificationUsers.
                    Where(nu => !nu.IsDeleted && nu.NotificationUserFCMTokens.
                    Any(f => !f.IsDeleted)).
                    SelectMany(nu => nu.NotificationUserFCMTokens.Where(f => !f.IsDeleted).
                    Select(f => new NotificationUserTokenModel
                    {
                        ApplicationType = f.DeviceType,
                        Token = f.FCMToken
                    })).ToList()
                }).ToListAsync();

            var employeeIds = new List<int>() { request.EmployeeId };

            var handleNotificationModel = new HandleNotificationModel
            {
                NotificationUsers = notificationUsers,
                EmployeeIds = employeeIds,
                NotificationType = NotificationType.RejectingOvertimeRequest,
                NotificationStatus = NotificationStatus.Info,
                Priority = NotificationPriority.Medium
            };

            await notificationHandleBL.HandleNotifications(handleNotificationModel);

            #endregion

            return true;
        }
        public async Task<GetOvertimesInformationsResponseDTO> GetOvertimesInformations()
        {
            var requestOvertimeRepository = repositoryManager.RequestOvertimeRepository;
            var query = requestOvertimeRepository.Get(requestOvertime => requestOvertime.Request.Type == RequestType.Overtime
            && !requestOvertime.Request.IsDeleted &&
            requestOvertime.Request.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetOvertimesInformationsResponseDTO
            {
                TotalOvertimesCount = await query.CountAsync(),
                AcceptedCount = await query.Where(request => request.Request.Status == RequestStatus.Accepted).CountAsync(),
                RejectedCount = await query.Where(request => request.Request.Status == RequestStatus.Rejected).CountAsync(),
                PendingCount = await query.Where(request => request.Request.Status == RequestStatus.Pending).CountAsync()
            };

            #endregion
        }
    }
}

