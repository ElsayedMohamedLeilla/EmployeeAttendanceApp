using AutoMapper;
using Dawem.Contract.BusinessLogic.Requests;
using Dawem.Contract.BusinessLogicCore;
using Dawem.Contract.BusinessValidation.Requests;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Others;
using Dawem.Models.Dtos.Requests;
using Dawem.Models.Dtos.Requests.Justifications;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Requests.Justifications;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Requests.Justifications;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Requests
{
    public class RequestJustificationBL : IRequestJustificationBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRequestJustificationBLValidation requestJustificationBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;
        public RequestJustificationBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
            IUploadBLC _uploadBLC,
           RequestInfo _requestHeaderContext,
           IRequestJustificationBLValidation _requestJustificationBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            requestJustificationBLValidation = _requestJustificationBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;
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
                        var result = await uploadBLC.UploadFile(attachment, AmgadKeys.justificationRequests)
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
                        var result = await uploadBLC.UploadFile(attachment, AmgadKeys.justificationRequests)
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

            getRequest.EmployeeId = model.EmployeeId ?? 0;
            getRequest.ForEmployee = model.ForEmployee;
            getRequest.IsNecessary = model.IsNecessary;
            getRequest.Date = model.DateFrom;
            getRequest.ModifiedDate = DateTime.Now;
            getRequest.ModifyUserId = requestInfo.UserId;


            getRequestJustification.ModifiedDate = DateTime.Now;
            getRequestJustification.ModifyUserId = requestInfo.UserId;
            getRequestJustification.DateTo = model.DateTo;

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

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestJustificationsList = await queryPaged.Select(requestJustification => new GetRequestJustificationsResponseModelDTO
            {
                Id = requestJustification.Request.Id,
                Code = requestJustification.Request.Code,
                EmployeeName = requestJustification.Request.Employee.Name,
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

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

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
                    EmployeeName = requestJustification.Request.Employee.Name,
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
                        FilePath = uploadBLC.GetFilePath(a.FileName, AmgadKeys.justificationRequests),
                    }).ToList(),
                    Status = requestJustification.Request.Status,
                    StatusName = TranslationHelper.GetTranslation(requestJustification.Request.Status.ToString(), requestInfo.Lang)
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
                        FilePath = uploadBLC.GetFilePath(a.FileName, AmgadKeys.justificationRequests)
                    }).ToList()
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
    }
}

