using AutoMapper;
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
using Dawem.Models.Dtos.Dawem.Others;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Requests;
using Dawem.Models.Requests.Justifications;
using Dawem.Models.Response.Dawem.Requests;
using Dawem.Models.Response.Dawem.Requests.Justifications;
using Dawem.RealTime.Helper;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Dawem.Requests.Justifications;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Requests
{
    public class RequestJustificationBL : IRequestJustificationBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRequestJustificationBLValidation requestJustificationBLValidation;
        private readonly IRequestBLValidation requestBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;
        private readonly INotificationService notificationServiceByFireBaseAdmin;

        public RequestJustificationBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IRequestBLValidation _requestBLValidation,
            IMapper _mapper,
            IUploadBLC _uploadBLC,
           RequestInfo _requestHeaderContext,
           IRequestJustificationBLValidation _requestJustificationBLValidation,
           INotificationService _notificationServiceByFireBaseAdmin)
        {
            unitOfWork = _unitOfWork;
            requestBLValidation = _requestBLValidation;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            requestJustificationBLValidation = _requestJustificationBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;
            notificationServiceByFireBaseAdmin = _notificationServiceByFireBaseAdmin;

        }
        public async Task<int> Create(CreateRequestJustificationDTO model)
        {
            #region Model Validation

            var validator = new CreateRequestJustificationDTOValidator();
            var validatorResult = validator.Validate(model);
            if (!validatorResult.IsValid)
            {
                var error = validatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            var employeeId = await requestJustificationBLValidation.CreateValidation(model);

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
                        var result = await uploadBLC.UploadFile(attachment, AmgadKeys.JustificationRequests)
                            ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadRequestAttachements); ;
                        fileNames.Add(result.FileName);
                    }
                }
                model.AttachmentsNames = fileNames;
            }

            #endregion

            #region Insert Request Justification

            #region Set Request Justification Code

            var getRequestNextCode = await repositoryManager.RequestRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            var getRequestJustificationNextCode = await repositoryManager.RequestRepository
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
            request.RequestJustification.Code = getRequestJustificationNextCode;
            request.Status = RequestStatus.Pending;
            request.IsActive = true;
            request.RequestJustification.IsActive = true;

            repositoryManager.RequestRepository.Insert(request);
            await unitOfWork.SaveAsync();

            #endregion

            var requestEmployee = await repositoryManager
               .EmployeeRepository.Get(r => r.Id == employeeId)
               .Select(e => new
               {
                   e.Name,
                   e.DirectManagerId 
               }).FirstOrDefaultAsync();

            #region Save Notification In DB

            if(requestEmployee.DirectManagerId > 0)
            {
                var getNotificationNextCode = await repositoryManager.NotificationStoreRepository
              .Get(e => e.CompanyId == requestInfo.CompanyId)
              .Select(e => e.Code)
              .DefaultIfEmpty()
              .MaxAsync() + 1;
                var notificationStore = new NotificationStore()
                {
                    Code = getNotificationNextCode,
                    EmployeeId = requestEmployee.DirectManagerId ?? 0,
                    CompanyId = requestInfo.CompanyId,
                    AddUserId = requestInfo.UserId,
                    AddedDate = DateTime.UtcNow,
                    Status = NotificationStatus.Info,
                    NotificationType = NotificationType.NewJustificationRequest,
                    IsRead = false,
                    IsActive = true,
                    Priority = Priority.Medium

                };
                repositoryManager.NotificationStoreRepository.Insert(notificationStore);
                await unitOfWork.SaveAsync();
            
           
            #endregion

            #region Fire Notification & Email
            List<int> userIds = repositoryManager.UserRepository.Get(s => !s.IsDeleted && s.IsActive & s.EmployeeId == requestEmployee.DirectManagerId).Select(u => u.Id).ToList();
            if (userIds.Count > 0)
            {
                await notificationServiceByFireBaseAdmin.SendNotificationsAndEmails(userIds, NotificationType.NewVacationRequest, NotificationStatus.Info);
            }
                #endregion
            }

            #region Handle Response

            await unitOfWork.CommitAsync();
            return request.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateRequestJustificationDTO model)
        {
            #region Model Validation

            var validator = new UpdateRequestJustificationModelDTOValidator();
            var validatorResult = validator.Validate(model);
            if (!validatorResult.IsValid)
            {
                var error = validatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            var employeeId = await requestJustificationBLValidation.UpdateValidation(model);

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
                        var result = await uploadBLC.UploadFile(attachment, AmgadKeys.JustificationRequests)
                            ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadRequestAttachements); ;
                        newFileNames.Add(result.FileName);
                    }
                }
            }

            #endregion

            #region Update Request Justification

            var getRequest = await repositoryManager.RequestRepository
                 .GetEntityByConditionWithTrackingAsync(request => !request.IsDeleted
                 && request.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            var getRequestJustification = await repositoryManager.RequestJustificationRepository
                 .GetEntityByConditionWithTrackingAsync(requestJustification => !requestJustification.Request.IsDeleted
                 && requestJustification.Request.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            getRequest.EmployeeId = employeeId ?? 0;
            getRequest.ForEmployee = model.ForEmployee;
            getRequest.IsNecessary = model.IsNecessary;
            getRequest.Date = model.DateFrom;
            getRequest.ModifiedDate = DateTime.Now;
            getRequest.ModifyUserId = requestInfo.UserId;
            getRequest.Notes = model.Notes;


            getRequestJustification.JustificationTypeId = model.JustificationTypeId;
            getRequestJustification.ModifiedDate = DateTime.Now;
            getRequestJustification.ModifyUserId = requestInfo.UserId;
            getRequestJustification.DateTo = model.DateTo;
            getRequestJustification.Notes = model.Notes;

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
        public async Task<GetRequestJustificationsResponseDTO> Get(GetRequestJustificationCriteria criteria)
        {
            var requestJustificationRepository = repositoryManager.RequestJustificationRepository;
            var query = requestJustificationRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = requestJustificationRepository.OrderBy(query, nameof(RequestJustification.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestJustificationsList = await queryPaged.Select(requestJustification => new GetRequestJustificationsResponseModelDTO
            {
                Id = requestJustification.Request.Id,
                Code = requestJustification.Request.Code,
                Employee = new RequestEmployeeModel
                {
                    EmployeeNumber = requestJustification.Request.Employee.EmployeeNumber,
                    Name = requestJustification.Request.Employee.Name,
                    ProfileImagePath = uploadBLC.GetFilePath(requestJustification.Request.Employee.ProfileImageName, LeillaKeys.Employees)
                },
                JustificationTypeName = requestJustification.JustificatioType.Name,
                DateFrom = requestJustification.Request.Date,
                DateTo = requestJustification.DateTo,
                Status = requestJustification.Request.Status,
                StatusName = TranslationHelper.GetTranslation(requestJustification.Request.Status.ToString(), requestInfo.Lang)

            }).ToListAsync();

            return new GetRequestJustificationsResponseDTO
            {
                JustificationRequests = requestJustificationsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<EmployeeGetRequestJustificationsResponseDTO> EmployeeGet(EmployeeGetRequestJustificationCriteria criteria)
        {
            #region Is Employee Validation

            await requestBLValidation.IsEmployeeValidation();

            #endregion

            var requestJustificationRepository = repositoryManager.RequestJustificationRepository;
            var query = requestJustificationRepository.EmployeeGetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = requestJustificationRepository.OrderBy(query, nameof(RequestJustification.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestJustificationsList = await queryPaged.Select(requestJustification => new EmployeeGetRequestJustificationsResponseModelDTO
            {
                Id = requestJustification.Request.Id,
                Code = requestJustification.Request.Code,
                AddedDate = requestJustification.Request.AddedDate,
                DirectManagerName = requestJustification.Request.Employee.DirectManager != null ?
                requestJustification.Request.Employee.DirectManager.Name : null,
                JustificationTypeName = requestJustification.JustificatioType.Name,
                DateFrom = requestJustification.Request.Date,
                DateTo = requestJustification.DateTo,
                Status = requestJustification.Request.Status,
                StatusName = TranslationHelper.GetTranslation(requestJustification.Request.Status.ToString(), requestInfo.Lang)
            }).ToListAsync();

            return new EmployeeGetRequestJustificationsResponseDTO
            {
                JustificationRequests = requestJustificationsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetRequestJustificationsForDropDownResponseDTO> GetForDropDown(GetRequestJustificationCriteria criteria)
        {
            criteria.IsActive = true;
            var requestJustificationRepository = repositoryManager.RequestJustificationRepository;
            var query = requestJustificationRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = requestJustificationRepository.OrderBy(query, nameof(RequestJustification.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestJustificationsList = await queryPaged.Select(e => new GetRequestJustificationsForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Request.Employee.Name
            }).ToListAsync();

            return new GetRequestJustificationsForDropDownResponseDTO
            {
                JustificationRequests = requestJustificationsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetRequestJustificationInfoResponseDTO> GetInfo(int requestId)
        {
            var requestJustification = await repositoryManager.RequestJustificationRepository.Get(e => e.Request.Id == requestId && !e.Request.IsDeleted)
                .Select(requestJustification => new GetRequestJustificationInfoResponseDTO
                {
                    Code = requestJustification.Request.Code,
                    Employee = new RequestEmployeeModel
                    {
                        EmployeeNumber = requestJustification.Request.Employee.EmployeeNumber,
                        Name = requestJustification.Request.Employee.Name,
                        ProfileImagePath = uploadBLC.GetFilePath(requestJustification.Request.Employee.ProfileImageName, LeillaKeys.Employees)
                    },
                    JustificationTypeName = requestJustification.JustificatioType.Name,
                    DateFrom = requestJustification.Request.Date,
                    DateTo = requestJustification.DateTo,
                    IsActive = requestJustification.Request.IsActive,
                    IsNecessary = requestJustification.Request.IsNecessary,
                    ForEmployee = requestJustification.Request.ForEmployee,
                    Attachments = requestJustification.Request.RequestAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, AmgadKeys.JustificationRequests),
                    }).ToList(),
                    Status = requestJustification.Request.Status,
                    StatusName = TranslationHelper.GetTranslation(requestJustification.Request.Status.ToString(), requestInfo.Lang),
                    Notes = requestJustification.Request.Notes,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            return requestJustification;
        }
        public async Task<GetRequestJustificationByIdResponseDTO> GetById(int RequestJustificationId)
        {
            var requestJustification = await repositoryManager.RequestJustificationRepository.Get(e => e.Request.Id == RequestJustificationId && !e.IsDeleted)
                .Select(requestJustification => new GetRequestJustificationByIdResponseDTO
                {
                    Id = requestJustification.Request.Id,
                    Code = requestJustification.Request.Code,
                    EmployeeId = requestJustification.Request.EmployeeId,
                    JustificationTypeId = requestJustification.JustificationTypeId,
                    DateFrom = requestJustification.Request.Date,
                    DateTo = requestJustification.DateTo,
                    IsActive = requestJustification.Request.IsActive,
                    IsNecessary = requestJustification.Request.IsNecessary,
                    ForEmployee = requestJustification.Request.ForEmployee,
                    Attachments = requestJustification.Request.RequestAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, AmgadKeys.JustificationRequests)
                    }).ToList(),
                    Notes = requestJustification.Request.Notes
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            return requestJustification;

        }
        public async Task<bool> Delete(int requestId)
        {
            var requestJustification = await repositoryManager.RequestRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == requestId) ??
                throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);
            requestJustification.Delete();
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
            return true;
        }
        public async Task<GetJustificationsInformationsResponseDTO> GetJustificationsInformations()
        {
            var requestJustificationRepository = repositoryManager.RequestJustificationRepository;
            var query = requestJustificationRepository.Get(requestJustification => requestJustification.Request.Type == RequestType.Justification
            && !requestJustification.Request.IsDeleted &&
            requestJustification.Request.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetJustificationsInformationsResponseDTO
            {
                TotalJustificationsCount = await query.CountAsync(),
                AcceptedCount = await query.Where(request => request.Request.Status == RequestStatus.Accepted).CountAsync(),
                RejectedCount = await query.Where(request => request.Request.Status == RequestStatus.Rejected).CountAsync(),
                PendingCount = await query.Where(request => request.Request.Status == RequestStatus.Pending).CountAsync()
            };

            #endregion
        }
    }
}

