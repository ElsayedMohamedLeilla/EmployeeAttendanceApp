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

            #region Assign Delegatos In DepartmentZones Object

            if (model.ZoneIds != null && model.ZoneIds.Count > 0)
                model.MapEmployeeZones();

            #endregion

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
                var result = await uploadBLC.UploadFile(model.ProfileImageFile, LeillaKeys.Employees)
                    ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadProfileImage); ;
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
            employee.AddedApplicationType = requestInfo.ApplicationType;
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

            #region assign Delegatos In DepartmentZones Object
            model.MapEmployeeZones();
            #endregion

            #region Upload Profile Image

            string imageName = null;
            if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
            {
                var result = await uploadBLC.UploadFile(model.ProfileImageFile, LeillaKeys.Employees)
                    ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadProfileImage);
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
            getEmployee.ModifyUserId = requestInfo.UserId;
            getEmployee.AttendanceType = model.AttendanceType;
            getEmployee.EmployeeType = model.EmployeeType;
            getEmployee.JobTitleId = model.JobTitleId;
            getEmployee.DirectManagerId = model.DirectManagerId;
            getEmployee.ScheduleId = model.ScheduleId;
            getEmployee.EmployeeNumber = model.EmployeeNumber;
            getEmployee.AnnualVacationBalance = model.AnnualVacationBalance;
            getEmployee.ProfileImageName = !string.IsNullOrEmpty(imageName) ? imageName : !string.IsNullOrEmpty(model.ProfileImageName)
                ? getEmployee.ProfileImageName : null;
            getEmployee.ModifiedApplicationType = requestInfo.ApplicationType;

            #region Update ZoneEmployee

            List<ZoneEmployee> existDbList = repositoryManager.ZoneEmployeeRepository
                    .GetByCondition(e => e.EmployeeId == getEmployee.Id)
                    .ToList();

            List<int> existingZoneIds = existDbList.Select(e => e.ZoneId).ToList();

            List<ZoneEmployee> addedEmployeeZones = model.Zones
                .Where(ge => !existingZoneIds.Contains(ge.ZoneId))
                .Select(ge => new ZoneEmployee
                {
                    EmployeeId = model.Id,
                    ZoneId = ge.ZoneId,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                })
                .ToList();

            List<int> ZonesToRemove = existDbList
                .Where(ge => !model.ZoneIds.Contains(ge.ZoneId))
                .Select(ge => ge.ZoneId)
                .ToList();

            List<ZoneEmployee> removedEmployeeZones = repositoryManager.ZoneEmployeeRepository
                .GetByCondition(e => e.EmployeeId == model.Id && ZonesToRemove.Contains(e.ZoneId))
                .ToList();

            if (removedEmployeeZones.Count > 0)
                repositoryManager.ZoneEmployeeRepository.BulkDeleteIfExist(removedEmployeeZones);
            if (addedEmployeeZones.Count > 0)
                repositoryManager.ZoneEmployeeRepository.BulkInsert(addedEmployeeZones);

            #endregion

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetEmployeesResponse> Get(GetEmployeesCriteria criteria)
        {
            var employeeRepository = repositoryManager.EmployeeRepository;
            var query = employeeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = employeeRepository.OrderBy(query, nameof(Employee.Id), LeillaKeys.Desc);

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
                EmployeeNumber = e.EmployeeNumber,
                AnnualVacationBalance = e.AnnualVacationBalance,
                ProfileImagePath = uploadBLC.GetFilePath(e.ProfileImageName, LeillaKeys.Employees)
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
            criteria.IsActive = true;
            var employeeRepository = repositoryManager.EmployeeRepository;
            var query = employeeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = employeeRepository.OrderBy(query, nameof(Employee.Id), LeillaKeys.Desc);

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
                    DirectManagerName = e.DirectManager.Name,
                    Email = e.Email,
                    MobileNumber = e.MobileNumber,
                    Address = e.Address,
                    IsActive = e.IsActive,
                    JoiningDate = e.JoiningDate,
                    AnnualVacationBalance = e.AnnualVacationBalance,
                    JobTitleName = e.JobTitle.Name,
                    ScheduleName = e.Schedule.Name,
                    EmployeeNumber = e.EmployeeNumber,
                    AttendanceTypeName = TranslationHelper.GetTranslation(e.AttendanceType.ToString(), requestInfo.Lang),
                    EmployeeTypeName = TranslationHelper.GetTranslation(e.EmployeeType.ToString(), requestInfo.Lang),
                    ProfileImagePath = uploadBLC.GetFilePath(e.ProfileImageName, LeillaKeys.Employees),
                    DisableReason = e.DisableReason
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);

            return employee;
        }
        public async Task<GetCurrentEmployeeInfoResponseModel> GetCurrentEmployeeInfo()
        {
            var employeeId = await repositoryManager.UserRepository.Get(u => !u.IsDeleted && u.Id == requestInfo.UserId && u.EmployeeId != null)
                .Select(u => u.EmployeeId)
                .FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCurrentUserNotEmployee);

            var employee = await repositoryManager.EmployeeRepository.Get(e => e.Id == employeeId && !e.IsDeleted)
                .Select(e => new GetCurrentEmployeeInfoResponseModel
                {
                    Name = e.Name,
                    DapartmentName = e.Department.Name,
                    DirectManagerName = e.DirectManager.Name,
                    Email = e.Email,
                    MobileNumber = e.MobileNumber,
                    Address = e.Address,
                    JobTitleName = e.JobTitle.Name,
                    ProfileImagePath = uploadBLC.GetFilePath(e.ProfileImageName, LeillaKeys.Employees)
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);

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
                    DirectManagerId = e.DirectManagerId,
                    Email = e.Email,
                    MobileNumber = e.MobileNumber,
                    Address = e.Address,
                    IsActive = e.IsActive,
                    JoiningDate = e.JoiningDate,
                    AnnualVacationBalance = e.AnnualVacationBalance,
                    JobTitleId = e.JobTitleId,
                    ScheduleId = e.ScheduleId,
                    AttendanceType = e.AttendanceType,
                    EmployeeType = e.EmployeeType,
                    EmployeeNumber = e.EmployeeNumber,
                    ProfileImageName = e.ProfileImageName,
                    ProfileImagePath = uploadBLC.GetFilePath(e.ProfileImageName, LeillaKeys.Employees),
                    DisableReason = e.DisableReason,
                    ZoneIds = e.Zones
                    .Join(repositoryManager.ZoneRepository.GetAll(),
                    zoneDepartment => zoneDepartment.ZoneId,
                    zone => zone.Id,
                    (zoneDepartment, zone) => zone.Id)
                    .ToList(),
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);

            return employee;
        }
        public async Task<bool> Enable(int employeeId)
        {
            var employee = await repositoryManager.EmployeeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == employeeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);
            employee.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var employee = await repositoryManager.EmployeeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);
            employee.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Delete(int employeeId)
        {
            var employee = await repositoryManager.EmployeeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == employeeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);
            employee.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}

