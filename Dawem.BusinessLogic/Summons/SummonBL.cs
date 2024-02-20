using AutoMapper;
using Dawem.Contract.BusinessLogic.Summons;
using Dawem.Contract.BusinessValidation.Summons;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Summons;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Employees;
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

            var companyId = requestInfo.CompanyId;
            await summonBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert Summon

            #region Set Summon code
            var getNextCode = await repositoryManager.SummonRepository
                .Get(e => e.CompanyId == companyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var summon = mapper.Map<Summon>(model);

            summon.AddUserId = requestInfo.UserId;

            #region Handle Company Id

            summon.CompanyId = companyId;

            summon.SummonEmployees?.ForEach(e => e.CompanyId = companyId);
            summon.SummonGroups?.ForEach(e => e.CompanyId = companyId);
            summon.SummonDepartments?.ForEach(e => e.CompanyId = companyId);
            summon.SummonNotifyWays?.ForEach(e => e.CompanyId = companyId);
            summon.SummonSanctions?.ForEach(e => e.CompanyId = companyId);

            #endregion

            summon.Code = getNextCode;
            repositoryManager.SummonRepository.Insert(summon);
            await unitOfWork.SaveAsync();

            #endregion

            #region Notifiacations

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
                getSummon.DateAndTime = model.DateAndTime;
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
                                CompanyId = requestInfo.CompanyId,
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
                                CompanyId = requestInfo.CompanyId,
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
                                CompanyId = requestInfo.CompanyId,
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
                        CompanyId = requestInfo.CompanyId,
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
                        CompanyId = requestInfo.CompanyId,
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
            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var summonsList = await queryPaged.Select(s => new GetSummonsResponseModel
            {
                Id = s.Id,
                Code = s.Code,
                ForType = s.ForType,
                DateAndTime = s.DateAndTime,
                ForTypeName = TranslationHelper.GetTranslation(s.ForType.ToString(), requestInfo.Lang),
                IsActive = s.IsActive
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
                .Select(s => new GetSummonInfoResponseModel
                {
                    Code = s.Code,
                    ForAllEmployees = s.ForAllEmployees,
                    DateAndTime = s.DateAndTime,
                    AllowedTimeName = s.AllowedTime + LeillaKeys.Space + TranslationHelper.GetTranslation(s.TimeType.ToString() + LeillaKeys.TimeType, requestInfo.Lang),
                    ForTypeName = TranslationHelper.GetTranslation(s.ForType.ToString(), requestInfo.Lang),
                    NotifyWays = s.SummonNotifyWays.Count > 0 ? s.SummonNotifyWays.Select(n => TranslationHelper.GetTranslation(n.NotifyWay.ToString() + LeillaKeys.NotifyWay, requestInfo.Lang)).ToList() : null,
                    Employees = s.SummonEmployees.Count > 0 ? s.SummonEmployees.Select(e => e.Employee.Name).ToList() : null,
                    Groups = s.SummonGroups.Count > 0 ? s.SummonGroups.Select(e => e.Group.Name).ToList() : null,
                    Departments = s.SummonDepartments.Count > 0 ? s.SummonDepartments.Select(e => e.Department.Name).ToList() : null,
                    Sanctions = s.SummonSanctions.Count > 0 ? s.SummonSanctions.Select(e => e.Sanction.Name).ToList() : null,
                    IsActive = s.IsActive
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySummonNotFound);

            return summon;
        }
        public async Task<GetSummonByIdResponseModel> GetById(int summonId)
        {
            var summon = await repositoryManager.SummonRepository.Get(e => e.Id == summonId && !e.IsDeleted)
                .Select(s => new GetSummonByIdResponseModel
                {
                    Id = s.Id,
                    Code = s.Code,
                    ForType = s.ForType,
                    ForAllEmployees = s.ForAllEmployees,
                    DateAndTime = s.DateAndTime,
                    AllowedTime = s.AllowedTime,
                    TimeType = s.TimeType,
                    NotifyWays = s.SummonNotifyWays.Count > 0 ? s.SummonNotifyWays.Select(e => e.NotifyWay).ToList() : null,
                    Employees = s.SummonEmployees.Count > 0 ? s.SummonEmployees.Select(e => e.EmployeeId).ToList() : null,
                    Groups = s.SummonGroups.Count > 0 ? s.SummonGroups.Select(e => e.GroupId).ToList() : null,
                    Departments = s.SummonDepartments.Count > 0 ? s.SummonDepartments.Select(e => e.DepartmentId).ToList() : null,
                    Sanctions = s.SummonSanctions.Count > 0 ? s.SummonSanctions.Select(e => e.SanctionId).ToList() : null,
                    IsActive = s.IsActive
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySummonNotFound);

            return summon;

        }
        public async Task<bool> Enable(int summonId)
        {
            var sanction = await repositoryManager.SummonRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == summonId) ??
                throw new BusinessValidationException(LeillaKeys.SorrySummonNotFound);
            sanction.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var sanction = await repositoryManager.SummonRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorrySummonNotFound);
            sanction.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
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
        public async Task HandleSummonMissingLog()
        {
            try
            {
                var timeZones = await repositoryManager.CountryRepository
                .Get(c => !c.IsDeleted && c.TimeZoneId != null)
                .Select(c => c.TimeZoneId)
                .Distinct()
                .ToListAsync();

                var timeZonesDates = timeZones.Select(t => new
                {
                    TimeZoneId = t,
                    LocalDateTime = StringHelper.GetLocalDateTime(t)
                }).ToList();

                var maxLocalDate = timeZonesDates.Max(t => t.LocalDateTime);

                var tempGetEmployeesMissing = await repositoryManager
                    .EmployeeRepository.Get(e => !e.IsDeleted && e.Company.Country.TimeZoneId != null &&
                    e.Company.Summons.Any(s => !s.IsDeleted &&
                    maxLocalDate >= s.DateAndTime &&
                    ((s.TimeType == TimeType.Second && EF.Functions.DateDiffSecond(s.DateAndTime, maxLocalDate) > s.AllowedTime) ||
                    (s.TimeType == TimeType.Minute && EF.Functions.DateDiffMinute(s.DateAndTime, maxLocalDate) > s.AllowedTime) ||
                    (s.TimeType == TimeType.Hour && EF.Functions.DateDiffHour(s.DateAndTime, maxLocalDate) > s.AllowedTime)) &&
                    ((s.ForAllEmployees.HasValue && s.ForAllEmployees.Value) ||
                    (s.SummonEmployees != null && s.SummonEmployees.Any(se => !se.IsDeleted && se.EmployeeId == e.Id)) ||
                    (s.SummonGroups != null && s.SummonGroups.Any(sg => !sg.IsDeleted && sg.Group.GroupEmployees != null && sg.Group.GroupEmployees.Any(ge => !ge.IsDeleted && ge.EmployeeId == e.Id))) ||
                    (s.SummonDepartments != null && s.SummonDepartments.Any(sd => !sd.IsDeleted && sd.Department.Employees != null && sd.Department.Employees.Any(de => !de.IsDeleted && de.Id == e.Id)))) &&
                    !s.SummonMissingLogs.Any(sml => sml.EmployeeId == e.Id)))
                    .Select(e => new
                    {
                        EmployeeId = e.Id,
                        e.CompanyId,
                        e.Company.Country.TimeZoneId,
                        Summons = e.Company.Summons.Where(s => !s.IsDeleted && maxLocalDate >= s.DateAndTime &&
                        ((s.TimeType == TimeType.Second && EF.Functions.DateDiffSecond(s.DateAndTime, maxLocalDate) > s.AllowedTime) ||
                        (s.TimeType == TimeType.Minute && EF.Functions.DateDiffMinute(s.DateAndTime, maxLocalDate) > s.AllowedTime) ||
                        (s.TimeType == TimeType.Hour && EF.Functions.DateDiffHour(s.DateAndTime, maxLocalDate) > s.AllowedTime)) &&
                        ((s.ForAllEmployees.HasValue && s.ForAllEmployees.Value) ||
                        (s.SummonEmployees != null && s.SummonEmployees.Any(se => !se.IsDeleted && se.EmployeeId == e.Id)) ||
                        (s.SummonGroups != null && s.SummonGroups.Any(sg => !sg.IsDeleted && sg.Group.GroupEmployees != null && sg.Group.GroupEmployees.Any(ge => !ge.IsDeleted && ge.EmployeeId == e.Id))) ||
                        (s.SummonDepartments != null && s.SummonDepartments.Any(sd => !sd.IsDeleted && sd.Department.Employees != null && sd.Department.Employees.Any(de => !de.IsDeleted && de.Id == e.Id)))) && !s.SummonMissingLogs.Any(sml => sml.EmployeeId == e.Id))
                        .Select(s => new
                        {
                            SummonId = s.Id,
                            SummonSanctions = s.SummonSanctions
                            .Select(ss => new { ss.Id, SanctionType = ss.Sanction.Type }),
                            s.TimeType,
                            s.AllowedTime,
                            s.DateAndTime,
                            s.SummonEmployees,
                            WillCanceledEmployeeAttendanceId = s.SummonSanctions
                            .Any(ss => ss.Sanction.Type == SanctionType.CancelDayFingerprint) && e.EmployeeAttendances
                            .Any(ea => !ea.IsDeleted && ea.IsActive && ea.LocalDate.Date == s.DateAndTime.Date) ?
                            e.EmployeeAttendances.FirstOrDefault(ea => !ea.IsDeleted && ea.IsActive &&
                            ea.LocalDate.Date == s.DateAndTime.Date).Id : (int?)null
                        }).ToList()
                    }).ToListAsync();

                var getEmployeesMissing = tempGetEmployeesMissing
                    .Where(e => e.Summons.Any(s => StringHelper.GetLocalDateTime(e.TimeZoneId) > s.DateAndTime &&
                    ((s.TimeType == TimeType.Second && (maxLocalDate - s.DateAndTime).TotalSeconds > s.AllowedTime) ||
                    (s.TimeType == TimeType.Minute && (maxLocalDate - s.DateAndTime).TotalMinutes > s.AllowedTime) ||
                    (s.TimeType == TimeType.Hour && (maxLocalDate - s.DateAndTime).TotalHours > s.AllowedTime))))
                    .Select(e => new
                    {
                        e.EmployeeId,
                        e.CompanyId,
                        Summons = e.Summons.Where(s => StringHelper.GetLocalDateTime(e.TimeZoneId) >= s.DateAndTime &&
                        ((s.TimeType == TimeType.Second && (maxLocalDate - s.DateAndTime).TotalSeconds > s.AllowedTime) ||
                        (s.TimeType == TimeType.Minute && (maxLocalDate - s.DateAndTime).TotalMinutes > s.AllowedTime) ||
                        (s.TimeType == TimeType.Hour && (maxLocalDate - s.DateAndTime).TotalHours > s.AllowedTime)))
                        .Select(s => new
                        {
                            s.SummonId,
                            s.SummonSanctions,
                            s.DateAndTime,
                            s.WillCanceledEmployeeAttendanceId
                        }).ToList()
                    }).ToList();

                if (getEmployeesMissing != null && getEmployeesMissing.Count > 0)
                {
                    var summonMissingLogs = new List<SummonMissingLog>();

                    var employeesMissingGroupedByCompany =
                        getEmployeesMissing.GroupBy(e => e.CompanyId).ToList();

                    #region Handle Cancel Employee Attendances

                    var willCanceledEmployeeAttendanceIds = getEmployeesMissing
                        .Where(e => e.Summons.Any(ss => ss.WillCanceledEmployeeAttendanceId > 0))
                        .SelectMany(s => s.Summons.Where(es => es.WillCanceledEmployeeAttendanceId > 0)
                        .Select(ss => ss.WillCanceledEmployeeAttendanceId.Value)).ToList();

                    if (willCanceledEmployeeAttendanceIds.Count > 0)
                    {
                        var willCanceledEmployeeAttendances = await repositoryManager.EmployeeAttendanceRepository
                                        .GetWithTracking(ea => willCanceledEmployeeAttendanceIds.Contains(ea.Id))
                                        .ToListAsync();

                        willCanceledEmployeeAttendances.ForEach(getEmployeeAttendance =>
                        {
                            getEmployeeAttendance.IsActive = false;
                            getEmployeeAttendance.Notes += LeillaKeys.Space +
                            TranslationHelper.GetTranslation(LeillaKeys.NoteDayAttendanceWasCanceledDueToFailureInSummons, LeillaKeys.Ar);
                        });

                        await unitOfWork.SaveAsync();
                    }

                    #endregion




                    foreach (var employeesMissingGroup in employeesMissingGroupedByCompany)
                    {
                        #region Set Summon code

                        var getMaxCode = await repositoryManager.SummonMissingLogRepository
                            .Get(e => e.CompanyId == employeesMissingGroup.Key)
                            .Select(e => e.Code)
                            .DefaultIfEmpty()
                            .MaxAsync();

                        #endregion

                        foreach (var employeesMissing in employeesMissingGroup)
                        {
                            foreach (var summon in employeesMissing.Summons)
                            {
                                getMaxCode++;

                                summonMissingLogs.Add(new SummonMissingLog()
                                {
                                    Code = getMaxCode,
                                    CompanyId = employeesMissing.CompanyId,
                                    EmployeeId = employeesMissing.EmployeeId,
                                    SummonId = summon.SummonId,
                                    SummonMissingLogSanctions = summon.SummonSanctions
                                    .Select(ss => new SummonMissingLogSanction()
                                    {
                                        SummonSanctionId = ss.Id,
                                        Done = ss.SanctionType == SanctionType.CancelDayFingerprint
                                    }).ToList()
                                });

                            }
                        }
                    }

                    repositoryManager.SummonMissingLogRepository.BulkInsert(summonMissingLogs);
                    _ = unitOfWork.SaveAsync();
                }


                #region Send Notification To Employees Missing Summon

                // here

                #endregion


                var exception1 = 0;
                var exception2 = 0;
                var exception3 = 0;
                var exception4 = 0;
            }
            catch (Exception ex)
            {
                var exception = ex;
            }
        }
    }
}

