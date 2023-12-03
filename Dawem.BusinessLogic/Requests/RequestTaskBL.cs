using AutoMapper;
using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Contract.BusinessLogicCore;
using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Employees.JobTitle;
using Dawem.Models.Dtos.Others;
using Dawem.Models.Dtos.Requests.Task;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees.TaskTypes;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Requests.Task;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Employees
{
    public class RequestTaskBL : IRequestTaskBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRequestTaskBLValidation requestTaskBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;
        public RequestTaskBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
            IUploadBLC _uploadBLC,
           RequestInfo _requestHeaderContext,
           IRequestTaskBLValidation _requestTaskBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            requestTaskBLValidation = _requestTaskBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;
        }
        public async Task<int> Create(CreateRequestTaskModelDTO model)
        {
            #region Model Validation

            var validator = new CreateRequestTaskModelDTOValidator();
            var validatorResult = validator.Validate(model);
            if (!validatorResult.IsValid)
            {
                var error = validatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            var employeeId = await requestTaskBLValidation.CreateValidation(model);

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
                        var result = await uploadBLC.UploadFile(attachment, LeillaKeys.TaskRequests)
                            ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadRequestAttachements); ;
                        fileNames.Add(result.FileName);
                    }
                }
                model.AttachmentsNames = fileNames;
            }

            #endregion

            #region Insert Request Task

            #region Set Request Task Code

            var getNextCode = await repositoryManager.RequestRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var requestTask = mapper.Map<Request>(model);
            requestTask.CompanyId = requestInfo.CompanyId;
            requestTask.AddUserId = requestInfo.UserId;
            requestTask.EmployeeId = employeeId ?? 0;
            requestTask.Code = getNextCode;
            requestTask.Status = RequestStatus.Pending;
            requestTask.IsActive = true;
            requestTask.RequestTask.IsActive = true;

            repositoryManager.RequestRepository.Insert(requestTask);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return requestTask.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateRequestTaskModelDTO model)
        {
            #region Model Validation

            var validator = new UpdateRequestTaskModelDTOValidator();
            var validatorResult = validator.Validate(model);
            if (!validatorResult.IsValid)
            {
                var error = validatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            var employeeId = await requestTaskBLValidation.UpdateValidation(model);

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
                        var result = await uploadBLC.UploadFile(attachment, LeillaKeys.TaskRequests)
                            ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadRequestAttachements); ;
                        newFileNames.Add(result.FileName);
                    }
                }
            }

            #endregion


            #region Update Request Task

            var getRequest = await repositoryManager.RequestRepository
                 .GetEntityByConditionWithTrackingAsync(request => !request.IsDeleted
                 && request.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            var getRequestTask = await repositoryManager.RequestTaskRepository
                 .GetEntityByConditionWithTrackingAsync(requestTask => !requestTask.Request.IsDeleted
                 && requestTask.Request.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            getRequest.EmployeeId = model.EmployeeId ?? 0;
            getRequest.ForEmployee = model.ForEmployee;
            getRequest.IsNecessary = model.IsNecessary;
            getRequest.Date = model.DateFrom;
            getRequest.ModifiedDate = DateTime.Now;
            getRequest.ModifyUserId = requestInfo.UserId;


            getRequestTask.ModifiedDate = DateTime.Now;
            getRequestTask.ModifyUserId = requestInfo.UserId;
            getRequestTask.DateTo = model.DateTo;

            await unitOfWork.SaveAsync();

            #region Update Task Employees 

            var existDbList = await repositoryManager.RequestTaskEmployeeRepository
                    .Get(e => e.RequestTaskId == getRequestTask.Id)
                    .ToListAsync();

            var existingEmployeeIds = existDbList.Select(e => e.EmployeeId).ToList();

            var addedTaskEmployees = model.TaskEmployeeIds
                .Where(employeeId => !existingEmployeeIds.Contains(employeeId))
                .Select(employeeId => new RequestTaskEmployee
                {
                    RequestTaskId = getRequestTask.Id,
                    EmployeeId = employeeId,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList();

            var removedTaskEmployeeIds = existDbList
                .Where(ge => !model.TaskEmployeeIds.Contains(ge.EmployeeId))
                .Select(ge => ge.EmployeeId)
                .ToList();

            var removedTaskEmployees = await repositoryManager.RequestTaskEmployeeRepository
                .Get(e => e.EmployeeId == model.Id && removedTaskEmployeeIds.Contains(e.EmployeeId))
                .ToListAsync();

            if (removedTaskEmployees.Count > 0)
                repositoryManager.RequestTaskEmployeeRepository.BulkDeleteIfExist(removedTaskEmployees);
            if (addedTaskEmployees.Count > 0)
                repositoryManager.RequestTaskEmployeeRepository.BulkInsert(addedTaskEmployees);

            await unitOfWork.SaveAsync();

            #endregion

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
                .Where(ge => !model.AttachmentsNames.Contains(ge.FileName))
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
        public async Task<GetRequestTasksResponse> Get(GetRequestTasksCriteria criteria)
        {
            var requestTaskRepository = repositoryManager.RequestTaskRepository;
            var query = requestTaskRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = requestTaskRepository.OrderBy(query, nameof(RequestTask.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestTasksList = await queryPaged.Select(requestTask => new GetRequestTasksResponseModel
            {
                Id = requestTask.Request.Id,
                Code = requestTask.Request.Code,
                EmployeeName = requestTask.Request.Employee.Name,
                TaskTypeName = requestTask.TaskType.Name,
                DateFrom = requestTask.Request.Date,
                DateTo = requestTask.DateTo,
                Status = requestTask.Request.Status,
                StatusName = TranslationHelper.GetTranslation(requestTask.Request.Status.ToString(), requestInfo.Lang)

            }).ToListAsync();

            return new GetRequestTasksResponse
            {
                TaskRequests = requestTasksList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetRequestTasksForDropDownResponse> GetForDropDown(GetRequestTasksCriteria criteria)
        {
            criteria.IsActive = true;
            var requestTaskRepository = repositoryManager.RequestTaskRepository;
            var query = requestTaskRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = requestTaskRepository.OrderBy(query, nameof(RequestTask.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestTasksList = await queryPaged.Select(e => new GetRequestTasksForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Request.Employee.Name
            }).ToListAsync();

            return new GetRequestTasksForDropDownResponse
            {
                TaskRequests = requestTasksList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetRequestTaskInfoResponseModel> GetInfo(int requestId)
        {
            var requestTask = await repositoryManager.RequestTaskRepository.Get(e => e.Request.Id == requestId && !e.Request.IsDeleted)
                .Select(requestTask => new GetRequestTaskInfoResponseModel
                {
                    Code = requestTask.Request.Code,
                    EmployeeName = requestTask.Request.Employee.Name,
                    TaskTypeName = requestTask.TaskType.Name,
                    DateFrom = requestTask.Request.Date,
                    DateTo = requestTask.DateTo,
                    IsActive = requestTask.Request.IsActive,
                    IsNecessary = requestTask.Request.IsNecessary,
                    ForEmployee = requestTask.Request.ForEmployee,
                    TaskEmployees = requestTask.TaskEmployees.Select(te => te.Employee.Name).ToList(),
                    Attachments = requestTask.Request.RequestAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, LeillaKeys.TaskRequests),
                    }).ToList(),
                    Status = requestTask.Request.Status,
                    StatusName = TranslationHelper.GetTranslation(requestTask.Request.Status.ToString(), requestInfo.Lang)
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            return requestTask;
        }
        public async Task<GetRequestTaskByIdResponseModel> GetById(int RequestTaskId)
        {
            var requestTask = await repositoryManager.RequestTaskRepository.Get(e => e.Id == RequestTaskId && !e.IsDeleted)
                .Select(requestTask => new GetRequestTaskByIdResponseModel
                {
                    Id = requestTask.Request.Id,
                    Code = requestTask.Request.Code,
                    EmployeeId = requestTask.Request.EmployeeId,
                    TaskTypeId = requestTask.TaskTypeId,
                    DateFrom = requestTask.Request.Date,
                    DateTo = requestTask.DateTo,
                    IsActive = requestTask.Request.IsActive,
                    IsNecessary = requestTask.Request.IsNecessary,
                    ForEmployee = requestTask.Request.ForEmployee,
                    TaskEmployeeIds = requestTask.TaskEmployees.Select(te => te.EmployeeId).ToList(),
                    Attachments = requestTask.Request.RequestAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, LeillaKeys.TaskRequests),
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            return requestTask;

        }
        public async Task<bool> Delete(int requestId)
        {
            var requestTask = await repositoryManager.RequestRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == requestId) ??
                throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);
            requestTask.Delete();
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

        public async Task<bool> Refuse(RefuseModelDTO refuseModelDTO)
        {
            var request = await repositoryManager.RequestRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == refuseModelDTO.Id) ??
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

            await unitOfWork.SaveAsync();
            return true;
        }
    }
}

