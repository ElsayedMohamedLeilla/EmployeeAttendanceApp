using AutoMapper;
using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.TaskType;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees.TaskTypes;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Employees;
using Dawem.Validation.FluentValidation.Employees.TaskTypes;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Employees
{
    public class TaskTypeBL : ITaskTypeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly ITaskTypeBLValidation departmentBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public TaskTypeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           ITaskTypeBLValidation _departmentBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            departmentBLValidation = _departmentBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateTaskTypeModel model)
        {
            #region Model Validation

            var createTaskTypeModel = new CreateTaskTypeModelValidator();
            var createTaskTypeModelResult = createTaskTypeModel.Validate(model);
            if (!createTaskTypeModelResult.IsValid)
            {
                var error = createTaskTypeModelResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            await departmentBLValidation.CreateValidation(model);

            #endregion
            unitOfWork.CreateTransaction();
            #region Insert TaskType

            #region Set TaskType code
            var getNextCode = await repositoryManager.TaskTypeRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var department = mapper.Map<TaskType>(model);
            department.CompanyId = requestInfo.CompanyId;
            department.AddUserId = requestInfo.UserId;

            department.Code = getNextCode;
            repositoryManager.TaskTypeRepository.Insert(department);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return department.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateTaskTypeModel model)
        {
            #region Model Validation

            var updateTaskTypeModelValidator = new UpdateTaskTypeModelValidator();
            var updateTaskTypeModelValidatorResult = updateTaskTypeModelValidator.Validate(model);
            if (!updateTaskTypeModelValidatorResult.IsValid)
            {
                var error = updateTaskTypeModelValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation
            await departmentBLValidation.UpdateValidation(model);
            #endregion

            unitOfWork.CreateTransaction();

            #region Update TaskType

            var getTaskType = await repositoryManager.TaskTypeRepository
                 .GetEntityByConditionWithTrackingAsync(taskType => !taskType.IsDeleted
                 && taskType.Id == model.Id);

            if (getTaskType != null)
            {
                getTaskType.Name = model.Name;
                getTaskType.IsActive = model.IsActive;
                getTaskType.ModifiedDate = DateTime.Now;
                getTaskType.ModifyUserId = requestInfo.UserId;
                await unitOfWork.SaveAsync();
                #region Handle Response
                await unitOfWork.CommitAsync();
                return true;
                #endregion
            }
            #endregion

            else
                throw new BusinessValidationException(DawemKeys.SorryTaskTypeNotFound);


        }
        public async Task<GetTaskTypesResponse> Get(GetTaskTypesCriteria criteria)
        {
            #region Model Validation

            var getValidator = new GetGenaricValidator();
            var getValidatorResult = getValidator.Validate(criteria);
            if (!getValidatorResult.IsValid)
            {
                var error = getValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion
            var departmentRepository = repositoryManager.TaskTypeRepository;
            var query = departmentRepository.GetAsQueryable(criteria);
            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = departmentRepository.OrderBy(query, nameof(TaskType.Id), DawemKeys.Desc);
            #endregion
            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var departmentsList = await queryPaged.Select(e => new GetTaskTypesResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
            }).ToListAsync();
            return new GetTaskTypesResponse
            {
                TaskTypes = departmentsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetTaskTypesForDropDownResponse> GetForDropDown(GetTaskTypesCriteria criteria)
        {
            #region Model Validation
            var getValidator = new GetGenaricValidator();
            var getValidatorResult = getValidator.Validate(criteria);
            if (!getValidatorResult.IsValid)
            {
                var error = getValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }
            #endregion

            criteria.IsActive = true;
            var departmentRepository = repositoryManager.TaskTypeRepository;
            var query = departmentRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = departmentRepository.OrderBy(query, nameof(TaskType.Id), DawemKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var departmentsList = await queryPaged.Select(e => new GetTaskTypesForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetTaskTypesForDropDownResponse
            {
                TaskTypes = departmentsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetTaskTypeInfoResponseModel> GetInfo(int TaskTypeId)
        {
            var department = await repositoryManager.TaskTypeRepository.Get(e => e.Id == TaskTypeId && !e.IsDeleted)
                .Select(e => new GetTaskTypeInfoResponseModel
                {
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(DawemKeys.SorryTaskTypeNotFound);

            return department;
        }
        public async Task<GetTaskTypeByIdResponseModel> GetById(int TaskTypeId)
        {
            var department = await repositoryManager.TaskTypeRepository.Get(e => e.Id == TaskTypeId && !e.IsDeleted)
                .Select(e => new GetTaskTypeByIdResponseModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(DawemKeys.SorryTaskTypeNotFound);

            return department;

        }
        public async Task<bool> Delete(int departmentd)
        {
            var department = await repositoryManager.TaskTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == departmentd) ??
                throw new BusinessValidationException(DawemKeys.SorryTaskTypeNotFound);
            department.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}

