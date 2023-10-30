using AutoMapper;
using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Contract.BusinessLogicCore;
using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees.Employee;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Employees;
using Dawem.Validation.FluentValidation.Employees.Employees;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Employees
{
    public class EmployeeBL : IEmployeeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IEmployeeBLValidation employeeBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;
        public EmployeeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
            IUploadBLC _uploadBLC,
           RequestInfo _requestHeaderContext,
           IEmployeeBLValidation _employeeBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            employeeBLValidation = _employeeBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;
        }
        public async Task<int> Create(CreateEmployeeModel model)
        {
            #region Model Validation

            var createEmployeeModel = new CreateEmployeeModelValidator();
            var createEmployeeModelResult = createEmployeeModel.Validate(model);
            if (!createEmployeeModelResult.IsValid)
            {
                var error = createEmployeeModelResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            await employeeBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Upload Profile Image

            string imageName = null;
            if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
            {
                var result = await uploadBLC.UploadImageFile(model.ProfileImageFile, DawemKeys.Employees)
                    ?? throw new BusinessValidationException(DawemKeys.SorryErrorHappenWhileUploadProfileImage); ;
                imageName = result.FileName;
            }

            #endregion

            #region Insert Employee

            #region Set Employee code

            var getNextCode = await repositoryManager.EmployeeRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var employee = mapper.Map<Employee>(model);
            employee.CompanyId = requestInfo.CompanyId;
            employee.AddUserId = requestInfo.UserId;
            employee.ProfileImageName = imageName;
            employee.Code = getNextCode;
            repositoryManager.EmployeeRepository.Insert(employee);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return employee.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateEmployeeModel model)
        {
            #region Model Validation

            var updateEmployeeModelValidator = new UpdateEmployeeModelValidator();
            var updateEmployeeModelValidatorResult = updateEmployeeModelValidator.Validate(model);
            if (!updateEmployeeModelValidatorResult.IsValid)
            {
                var error = updateEmployeeModelValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            await employeeBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Upload Profile Image

            string imageName = null;
            if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
            {
                var result = await uploadBLC.UploadImageFile(model.ProfileImageFile, DawemKeys.Employees)
                    ?? throw new BusinessValidationException(DawemKeys.SorryErrorHappenWhileUploadProfileImage);
                imageName = result.FileName;
            }

            #endregion

            #region Update Employee

            var getEmployee = await repositoryManager.EmployeeRepository.GetEntityByConditionWithTrackingAsync(employee => !employee.IsDeleted
            && employee.Id == model.Id);

            getEmployee.Name = model.Name;
            getEmployee.DepartmentId = model.DepartmentId;
            getEmployee.IsActive = model.IsActive;
            getEmployee.JoiningDate = model.JoiningDate;
            getEmployee.ModifiedDate = DateTime.Now;
            getEmployee.ProfileImageName = !string.IsNullOrEmpty(imageName) ? imageName : !string.IsNullOrEmpty(model.ProfileImageName)
                ? getEmployee.ProfileImageName : null;
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetEmployeesResponse> Get(GetEmployeesCriteria criteria)
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

            var employeeRepository = repositoryManager.EmployeeRepository;
            var query = employeeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = employeeRepository.OrderBy(query, nameof(Employee.Id), DawemKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var employeesList = await queryPaged.Select(e => new GetEmployeesResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DapartmentName = e.Department.Name,
                IsActive = e.IsActive,
                JoiningDate = e.JoiningDate,
                ProfileImagePath = e.ProfileImageName
            }).ToListAsync();

            return new GetEmployeesResponse
            {
                Employees = employeesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetEmployeesForDropDownResponse> GetForDropDown(GetEmployeesCriteria criteria)
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
            var employeeRepository = repositoryManager.EmployeeRepository;
            var query = employeeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = employeeRepository.OrderBy(query, nameof(Employee.Id), DawemKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var employeesList = await queryPaged.Select(e => new GetEmployeesForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetEmployeesForDropDownResponse
            {
                Employees = employeesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetEmployeeInfoResponseModel> GetInfo(int employeeId)
        {
            var employee = await repositoryManager.EmployeeRepository.Get(e => e.Id == employeeId && !e.IsDeleted)
                .Select(e => new GetEmployeeInfoResponseModel
                {
                    Code = e.Code,
                    Name = e.Name,
                    DapartmentName = e.Department.Name,
                    IsActive = e.IsActive,
                    JoiningDate = e.JoiningDate,
                    ProfileImagePath = uploadBLC.GetFilePath(e.ProfileImageName, DawemKeys.Employees)
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(DawemKeys.SorryEmployeeNotFound);

            return employee;
        }
        public async Task<GetEmployeeByIdResponseModel> GetById(int employeeId)
        {
            var employee = await repositoryManager.EmployeeRepository.Get(e => e.Id == employeeId && !e.IsDeleted)
                .Select(e => new GetEmployeeByIdResponseModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    DepartmentId = e.DepartmentId,
                    IsActive = e.IsActive,
                    JoiningDate = e.JoiningDate,
                    ProfileImageName = e.ProfileImageName,
                    ProfileImagePath = uploadBLC.GetFilePath(e.ProfileImageName, DawemKeys.Employees)
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(DawemKeys.SorryEmployeeNotFound);

            return employee;

        }
        public async Task<bool> Delete(int employeeId)
        {
            var employee = await repositoryManager.EmployeeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == employeeId) ??
                throw new BusinessValidationException(DawemKeys.SorryEmployeeNotFound);

            employee.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}

