using AutoMapper;
using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Provider;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Employees;
using Dawem.Validation.FluentValidation.Employees.Department;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Provider
{
    public class DepartmentBL : IDepartmentBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IDepartmentBLValidation departmentBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public DepartmentBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IDepartmentBLValidation _departmentBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            departmentBLValidation = _departmentBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateDepartmentModel model)
        {
            #region Model Validation

            var createDepartmentModel = new CreateDepartmentModelValidator();
            var createDepartmentModelResult = createDepartmentModel.Validate(model);
            if (!createDepartmentModelResult.IsValid)
            {
                var error = createDepartmentModelResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            await departmentBLValidation.CreateValidation(model);

            #endregion
            unitOfWork.CreateTransaction();
            #region Insert Department

            #region Set Department code
            var getNextCode = await repositoryManager.DepartmentRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var department = mapper.Map<Department>(model);
            department.CompanyId = requestInfo.CompanyId;
            department.AddUserId = requestInfo.UserId;

            department.Code = getNextCode;
            repositoryManager.DepartmentRepository.Insert(department);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return department.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateDepartmentModel model)
        {
            #region Model Validation

            var updateDepartmentModelValidator = new UpdateDepartmentModelValidator();
            var updateDepartmentModelValidatorResult = updateDepartmentModelValidator.Validate(model);
            if (!updateDepartmentModelValidatorResult.IsValid)
            {
                var error = updateDepartmentModelValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation
            await departmentBLValidation.UpdateValidation(model);
            #endregion

            unitOfWork.CreateTransaction();

            #region Update Department

            var getDepartment = await repositoryManager.DepartmentRepository.GetByIdAsync(model.Id);

            if (getDepartment != null)
            {
                getDepartment.Name = model.Name;
                getDepartment.IsActive = model.IsActive;
                getDepartment.ModifiedDate = DateTime.Now;
                getDepartment.ModifyUserId = requestInfo.UserId;
                await unitOfWork.SaveAsync();
                #region Handle Response
                await unitOfWork.CommitAsync();
                return true;
                #endregion
            }
            #endregion

            else
                throw new BusinessValidationException(DawemKeys.SorryDepartmentNotFound);


        }
        public async Task<GetDepartmentsResponse> Get(GetDepartmentsCriteria criteria)
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
            var departmentRepository = repositoryManager.DepartmentRepository;
            var query = departmentRepository.GetAsQueryable(criteria);
            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = departmentRepository.OrderBy(query, nameof(Department.Id), DawemKeys.Desc);
            #endregion
            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var departmentsList = await queryPaged.Select(e => new GetDepartmentsResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
            }).ToListAsync();
            return new GetDepartmentsResponse
            {
                Departments = departmentsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetDepartmentsForDropDownResponse> GetForDropDown(GetDepartmentsCriteria criteria)
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
            var departmentRepository = repositoryManager.DepartmentRepository;
            var query = departmentRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = departmentRepository.OrderBy(query, nameof(Department.Id), DawemKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var departmentsList = await queryPaged.Select(e => new GetDepartmentsForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetDepartmentsForDropDownResponse
            {
                Departments = departmentsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetDepartmentInfoResponseModel> GetInfo(int DepartmentId)
        {
            var department = await repositoryManager.DepartmentRepository.Get(e => e.Id == DepartmentId && !e.IsDeleted)
                .Select(e => new GetDepartmentInfoResponseModel
                {
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(DawemKeys.SorryDepartmentNotFound);

            return department;
        }
        public async Task<GetDepartmentByIdResponseModel> GetById(int DepartmentId)
        {
            var department = await repositoryManager.DepartmentRepository.Get(e => e.Id == DepartmentId && !e.IsDeleted)
                .Select(e => new GetDepartmentByIdResponseModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(DawemKeys.SorryDepartmentNotFound);

            return department;

        }
        public async Task<bool> Delete(int departmentd)
        {
            var department = await repositoryManager.DepartmentRepository.GetEntityByConditionAsync(d => !d.IsDeleted && d.Id == departmentd) ??
                throw new BusinessValidationException(DawemKeys.SorryDepartmentNotFound);
            department.IsDeleted = true;
            department.DeletionDate = DateTime.Now;
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}

