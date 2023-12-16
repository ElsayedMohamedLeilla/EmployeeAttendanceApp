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
using Dawem.Models.Dtos.Requests.Vacations;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Requests;
using Dawem.Models.Response.Requests.Justifications;
using Dawem.Models.Response.Requests.Vacations;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Requests.Vacations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Requests
{
    public class RequestVacationBL : IRequestVacationBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRequestVacationBLValidation requestVacationBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;
        private readonly IRequestBLValidation requestBLValidation;
        public RequestVacationBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
            IRequestBLValidation _requestBLValidation,
            IUploadBLC _uploadBLC,
           RequestInfo _requestHeaderContext,
           IRequestVacationBLValidation _requestVacationBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            requestVacationBLValidation = _requestVacationBLValidation;
            mapper = _mapper;
            requestBLValidation = _requestBLValidation;
            uploadBLC = _uploadBLC;
        }
        public async Task<int> Create(CreateRequestVacationDTO model)
        {
            #region Model Validation

            var validator = new CreateRequestVacationDTOValidator();
            var validatorResult = validator.Validate(model);
            if (!validatorResult.IsValid)
            {
                var error = validatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            var employeeId = await requestVacationBLValidation.CreateValidation(model);

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
                        var result = await uploadBLC.UploadFile(attachment, LeillaKeys.VacationRequests)
                            ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadRequestAttachements); ;
                        fileNames.Add(result.FileName);
                    }
                }
                model.AttachmentsNames = fileNames;
            }

            #endregion

            #region Insert Request Vacation

            #region Set Request Vacation Code

            var getRequestNextCode = await repositoryManager.RequestRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            var getRequestVacationNextCode = await repositoryManager.RequestRepository
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
            request.RequestVacation.Code = getRequestVacationNextCode;
            request.Status = RequestStatus.Pending;
            request.IsActive = true;
            request.RequestVacation.IsActive = true;

            repositoryManager.RequestRepository.Insert(request);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return request.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateRequestVacationDTO model)
        {
            #region Model Validation

            var validator = new UpdateRequestVacationModelDTOValidator();
            var validatorResult = validator.Validate(model);
            if (!validatorResult.IsValid)
            {
                var error = validatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            var employeeId = await requestVacationBLValidation.UpdateValidation(model);

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
                        var result = await uploadBLC.UploadFile(attachment, LeillaKeys.VacationRequests)
                            ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadRequestAttachements); ;
                        newFileNames.Add(result.FileName);
                    }
                }
            }

            #endregion

            #region Update Request Vacation

            var getRequest = await repositoryManager.RequestRepository
                 .GetEntityByConditionWithTrackingAsync(request => !request.IsDeleted
                 && request.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            var getRequestVacation = await repositoryManager.RequestVacationRepository
                 .GetEntityByConditionWithTrackingAsync(requestVacation => !requestVacation.Request.IsDeleted
                 && requestVacation.Request.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            getRequest.EmployeeId = model.EmployeeId ?? 0;
            getRequest.ForEmployee = model.ForEmployee;
            getRequest.IsNecessary = model.IsNecessary;
            getRequest.Date = model.DateFrom;
            getRequest.ModifiedDate = DateTime.Now;
            getRequest.ModifyUserId = requestInfo.UserId;
            

            getRequestVacation.ModifiedDate = DateTime.Now;
            getRequestVacation.ModifyUserId = requestInfo.UserId;
            getRequestVacation.DateTo = model.DateTo;
            getRequestVacation.NumberOfDays = (model.DateTo - model.DateFrom).Days + 1;
            

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
        public async Task<GetRequestVacationsResponseDTO> Get(GetRequestVacationsCriteria criteria)
        {
            var requestVacationRepository = repositoryManager.RequestVacationRepository;
            var query = requestVacationRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = requestVacationRepository.OrderBy(query, nameof(RequestVacation.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestVacationsList = await queryPaged.Select(requestVacation => new GetRequestVacationsResponseModelDTO
            {
                Id = requestVacation.Request.Id,
                Code = requestVacation.Request.Code,
                Employee = new RequestEmployeeModel
                {
                    Code = requestVacation.Request.Employee.Code,
                    Name = requestVacation.Request.Employee.Name,
                    ProfileImagePath = uploadBLC.GetFilePath(requestVacation.Request.Employee.ProfileImageName, LeillaKeys.Employees)
                },
                VacationTypeName = requestVacation.VacationType.Name,
                DateFrom = requestVacation.Request.Date,
                DateTo = requestVacation.DateTo,
                Status = requestVacation.Request.Status,
                StatusName = TranslationHelper.GetTranslation(requestVacation.Request.Status.ToString(), requestInfo.Lang)

            }).ToListAsync();

            return new GetRequestVacationsResponseDTO
            {
                VacationRequests = requestVacationsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<EmployeeGetRequestVacationsResponseDTO> EmployeeGet(EmployeeGetRequestVacationsCriteria criteria)
        {
            #region Is Employee Validation

            await requestBLValidation.IsEmployeeValidation();

            #endregion

            var requestVacationRepository = repositoryManager.RequestVacationRepository;
            var query = requestVacationRepository.EmployeeGetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = requestVacationRepository.OrderBy(query, nameof(RequestVacation.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestVacationsList = await queryPaged.Select(requestVacation => new EmployeeGetRequestVacationsResponseModelDTO
            {
                Id = requestVacation.Request.Id,
                Code = requestVacation.Request.Code,
                AddedDate = requestVacation.Request.AddedDate,
                DirectManagerName = requestVacation.Request.Employee.DirectManager != null ?
                requestVacation.Request.Employee.DirectManager.Name : null,
                VacationTypeName = requestVacation.VacationType.Name,
                DateFrom = requestVacation.Request.Date,
                DateTo = requestVacation.DateTo,
                Status = requestVacation.Request.Status,
                StatusName = TranslationHelper.GetTranslation(requestVacation.Request.Status.ToString(), requestInfo.Lang),
                NumberOfDays = requestVacation.NumberOfDays
            }).ToListAsync();

            return new EmployeeGetRequestVacationsResponseDTO
            {
                VacationRequests = requestVacationsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetRequestVacationsForDropDownResponseDTO> GetForDropDown(GetRequestVacationsCriteria criteria)
        {
            criteria.IsActive = true;
            var requestVacationRepository = repositoryManager.RequestVacationRepository;
            var query = requestVacationRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = requestVacationRepository.OrderBy(query, nameof(RequestVacation.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestVacationsList = await queryPaged.Select(e => new GetRequestVacationsForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Request.Employee.Name
            }).ToListAsync();

            return new GetRequestVacationsForDropDownResponseDTO
            {
                VacationRequests = requestVacationsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetRequestVacationInfoResponseDTO> GetInfo(int requestId)
        {
            var requestVacation = await repositoryManager.RequestVacationRepository.Get(e => e.Request.Id == requestId && !e.Request.IsDeleted)
                .Select(requestVacation => new GetRequestVacationInfoResponseDTO
                {
                    Code = requestVacation.Request.Code,
                    Employee = new RequestEmployeeModel
                    {
                        Code = requestVacation.Request.Employee.Code,
                        Name = requestVacation.Request.Employee.Name,
                        ProfileImagePath = uploadBLC.GetFilePath(requestVacation.Request.Employee.ProfileImageName, LeillaKeys.Employees)
                    },
                    VacationTypeName = requestVacation.VacationType.Name,
                    DateFrom = requestVacation.Request.Date,
                    DateTo = requestVacation.DateTo,
                    IsActive = requestVacation.Request.IsActive,
                    IsNecessary = requestVacation.Request.IsNecessary,
                    ForEmployee = requestVacation.Request.ForEmployee,
                    Attachments = requestVacation.Request.RequestAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, LeillaKeys.VacationRequests),
                    }).ToList(),
                    Status = requestVacation.Request.Status,
                    NumberOfDays = requestVacation.NumberOfDays,
                    BalanceBeforeRequest = requestVacation.BalanceBeforeRequest,
                    BalanceAfterRequest = requestVacation.BalanceAfterRequest,
                    StatusName = TranslationHelper.GetTranslation(requestVacation.Request.Status.ToString(), requestInfo.Lang)
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            return requestVacation;
        }
        public async Task<GetRequestVacationByIdResponseDTO> GetById(int RequestVacationId)
        {
            var requestVacation = await repositoryManager.RequestVacationRepository.Get(e => e.Request.Id == RequestVacationId && !e.IsDeleted)
                .Select(requestVacation => new GetRequestVacationByIdResponseDTO
                {
                    Id = requestVacation.Request.Id,
                    Code = requestVacation.Request.Code,
                    EmployeeId = requestVacation.Request.EmployeeId,
                    VacationTypeId = requestVacation.VacationTypeId,
                    DateFrom = requestVacation.Request.Date,
                    DateTo = requestVacation.DateTo,
                    IsActive = requestVacation.Request.IsActive,
                    IsNecessary = requestVacation.Request.IsNecessary,
                    ForEmployee = requestVacation.Request.ForEmployee,
                    Attachments = requestVacation.Request.RequestAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, LeillaKeys.VacationRequests)
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            return requestVacation;

        }
        public async Task<bool> Delete(int requestId)
        {
            var requestVacation = await repositoryManager.RequestRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == requestId) ??
                throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);
            requestVacation.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Accept(int requestId)
        {
            var request = await repositoryManager.RequestRepository
                .GetWithTracking(d => !d.IsDeleted && d.Id == requestId)
                .Include(r=>r.RequestVacation).FirstOrDefaultAsync() ??
               throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            #region Validation

            if (request.Status == RequestStatus.Accepted)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestAlreadyAccepted);
            }
            else if (request.Status == RequestStatus.Rejected)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestAlreadyRejected);
            }

            #region Validate And Set Balance

            var getVacationsType = await repositoryManager.VacationsTypeRepository.GetEntityByConditionAsync(c => !c.IsDeleted &&
                c.CompanyId == requestInfo.CompanyId && c.Id == request.RequestVacation.VacationTypeId) ?? throw new BusinessValidationException(LeillaKeys.SorryVacationTypeNotFound);

            var type = getVacationsType.Type;

            var currentYear = DateTime.UtcNow.Year;

            var checkTypeBalance = await repositoryManager.VacationBalanceRepository.GetEntityByConditionWithTrackingAsync(c => !c.IsDeleted &&
                c.CompanyId == requestInfo.CompanyId && c.EmployeeId == request.EmployeeId
                && c.VacationType == type && c.Year == currentYear) ?? throw new BusinessValidationException(LeillaKeys.SorryThereIsNoVacationBalanceOfSelectedVacationTypeForEmployee);

            var requiredDays = request.RequestVacation.NumberOfDays;
            if (requiredDays > checkTypeBalance.RemainingBalance)
            {
                throw new BusinessValidationException(null,
                    LeillaKeys.SorryThereIsNoSufficientBalanceForSelectedTypeForEmployee +
                    LeillaKeys.Space +
                    LeillaKeys.CurrentBalanceForEmployee +
                    checkTypeBalance.RemainingBalance +
                    LeillaKeys.LeftBracket + TranslationHelper.GetTranslation(checkTypeBalance.VacationType.ToString(), requestInfo.Lang) +
                    LeillaKeys.RightBracket);
            }

            checkTypeBalance.RemainingBalance -= requiredDays;

            #endregion

            #endregion

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
        public async Task<GetVacationsInformationsResponseDTO> GetVacationsInformations()
        {
            var requestVacationRepository = repositoryManager.RequestVacationRepository;
            var query = requestVacationRepository.Get(request => !request.Request.IsDeleted &&
            request.Request.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetVacationsInformationsResponseDTO
            {
                TotalVacationsCount = await query.CountAsync(),
                AcceptedCount = await query.Where(request => request.Request.Status == RequestStatus.Accepted).CountAsync(),
                RejectedCount = await query.Where(request => request.Request.Status == RequestStatus.Rejected).CountAsync(),
                PendingCount = await query.Where(request => request.Request.Status == RequestStatus.Pending).CountAsync()
            };

            #endregion
        }
        public async Task<EmployeeGetVacationsInformationsResponseDTO> EmployeeGetVacationsInformations()
        {
            #region Is Employee Validation

            await requestBLValidation.IsEmployeeValidation();

            #endregion

            var currentYear = DateTime.UtcNow.Year;

            var requestVacationRepository = repositoryManager.RequestVacationRepository;
            var query = requestVacationRepository.Get(request => !request.Request.IsDeleted &&
            request.Request.Date.Year == currentYear &&
            request.Request.CompanyId == requestInfo.CompanyId &&
            request.Request.EmployeeId == requestInfo.EmployeeId);

            #region Handle Response

            return new EmployeeGetVacationsInformationsResponseDTO
            {
                VacationsBalance = await query.AnyAsync(q => q.Request.Employee.VacationBalances != null && q.Request.Employee.VacationBalances.Where(b => b.Year == currentYear).Any()) ?  
                await query.Select(q=> q.Request.Employee.VacationBalances.Where(b=>b.Year == currentYear).Sum(b => b.Balance)).FirstOrDefaultAsync() : 0,
                AcceptedCount = await query.Where(request => request.Request.Status == RequestStatus.Accepted).CountAsync(),
                RejectedCount = await query.Where(request => request.Request.Status == RequestStatus.Rejected).CountAsync(),
                PendingCount = await query.Where(request => request.Request.Status == RequestStatus.Pending).CountAsync()
            };

            #endregion
        }
    }
}

