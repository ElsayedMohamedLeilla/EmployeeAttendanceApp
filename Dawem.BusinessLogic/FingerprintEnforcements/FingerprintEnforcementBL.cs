using AutoMapper;
using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.FingerprintEnforcements.FingerprintEnforcements;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees.AssignmentTypes;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Employees
{
    public class FingerprintEnforcementBL : IFingerprintEnforcementBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IFingerprintEnforcementBLValidation fingerprintEnforcementBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public FingerprintEnforcementBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IFingerprintEnforcementBLValidation _fingerprintEnforcementBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            fingerprintEnforcementBLValidation = _fingerprintEnforcementBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateFingerprintEnforcementModel model)
        {
            #region Business Validation

            await fingerprintEnforcementBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert FingerprintEnforcement

            #region Set FingerprintEnforcement code
            var getNextCode = await repositoryManager.FingerprintEnforcementRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var fingerprintEnforcement = mapper.Map<FingerprintEnforcement>(model);
            fingerprintEnforcement.CompanyId = requestInfo.CompanyId;
            fingerprintEnforcement.AddUserId = requestInfo.UserId;

            fingerprintEnforcement.Code = getNextCode;
            repositoryManager.FingerprintEnforcementRepository.Insert(fingerprintEnforcement);
            await unitOfWork.SaveAsync();

            #endregion

            #region Notifiacationas

            // for mogod

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return fingerprintEnforcement.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateFingerprintEnforcementModel model)
        {
            #region Business Validation

            await fingerprintEnforcementBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update FingerprintEnforcement

            var getFingerprintEnforcement = await repositoryManager.FingerprintEnforcementRepository
                .GetEntityByConditionWithTrackingAsync(fingerprintEnforcement => !fingerprintEnforcement.IsDeleted
                && fingerprintEnforcement.Id == model.Id);


            if (getFingerprintEnforcement != null)
            {
                getFingerprintEnforcement.ForType = model.ForType;
                getFingerprintEnforcement.ForAllEmployees = model.ForAllEmployees;
                getFingerprintEnforcement.FingerprintDate = model.FingerprintDate;
                getFingerprintEnforcement.AllowedTime = model.AllowedTime;
                getFingerprintEnforcement.TimeType = model.TimeType;
                getFingerprintEnforcement.IsActive = model.IsActive;
                getFingerprintEnforcement.ModifiedDate = DateTime.Now;
                getFingerprintEnforcement.ModifyUserId = requestInfo.UserId;

                #region Update Types And Actions

                var existEmployeeDbList = await repositoryManager.FingerprintEnforcementEmployeeRepository
                                .GetByCondition(e => e.FingerprintEnforcementId == getFingerprintEnforcement.Id)
                                .ToListAsync();
                var existGroupDbList = await repositoryManager.FingerprintEnforcementGroupRepository
                                .GetByCondition(e => e.FingerprintEnforcementId == getFingerprintEnforcement.Id)
                                .ToListAsync();
                var existDepartmentDbList = await repositoryManager.FingerprintEnforcementDepartmentRepository
                                .GetByCondition(e => e.FingerprintEnforcementId == getFingerprintEnforcement.Id)
                                .ToListAsync();
                var existActionDbList = await repositoryManager.FingerprintEnforcementActionRepository
                                .GetByCondition(e => e.FingerprintEnforcementId == getFingerprintEnforcement.Id)
                                .ToListAsync();

                switch (model.ForType)
                {
                    case ForType.Employees:

                        #region Handle Employees

                        var existingEmployeeIds = existEmployeeDbList.Select(e => e.EmployeeId).ToList();

                        var addedEmployees = model.Employees
                            .Where(employeeId => !existingEmployeeIds.Contains(employeeId))
                            .Select(employeeId => new FingerprintEnforcementEmployee
                            {
                                FingerprintEnforcementId = model.Id,
                                EmployeeId = employeeId,
                                ModifyUserId = requestInfo.UserId,
                                ModifiedDate = DateTime.UtcNow
                            })
                            .ToList();

                        var employeesToRemove = existEmployeeDbList
                            .Where(ge => !model.Employees.Contains(ge.EmployeeId))
                            .ToList();

                        if (employeesToRemove.Count > 0)
                            repositoryManager.FingerprintEnforcementEmployeeRepository.BulkDeleteIfExist(employeesToRemove);
                        if (addedEmployees.Count > 0)
                            repositoryManager.FingerprintEnforcementEmployeeRepository.BulkInsert(addedEmployees);

                        repositoryManager.FingerprintEnforcementGroupRepository.BulkDeleteIfExist(existGroupDbList);
                        repositoryManager.FingerprintEnforcementDepartmentRepository.BulkDeleteIfExist(existDepartmentDbList);

                        #endregion

                        break;
                    case ForType.Groups:

                        #region Handle Groups

                        var existingGroupIds = existGroupDbList.Select(e => e.GroupId).ToList();

                        var addedGroups = model.Groups
                            .Where(groupId => !existingGroupIds.Contains(groupId))
                            .Select(groupId => new FingerprintEnforcementGroup
                            {
                                FingerprintEnforcementId = model.Id,
                                GroupId = groupId,
                                ModifyUserId = requestInfo.UserId,
                                ModifiedDate = DateTime.UtcNow
                            })
                            .ToList();

                        var groupsToRemove = existGroupDbList
                            .Where(ge => !model.Groups.Contains(ge.GroupId))
                            .ToList();

                        if (groupsToRemove.Count > 0)
                            repositoryManager.FingerprintEnforcementGroupRepository.BulkDeleteIfExist(groupsToRemove);
                        if (addedGroups.Count > 0)
                            repositoryManager.FingerprintEnforcementGroupRepository.BulkInsert(addedGroups);

                        repositoryManager.FingerprintEnforcementEmployeeRepository.BulkDeleteIfExist(existEmployeeDbList);
                        repositoryManager.FingerprintEnforcementDepartmentRepository.BulkDeleteIfExist(existDepartmentDbList);

                        #endregion

                        break;
                    case ForType.Departments:

                        #region Handle Departments

                        var existingDepartmentIds = existDepartmentDbList.Select(e => e.DepartmentId).ToList();

                        var addedDepartments = model.Departments
                            .Where(departmentId => !existingDepartmentIds.Contains(departmentId))
                            .Select(departmentId => new FingerprintEnforcementDepartment
                            {
                                FingerprintEnforcementId = model.Id,
                                DepartmentId = departmentId,
                                ModifyUserId = requestInfo.UserId,
                                ModifiedDate = DateTime.UtcNow
                            })
                            .ToList();

                        var departmentsToRemove = existDepartmentDbList
                            .Where(ge => !model.Departments.Contains(ge.DepartmentId))
                            .ToList();

                        if (departmentsToRemove.Count > 0)
                            repositoryManager.FingerprintEnforcementDepartmentRepository.BulkDeleteIfExist(departmentsToRemove);
                        if (addedDepartments.Count > 0)
                            repositoryManager.FingerprintEnforcementDepartmentRepository.BulkInsert(addedDepartments);

                        repositoryManager.FingerprintEnforcementEmployeeRepository.BulkDeleteIfExist(existEmployeeDbList);
                        repositoryManager.FingerprintEnforcementGroupRepository.BulkDeleteIfExist(existGroupDbList);

                        #endregion

                        break;
                    default:
                        break;
                }


                #region Handle Actions

                var existingActionIds = existActionDbList.Select(e => e.NonComplianceActionId).ToList();

                var addedActions = model.Actions
                    .Where(actionId => !existingActionIds.Contains(actionId))
                    .Select(actionId => new FingerprintEnforcementAction
                    {
                        FingerprintEnforcementId = model.Id,
                        NonComplianceActionId = actionId,
                        ModifyUserId = requestInfo.UserId,
                        ModifiedDate = DateTime.UtcNow
                    })
                    .ToList();

                var actionsToRemove = existActionDbList
                    .Where(ge => !model.Actions.Contains(ge.NonComplianceActionId))
                    .ToList();

                if (actionsToRemove.Count > 0)
                    repositoryManager.FingerprintEnforcementActionRepository.BulkDeleteIfExist(actionsToRemove);
                if (addedActions.Count > 0)
                    repositoryManager.FingerprintEnforcementActionRepository.BulkInsert(addedActions);

                #endregion

                #endregion

                await unitOfWork.SaveAsync();

                #region Handle Response
                await unitOfWork.CommitAsync();
                return true;
                #endregion
            }
            #endregion

            else
                throw new BusinessValidationException(LeillaKeys.SorryFingerprintEnforcementNotFound);


        }
        public async Task<GetFingerprintEnforcementsResponse> Get(GetFingerprintEnforcementsCriteria criteria)
        {
            var fingerprintEnforcementRepository = repositoryManager.FingerprintEnforcementRepository;
            var query = fingerprintEnforcementRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = fingerprintEnforcementRepository.OrderBy(query, nameof(FingerprintEnforcement.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var fingerprintEnforcementsList = await queryPaged.Select(e => new GetFingerprintEnforcementsResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                ForType = e.ForType,
                FingerprintDate = e.FingerprintDate,
                ForTypeName = TranslationHelper.GetTranslation(e.ForType.ToString(), requestInfo.Lang),
                IsActive = e.IsActive
            }).ToListAsync();
            return new GetFingerprintEnforcementsResponse
            {
                FingerprintEnforcements = fingerprintEnforcementsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetFingerprintEnforcementInfoResponseModel> GetInfo(int FingerprintEnforcementId)
        {
            var fingerprintEnforcement = await repositoryManager.FingerprintEnforcementRepository.Get(e => e.Id == FingerprintEnforcementId && !e.IsDeleted)
                .Select(e => new GetFingerprintEnforcementInfoResponseModel
                {
                    Code = e.Code,
                    ForType = e.ForType,
                    ForAllEmployees = e.ForAllEmployees,
                    FingerprintDate = e.FingerprintDate,
                    AllowedTime = e.AllowedTime,
                    TimeType = e.TimeType,
                    ForTypeName = TranslationHelper.GetTranslation(e.ForType.ToString(), requestInfo.Lang),
                    Employees = e.FingerprintEnforcementEmployees != null ? e.FingerprintEnforcementEmployees.Select(e => e.Employee.Name).ToList() : null,
                    Groups = e.FingerprintEnforcementGroups != null ? e.FingerprintEnforcementGroups.Select(e => e.Group.Name).ToList() : null,
                    Departments = e.FingerprintEnforcementDepartments != null ? e.FingerprintEnforcementDepartments.Select(e => e.Department.Name).ToList() : null,
                    Actions = e.FingerprintEnforcementActions != null ? e.FingerprintEnforcementActions.Select(e => e.NonComplianceAction.Name).ToList() : null,
                    IsActive = e.IsActive
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryFingerprintEnforcementNotFound);

            return fingerprintEnforcement;
        }
        public async Task<GetFingerprintEnforcementByIdResponseModel> GetById(int FingerprintEnforcementId)
        {
            var fingerprintEnforcement = await repositoryManager.FingerprintEnforcementRepository.Get(e => e.Id == FingerprintEnforcementId && !e.IsDeleted)
                .Select(e => new GetFingerprintEnforcementByIdResponseModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    ForType = e.ForType,
                    ForAllEmployees = e.ForAllEmployees,
                    FingerprintDate = e.FingerprintDate,
                    AllowedTime = e.AllowedTime,
                    TimeType = e.TimeType,
                    Employees = e.FingerprintEnforcementEmployees != null ? e.FingerprintEnforcementEmployees.Select(e => e.EmployeeId).ToList() : null,
                    Groups = e.FingerprintEnforcementGroups != null ? e.FingerprintEnforcementGroups.Select(e => e.GroupId).ToList() : null,
                    Departments = e.FingerprintEnforcementDepartments != null ? e.FingerprintEnforcementDepartments.Select(e => e.DepartmentId).ToList() : null,
                    Actions = e.FingerprintEnforcementActions != null ? e.FingerprintEnforcementActions.Select(e => e.NonComplianceActionId).ToList() : null,
                    IsActive = e.IsActive
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryFingerprintEnforcementNotFound);

            return fingerprintEnforcement;

        }
        public async Task<bool> Delete(int fingerprintEnforcementd)
        {
            var fingerprintEnforcement = await repositoryManager.FingerprintEnforcementRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == fingerprintEnforcementd) ??
                throw new BusinessValidationException(LeillaKeys.SorryFingerprintEnforcementNotFound);
            fingerprintEnforcement.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetFingerprintEnforcementsInformationsResponseDTO> GetFingerprintEnforcementsInformations()
        {
            var fingerprintEnforcementRepository = repositoryManager.FingerprintEnforcementRepository;
            var query = fingerprintEnforcementRepository.Get(fingerprintEnforcement => fingerprintEnforcement.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetFingerprintEnforcementsInformationsResponseDTO
            {
                TotalCount = await query.Where(fingerprintEnforcement => !fingerprintEnforcement.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(fingerprintEnforcement => !fingerprintEnforcement.IsDeleted && fingerprintEnforcement.IsActive).CountAsync(),
                NotActiveCount = await query.Where(fingerprintEnforcement => !fingerprintEnforcement.IsDeleted && !fingerprintEnforcement.IsActive).CountAsync(),
                DeletedCount = await query.Where(fingerprintEnforcement => fingerprintEnforcement.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}

