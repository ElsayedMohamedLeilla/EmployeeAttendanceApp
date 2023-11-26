using AutoMapper;
using Dawem.Contract.BusinessLogic.Employees.Department;
using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Employees;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Department;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees.Departments;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Employees;
using Dawem.Validation.FluentValidation.Employees.Departments;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Employees.Departments
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
            #region assign Delegatos In DepartmentZones Object
            model.MapDepartmentZones();
            #endregion
            #region assign DelegatorsIdes In DepartmentManagerDelegators Object
            model.MapDepartmentManagarDelegators();
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
            department.AddedApplicationType = requestInfo.ApplicationType;
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
            #region assign Delegatos In DepartmentZones Object
            model.MapDepartmentZones();
            #endregion
            #region assign DelegatorsIdes In DepartmentManagerDelegators Object
            model.MapDepartmentManagarDelegators();
            #endregion
            #region Business Validation
            await departmentBLValidation.UpdateValidation(model);
            #endregion

            unitOfWork.CreateTransaction();

            #region Update Department

            var getDepartment = await repositoryManager.DepartmentRepository.GetEntityByConditionWithTrackingAsync(department => !department.IsDeleted
            && department.Id == model.Id);

            if (getDepartment != null)
            {
                getDepartment.Name = model.Name;
                getDepartment.ParentId = model.ParentId;
                getDepartment.IsActive = model.IsActive;
                getDepartment.ModifiedDate = DateTime.Now;
                getDepartment.ModifyUserId = requestInfo.UserId;
                getDepartment.ManagerId = model.ManagerId;
                await unitOfWork.SaveAsync();

                #region Update DepartmentZones
         
                    List<ZoneDepartment> existDbList = repositoryManager.DepartmentZoneRepository
                        .GetByCondition(e => e.DepartmentId == getDepartment.Id)
                        .ToList();

                    List<int> existingZoneIds = existDbList.Select(e => e.ZoneId).ToList();

                    List<ZoneDepartment> addedDepartmentZones = model.Zones
                        .Where(ge => !existingZoneIds.Contains(ge.ZoneId))
                        .Select(ge => new ZoneDepartment
                        {
                            DepartmentId = model.Id,
                            ZoneId = ge.ZoneId,
                            ModifyUserId = requestInfo.UserId,
                            ModifiedDate = DateTime.UtcNow
                        })
                        .ToList();

                    List<int> ZonesToRemove = existDbList
                        .Where(ge => !model.ZoneIds.Contains(ge.ZoneId))
                        .Select(ge => ge.ZoneId)
                        .ToList();

                    List<ZoneDepartment> removedDepartmentZones = repositoryManager.DepartmentZoneRepository
                        .GetByCondition(e => e.DepartmentId == model.Id && ZonesToRemove.Contains(e.ZoneId))
                        .ToList();

                    if (removedDepartmentZones.Count > 0)
                        repositoryManager.DepartmentZoneRepository.BulkDeleteIfExist(removedDepartmentZones);
                    if (addedDepartmentZones.Count > 0)
                        repositoryManager.DepartmentZoneRepository.BulkInsert(addedDepartmentZones);

                #endregion
                #region Update DepartmentManagerDelgators
               
                    List<DepartmentManagerDelegator> ExistDbList = repositoryManager.DepartmentManagerDelegatorRepository
                        .GetByCondition(e => e.DepartmentId == getDepartment.Id)
                        .ToList();

                    List<int> existingEmployeeIds = ExistDbList.Select(e => e.EmployeeId).ToList();

                    List<DepartmentManagerDelegator> addedDepartmentManagerDelegators = model.ManagerDelegators
                        .Where(gmd => !existingEmployeeIds.Contains(gmd.EmployeeId))
                        .Select(gmd => new DepartmentManagerDelegator
                        {
                            DepartmentId = model.Id,
                            EmployeeId = gmd.EmployeeId,
                            ModifyUserId = requestInfo.UserId,
                            ModifiedDate = DateTime.UtcNow
                        }).ToList();

                    List<int> DepartmentManagerDelegatorToRemove = ExistDbList
                        .Where(gmd => !model.ManagerDelegatorIds.Contains(gmd.EmployeeId))
                        .Select(gmd => gmd.EmployeeId)
                        .ToList();

                    List<DepartmentManagerDelegator> removedDepartmentManagerDelegators = repositoryManager.DepartmentManagerDelegatorRepository
                        .GetByCondition(e => e.DepartmentId == model.Id && DepartmentManagerDelegatorToRemove.Contains(e.EmployeeId))
                        .ToList();
                    if (removedDepartmentManagerDelegators.Count > 0)
                        repositoryManager.DepartmentManagerDelegatorRepository.BulkDeleteIfExist(removedDepartmentManagerDelegators);
                    if (addedDepartmentManagerDelegators.Count > 0)
                        repositoryManager.DepartmentManagerDelegatorRepository.BulkInsert(addedDepartmentManagerDelegators);
                
                #endregion
                #region Handle Response
                await unitOfWork.CommitAsync();
                return true;
                #endregion
            }
            #endregion

            else
                throw new BusinessValidationException(LeillaKeys.SorryDepartmentNotFound);


        }
        public async Task<GetDepartmentsResponse> Get(GetDepartmentsCriteria criteria)
        {
            var departmentRepository = repositoryManager.DepartmentRepository;
            var query = departmentRepository.GetAsQueryable(criteria);
            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = departmentRepository.OrderBy(query, nameof(Department.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var departmentsList = await queryPaged.Select(e => new GetDepartmentsResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                NumberOfEmployees = e.Employees != null ? e.Employees.Count : 0,
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
            criteria.IsActive = true;
            var departmentRepository = repositoryManager.DepartmentRepository;
            var query = departmentRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = departmentRepository.OrderBy(query, nameof(Department.Id), LeillaKeys.Desc);
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
                    ParentName = e.Parent != null ? e.Parent.Name : null,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryDepartmentNotFound);

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
                    ParentId = e.Parent != null ? e.ParentId : null,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryDepartmentNotFound);

            return department;

        }
        public async Task<bool> Delete(int departmentd)
        {
            var department = await repositoryManager.DepartmentRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == departmentd) ??
                throw new BusinessValidationException(LeillaKeys.SorryDepartmentNotFound);
            department.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}

