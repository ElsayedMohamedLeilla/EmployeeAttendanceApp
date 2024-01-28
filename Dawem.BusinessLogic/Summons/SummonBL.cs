using AutoMapper;
using Dawem.Contract.BusinessLogic.Summons;
using Dawem.Contract.BusinessValidation.Summons;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Summons;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Summons.Summons;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Summons.Summons;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Summons
{
    public class SummonBL : ISummonBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly ISummonBLValidation summonBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public SummonBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           ISummonBLValidation _summonBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            summonBLValidation = _summonBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateSummonModel model)
        {
            #region Business Validation

            await summonBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert Summon

            #region Set Summon code
            var getNextCode = await repositoryManager.SummonRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var summon = mapper.Map<Summon>(model);
            summon.CompanyId = requestInfo.CompanyId;
            summon.AddUserId = requestInfo.UserId;

            summon.Code = getNextCode;
            repositoryManager.SummonRepository.Insert(summon);
            await unitOfWork.SaveAsync();

            #endregion

            #region Notifiacationas

            // for mogod

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return summon.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateSummonModel model)
        {
            #region Business Validation

            await summonBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update Summon

            var getSummon = await repositoryManager.SummonRepository
                .GetEntityByConditionWithTrackingAsync(summon => !summon.IsDeleted
                && summon.Id == model.Id);


            if (getSummon != null)
            {
                getSummon.ForType = model.ForType;
                getSummon.ForAllEmployees = model.ForAllEmployees;
                getSummon.FingerprintDate = model.FingerprintDate;
                getSummon.AllowedTime = model.AllowedTime;
                getSummon.TimeType = model.TimeType;
                getSummon.IsActive = model.IsActive;
                getSummon.ModifiedDate = DateTime.Now;
                getSummon.ModifyUserId = requestInfo.UserId;

                #region Update Types And Sanctions

                var existEmployeeDbList = await repositoryManager.SummonEmployeeRepository
                                .GetByCondition(e => !e.IsDeleted && e.SummonId == getSummon.Id)
                                .ToListAsync();
                var existGroupDbList = await repositoryManager.SummonGroupRepository
                                .GetByCondition(e => !e.IsDeleted & e.SummonId == getSummon.Id)
                                .ToListAsync();
                var existDepartmentDbList = await repositoryManager.SummonDepartmentRepository
                                .GetByCondition(e => !e.IsDeleted & e.SummonId == getSummon.Id)
                                .ToListAsync();
                var existSanctionDbList = await repositoryManager.SummonSanctionRepository
                                .GetByCondition(e => !e.IsDeleted & e.SummonId == getSummon.Id)
                                .ToListAsync();
                var existNotifyWaysDbList = await repositoryManager.SummonNotifyWayRepository
                                .GetByCondition(e => !e.IsDeleted & e.SummonId == getSummon.Id)
                                .ToListAsync();

                switch (model.ForType)
                {
                    case ForType.Employees:

                        #region Handle Employees

                        var existingEmployeeIds = existEmployeeDbList.Select(e => e.EmployeeId).ToList();

                        var addedEmployees = model.Employees
                            .Where(employeeId => !existingEmployeeIds.Contains(employeeId))
                            .Select(employeeId => new SummonEmployee
                            {
                                SummonId = model.Id,
                                EmployeeId = employeeId,
                                ModifyUserId = requestInfo.UserId,
                                ModifiedDate = DateTime.UtcNow
                            })
                            .ToList();

                        var employeesToRemove = existEmployeeDbList
                            .Where(ge => !model.Employees.Contains(ge.EmployeeId))
                            .ToList();

                        if (employeesToRemove.Count > 0)
                            repositoryManager.SummonEmployeeRepository.BulkDeleteIfExist(employeesToRemove);
                        if (addedEmployees.Count > 0)
                            repositoryManager.SummonEmployeeRepository.BulkInsert(addedEmployees);

                        repositoryManager.SummonGroupRepository.BulkDeleteIfExist(existGroupDbList);
                        repositoryManager.SummonDepartmentRepository.BulkDeleteIfExist(existDepartmentDbList);

                        #endregion

                        break;
                    case ForType.Groups:

                        #region Handle Groups

                        var existingGroupIds = existGroupDbList.Select(e => e.GroupId).ToList();

                        var addedGroups = model.Groups
                            .Where(groupId => !existingGroupIds.Contains(groupId))
                            .Select(groupId => new SummonGroup
                            {
                                SummonId = model.Id,
                                GroupId = groupId,
                                ModifyUserId = requestInfo.UserId,
                                ModifiedDate = DateTime.UtcNow
                            })
                            .ToList();

                        var groupsToRemove = existGroupDbList
                            .Where(ge => !model.Groups.Contains(ge.GroupId))
                            .ToList();

                        if (groupsToRemove.Count > 0)
                            repositoryManager.SummonGroupRepository.BulkDeleteIfExist(groupsToRemove);
                        if (addedGroups.Count > 0)
                            repositoryManager.SummonGroupRepository.BulkInsert(addedGroups);

                        repositoryManager.SummonEmployeeRepository.BulkDeleteIfExist(existEmployeeDbList);
                        repositoryManager.SummonDepartmentRepository.BulkDeleteIfExist(existDepartmentDbList);

                        #endregion

                        break;
                    case ForType.Departments:

                        #region Handle Departments

                        var existingDepartmentIds = existDepartmentDbList.Select(e => e.DepartmentId).ToList();

                        var addedDepartments = model.Departments
                            .Where(departmentId => !existingDepartmentIds.Contains(departmentId))
                            .Select(departmentId => new SummonDepartment
                            {
                                SummonId = model.Id,
                                DepartmentId = departmentId,
                                ModifyUserId = requestInfo.UserId,
                                ModifiedDate = DateTime.UtcNow
                            })
                            .ToList();

                        var departmentsToRemove = existDepartmentDbList
                            .Where(ge => !model.Departments.Contains(ge.DepartmentId))
                            .ToList();

                        if (departmentsToRemove.Count > 0)
                            repositoryManager.SummonDepartmentRepository.BulkDeleteIfExist(departmentsToRemove);
                        if (addedDepartments.Count > 0)
                            repositoryManager.SummonDepartmentRepository.BulkInsert(addedDepartments);

                        repositoryManager.SummonEmployeeRepository.BulkDeleteIfExist(existEmployeeDbList);
                        repositoryManager.SummonGroupRepository.BulkDeleteIfExist(existGroupDbList);

                        #endregion

                        break;
                    default:
                        break;
                }

                #region Handle Sanctions

                var existingSanctionIds = existSanctionDbList.Select(e => e.SanctionId).ToList();

                var addedSanctions = model.Sanctions
                    .Where(actionId => !existingSanctionIds.Contains(actionId))
                    .Select(actionId => new SummonSanction
                    {
                        SummonId = model.Id,
                        SanctionId = actionId,
                        ModifyUserId = requestInfo.UserId,
                        ModifiedDate = DateTime.UtcNow
                    })
                    .ToList();

                var actionsToRemove = existSanctionDbList
                    .Where(ge => !model.Sanctions.Contains(ge.SanctionId))
                    .ToList();

                if (actionsToRemove.Count > 0)
                    repositoryManager.SummonSanctionRepository.BulkDeleteIfExist(actionsToRemove);
                if (addedSanctions.Count > 0)
                    repositoryManager.SummonSanctionRepository.BulkInsert(addedSanctions);

                #endregion

                #region Handle Notify Ways

                var existingNotifyWaysIds = existNotifyWaysDbList.Select(e => e.NotifyWay).ToList();

                var addedNotifyWays = model.NotifyWays
                    .Where(notifyWay => !existingNotifyWaysIds.Contains(notifyWay))
                    .Select(notifyWay => new SummonNotifyWay
                    {
                        SummonId = model.Id,
                        NotifyWay = notifyWay,
                        ModifyUserId = requestInfo.UserId,
                        ModifiedDate = DateTime.UtcNow
                    })
                    .ToList();

                var notifyWaysToRemove = existNotifyWaysDbList
                    .Where(ge => !model.NotifyWays.Contains(ge.NotifyWay))
                    .ToList();

                if (notifyWaysToRemove.Count > 0)
                    repositoryManager.SummonNotifyWayRepository.BulkDeleteIfExist(notifyWaysToRemove);
                if (addedNotifyWays.Count > 0)
                    repositoryManager.SummonNotifyWayRepository.BulkInsert(addedNotifyWays);

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
                throw new BusinessValidationException(LeillaKeys.SorrySummonNotFound);


        }
        public async Task<GetSummonsResponse> Get(GetSummonsCriteria criteria)
        {
            var summonRepository = repositoryManager.SummonRepository;
            var query = summonRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = summonRepository.OrderBy(query, nameof(Summon.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var summonsList = await queryPaged.Select(e => new GetSummonsResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                ForType = e.ForType,
                FingerprintDate = e.FingerprintDate,
                ForTypeName = TranslationHelper.GetTranslation(e.ForType.ToString(), requestInfo.Lang),
                IsActive = e.IsActive
            }).ToListAsync();
            return new GetSummonsResponse
            {
                Summons = summonsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetSummonInfoResponseModel> GetInfo(int summonId)
        {
            var summon = await repositoryManager.SummonRepository.Get(e => e.Id == summonId && !e.IsDeleted)
                .Select(e => new GetSummonInfoResponseModel
                {
                    Code = e.Code,
                    ForType = e.ForType,
                    ForAllEmployees = e.ForAllEmployees,
                    FingerprintDate = e.FingerprintDate,
                    AllowedTime = e.AllowedTime,
                    TimeType = e.TimeType,
                    ForTypeName = TranslationHelper.GetTranslation(e.ForType.ToString(), requestInfo.Lang),
                    NotifyWays = e.SummonNotifyWays != null ? e.SummonNotifyWays.Select(n => TranslationHelper.GetTranslation(n.NotifyWay.ToString() + LeillaKeys.NotifyWay, requestInfo.Lang)).ToList() : null,
                    Employees = e.SummonEmployees != null ? e.SummonEmployees.Select(e => e.Employee.Name).ToList() : null,
                    Groups = e.SummonGroups != null ? e.SummonGroups.Select(e => e.Group.Name).ToList() : null,
                    Departments = e.SummonDepartments != null ? e.SummonDepartments.Select(e => e.Department.Name).ToList() : null,
                    Actions = e.SummonActions != null ? e.SummonActions.Select(e => e.Sanction.Name).ToList() : null,
                    IsActive = e.IsActive
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySummonNotFound);

            return summon;
        }
        public async Task<GetSummonByIdResponseModel> GetById(int summonId)
        {
            var summon = await repositoryManager.SummonRepository.Get(e => e.Id == summonId && !e.IsDeleted)
                .Select(e => new GetSummonByIdResponseModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    ForType = e.ForType,
                    ForAllEmployees = e.ForAllEmployees,
                    FingerprintDate = e.FingerprintDate,
                    AllowedTime = e.AllowedTime,
                    TimeType = e.TimeType,
                    NotifyWays = e.SummonNotifyWays != null ? e.SummonNotifyWays.Select(e => e.NotifyWay).ToList() : null,
                    Employees = e.SummonEmployees != null ? e.SummonEmployees.Select(e => e.EmployeeId).ToList() : null,
                    Groups = e.SummonGroups != null ? e.SummonGroups.Select(e => e.GroupId).ToList() : null,
                    Departments = e.SummonDepartments != null ? e.SummonDepartments.Select(e => e.DepartmentId).ToList() : null,
                    Actions = e.SummonActions != null ? e.SummonActions.Select(e => e.SanctionId).ToList() : null,
                    IsActive = e.IsActive
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySummonNotFound);

            return summon;

        }
        public async Task<bool> Delete(int summond)
        {
            var summon = await repositoryManager.SummonRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == summond) ??
                throw new BusinessValidationException(LeillaKeys.SorrySummonNotFound);
            summon.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetSummonsInformationsResponseDTO> GetSummonsInformations()
        {
            var summonRepository = repositoryManager.SummonRepository;
            var query = summonRepository.Get(summon => summon.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetSummonsInformationsResponseDTO
            {
                TotalCount = await query.Where(summon => !summon.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(summon => !summon.IsDeleted && summon.IsActive).CountAsync(),
                NotActiveCount = await query.Where(summon => !summon.IsDeleted && !summon.IsActive).CountAsync(),
                DeletedCount = await query.Where(summon => summon.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}

