using AutoMapper;
using Dawem.BusinessLogic.Dawem.Core.NotificationsStores;
using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessLogic.Dawem.Requests;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.BusinessValidation.Dawem.Requests;
using Dawem.Contract.RealTime.Firebase;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Requests;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Others;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Requests;
using Dawem.Models.Requests.Permissions;
using Dawem.Models.Response.Dawem.Requests;
using Dawem.Models.Response.Dawem.Requests.Permissions; 
using Dawem.RealTime.Helper;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Dawem.Requests.Permissions;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Requests
{
    public class RequestPermissionBL : IRequestPermissionBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRequestPermissionBLValidation requestPermissionBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IRequestBLValidation requestBLValidation;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;
        private readonly INotificationHandleBL notificationHandleBL;

        public RequestPermissionBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IRequestBLValidation _requestBLValidation,
            IMapper _mapper,
            IUploadBLC _uploadBLC,
           RequestInfo _requestHeaderContext,
           IRequestPermissionBLValidation _requestPermissionBLValidation,
           INotificationHandleBL _notificationHandleBL)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            requestPermissionBLValidation = _requestPermissionBLValidation;
            mapper = _mapper;
            requestBLValidation = _requestBLValidation;
            uploadBLC = _uploadBLC;
            notificationHandleBL = _notificationHandleBL;
        }
        public async Task<int> Create(CreateRequestPermissionModelDTO model)
        {
            #region Model Validation

            var validator = new CreateRequestPermissionModelDTOValidator();
            var validatorResult = validator.Validate(model);
            if (!validatorResult.IsValid)
            {
                var error = validatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            var employeeId = await requestPermissionBLValidation.CreateValidation(model);

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
                        var result = await uploadBLC.UploadFile(attachment, LeillaKeys.PermissionRequests)
                            ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadRequestAttachements);
                        fileNames.Add(result.FileName);
                    }
                }
                model.AttachmentsNames = fileNames;
            }

            #endregion

            #region Insert Request Permission

            #region Set Request Permission Code

            var getRequestNextCode = await repositoryManager.RequestRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            var getRequestPermissionNextCode = await repositoryManager.RequestRepository
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
            request.RequestPermission.Code = getRequestPermissionNextCode;
            request.Status = RequestStatus.Pending;
            request.IsActive = true;
            request.RequestPermission.IsActive = true;

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

            var userIds = await repositoryManager.UserRepository.
                Get(s => !s.IsDeleted && s.IsActive &
                s.EmployeeId == requestEmployee.DirectManagerId).
                Select(u => u.Id).ToListAsync();

            var employeeIds = new List<int>() { requestEmployee.DirectManagerId };

            var handleNotificationModel = new HandleNotificationModel
            {
                UserIds = userIds,
                EmployeeIds = employeeIds,
                NotificationType = NotificationType.NewPermissionRequent,
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
        public async Task<bool> Update(UpdateRequestPermissionModelDTO model)
        {
            #region Model Validation

            var validator = new UpdateRequestPermissionModelDTOValidator();
            var validatorResult = validator.Validate(model);
            if (!validatorResult.IsValid)
            {
                var error = validatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            var employeeId = await requestPermissionBLValidation.UpdateValidation(model);

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
                        var result = await uploadBLC.UploadFile(attachment, LeillaKeys.PermissionRequests)
                            ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadRequestAttachements); ;
                        newFileNames.Add(result.FileName);
                    }
                }
            }

            #endregion

            #region Update Request Permission

            var getRequest = await repositoryManager.RequestRepository
                 .GetEntityByConditionWithTrackingAsync(request => !request.IsDeleted
                 && request.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            var getRequestPermission = await repositoryManager.RequestPermissionRepository
                 .GetEntityByConditionWithTrackingAsync(requestPermission => !requestPermission.Request.IsDeleted
                 && requestPermission.Request.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            getRequest.EmployeeId = employeeId ?? 0;
            getRequest.ForEmployee = model.ForEmployee;
            getRequest.IsNecessary = model.IsNecessary;
            getRequest.Date = model.DateFrom;
            getRequest.ModifiedDate = DateTime.Now;
            getRequest.ModifyUserId = requestInfo.UserId;
            getRequest.Notes = model.Notes;


            getRequestPermission.PermissionTypeId = model.PermissionTypeId;
            getRequestPermission.ModifiedDate = DateTime.Now;
            getRequestPermission.ModifyUserId = requestInfo.UserId;
            getRequestPermission.DateTo = model.DateTo;
            getRequestPermission.Notes = model.Notes;

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
        public async Task<GetRequestPermissionsResponse> Get(GetRequestPermissionsCriteria criteria)
        {
            var requestPermissionRepository = repositoryManager.RequestPermissionRepository;
            var query = requestPermissionRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = requestPermissionRepository.OrderBy(query, nameof(RequestPermission.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestPermissionsList = await queryPaged.Select(requestPermission => new GetRequestPermissionsResponseModel
            {
                Id = requestPermission.Request.Id,
                Code = requestPermission.Request.Code,
                Employee = new RequestEmployeeModel
                {
                    EmployeeNumber = requestPermission.Request.Employee.EmployeeNumber,
                    Name = requestPermission.Request.Employee.Name,
                    ProfileImagePath = uploadBLC.GetFilePath(requestPermission.Request.Employee.ProfileImageName, LeillaKeys.Employees)
                },
                PermissionTypeName = requestPermission.PermissionType.Name,
                DateFrom = requestPermission.Request.Date,
                DateTo = requestPermission.DateTo,
                Period = LeillaKeys.LeftBracket +
                (requestPermission.DateTo - requestPermission.Request.Date).TotalHours +
                LeillaKeys.RightBracket + LeillaKeys.Space + TranslationHelper.GetTranslation(LeillaKeys.Hour, requestInfo.Lang),
                Status = requestPermission.Request.Status,
                StatusName = TranslationHelper.GetTranslation(requestPermission.Request.Status.ToString(), requestInfo.Lang)

            }).ToListAsync();

            return new GetRequestPermissionsResponse
            {
                PermissionRequests = requestPermissionsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<EmployeeGetRequestPermissionsResponseDTO> EmployeeGet(EmployeeGetRequestPermissionsCriteria criteria)
        {
            #region Is Employee Validation

            await requestBLValidation.IsEmployeeValidation();

            #endregion

            var requestPermissionRepository = repositoryManager.RequestPermissionRepository;
            var query = requestPermissionRepository.EmployeeGetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = requestPermissionRepository.OrderBy(query, nameof(RequestPermission.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestPermissionsList = await queryPaged.Select(requestPermission => new EmployeeGetRequestPermissionsResponseModelDTO
            {
                Id = requestPermission.Request.Id,
                Code = requestPermission.Request.Code,
                AddedDate = requestPermission.Request.AddedDate,
                DirectManagerName = requestPermission.Request.Employee.DirectManager != null ?
                requestPermission.Request.Employee.DirectManager.Name : null,
                PermissionTypeName = requestPermission.PermissionType.Name,
                DateFrom = requestPermission.Request.Date,
                DateTo = requestPermission.DateTo,
                Status = requestPermission.Request.Status,
                StatusName = TranslationHelper.GetTranslation(requestPermission.Request.Status.ToString(), requestInfo.Lang)
            }).ToListAsync();

            return new EmployeeGetRequestPermissionsResponseDTO
            {
                PermissionRequests = requestPermissionsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetRequestPermissionsForDropDownResponse> GetForDropDown(GetRequestPermissionsCriteria criteria)
        {
            criteria.IsActive = true;
            var requestPermissionRepository = repositoryManager.RequestPermissionRepository;
            var query = requestPermissionRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = requestPermissionRepository.OrderBy(query, nameof(RequestPermission.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestPermissionsList = await queryPaged.Select(e => new GetRequestPermissionsForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Request.Employee.Name
            }).ToListAsync();

            return new GetRequestPermissionsForDropDownResponse
            {
                PermissionRequests = requestPermissionsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetRequestPermissionInfoResponseModel> GetInfo(int requestId)
        {
            var requestPermission = await repositoryManager.RequestPermissionRepository.Get(e => e.Request.Id == requestId && !e.Request.IsDeleted)
                .Select(requestPermission => new GetRequestPermissionInfoResponseModel
                {
                    Code = requestPermission.Request.Code,
                    Employee = new RequestEmployeeModel
                    {
                        EmployeeNumber = requestPermission.Request.Employee.EmployeeNumber,
                        Name = requestPermission.Request.Employee.Name,
                        ProfileImagePath = uploadBLC.GetFilePath(requestPermission.Request.Employee.ProfileImageName, LeillaKeys.Employees)
                    },
                    PermissionTypeName = requestPermission.PermissionType.Name,
                    DateFrom = requestPermission.Request.Date,
                    DateTo = requestPermission.DateTo,
                    Period = LeillaKeys.LeftBracket +
                    (requestPermission.DateTo - requestPermission.Request.Date).TotalHours +
                    LeillaKeys.RightBracket + LeillaKeys.Space + TranslationHelper.GetTranslation(LeillaKeys.Hour, requestInfo.Lang),
                    IsActive = requestPermission.Request.IsActive,
                    IsNecessary = requestPermission.Request.IsNecessary,
                    ForEmployee = requestPermission.Request.ForEmployee,
                    Attachments = requestPermission.Request.RequestAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, LeillaKeys.PermissionRequests),
                    }).ToList(),
                    Status = requestPermission.Request.Status,
                    StatusName = TranslationHelper.GetTranslation(requestPermission.Request.Status.ToString(), requestInfo.Lang),
                    Notes = requestPermission.Request.Notes
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            return requestPermission;
        }
        public async Task<GetRequestPermissionByIdResponseModel> GetById(int RequestPermissionId)
        {
            var requestPermission = await repositoryManager.RequestPermissionRepository.Get(e => e.Request.Id == RequestPermissionId && !e.IsDeleted)
                .Select(requestPermission => new GetRequestPermissionByIdResponseModel
                {
                    Id = requestPermission.Request.Id,
                    Code = requestPermission.Request.Code,
                    EmployeeId = requestPermission.Request.EmployeeId,
                    PermissionTypeId = requestPermission.PermissionTypeId,
                    DateFrom = requestPermission.Request.Date,
                    DateTo = requestPermission.DateTo,
                    Status = requestPermission.Request.Status,
                    IsActive = requestPermission.Request.IsActive,
                    IsNecessary = requestPermission.Request.IsNecessary,
                    ForEmployee = requestPermission.Request.ForEmployee,
                    Attachments = requestPermission.Request.RequestAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, LeillaKeys.PermissionRequests)
                    }).ToList(),
                    Notes = requestPermission.Request.Notes
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            return requestPermission;

        }
        public async Task<bool> Delete(int requestId)
        {
            var requestPermission = await repositoryManager.RequestRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == requestId) ??
                throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);
            requestPermission.Delete();
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

            var userIds = await repositoryManager.UserRepository.
                Get(s => !s.IsDeleted && s.IsActive &
                s.EmployeeId == request.EmployeeId).
                Select(u => u.Id).ToListAsync();

            var employeeIds = new List<int>() { request.EmployeeId };

            var handleNotificationModel = new HandleNotificationModel
            {
                UserIds = userIds,
                EmployeeIds = employeeIds,
                NotificationType = NotificationType.AcceptingPermissionRequest,
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

            var userIds = await repositoryManager.UserRepository.
                Get(s => !s.IsDeleted && s.IsActive &
                s.EmployeeId == request.EmployeeId).
                Select(u => u.Id).ToListAsync();

            var employeeIds = new List<int>() { request.EmployeeId };

            var handleNotificationModel = new HandleNotificationModel
            {
                UserIds = userIds,
                EmployeeIds = employeeIds,
                NotificationType = NotificationType.RejectingPermissionRequest,
                NotificationStatus = NotificationStatus.Info,
                Priority = NotificationPriority.Medium
            };

            await notificationHandleBL.HandleNotifications(handleNotificationModel);

            #endregion

            return true;
        }
        public async Task<GetPermissionsInformationsResponseDTO> GetPermissionsInformations()
        {
            var requestPermissionRepository = repositoryManager.RequestPermissionRepository;
            var query = requestPermissionRepository.Get(requestPermission => requestPermission.Request.Type == RequestType.Permission &&
            !requestPermission.Request.IsDeleted &&
            requestPermission.Request.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetPermissionsInformationsResponseDTO
            {
                TotalPermissionsCount = await query.CountAsync(),
                AcceptedCount = await query.Where(request => request.Request.Status == RequestStatus.Accepted).CountAsync(),
                RejectedCount = await query.Where(request => request.Request.Status == RequestStatus.Rejected).CountAsync(),
                PendingCount = await query.Where(request => request.Request.Status == RequestStatus.Pending).CountAsync()
            };

            #endregion
        }
    }
}

