using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Summons;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.BusinessValidation.Dawem.Summons;
using Dawem.Contract.RealTime.Firebase;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Summons;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Summons.Summons;
using Dawem.Models.Generic.Exceptions;
using Dawem.Models.Response.Summons.Summons;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Summons
{
    public class SummonBL : ISummonBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly ISummonBLValidation summonBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;
        private readonly INotificationServiceByFireBaseAdmin notificationServiceByFireBaseAdmin;

        public SummonBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           ISummonBLValidation _summonBLValidation, IUploadBLC _uploadBLC, INotificationServiceByFireBaseAdmin _notificationServiceByFireBaseAdmin)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            summonBLValidation = _summonBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;
            notificationServiceByFireBaseAdmin = _notificationServiceByFireBaseAdmin;
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

            #region Handle End Date

            var getTimeZoneId = await repositoryManager.CompanyRepository.
                Get(c => !c.IsDeleted && c.Id == requestInfo.CompanyId && c.Country.TimeZoneId != null).
                Select(c => c.Country.TimeZoneId).
                FirstOrDefaultAsync();
            var utcDateTime = DateHelper.GetUTSDateTime(summon.LocalDateAndTime, getTimeZoneId);
            summon.StartDateAndTimeUTC = utcDateTime;

            switch (model.TimeType)
            {
                case TimeType.Second:
                    summon.EndDateAndTimeUTC = utcDateTime.AddSeconds(model.AllowedTime);
                    break;
                case TimeType.Minute:
                    summon.EndDateAndTimeUTC = utcDateTime.AddMinutes(model.AllowedTime);
                    break;
                case TimeType.Hour:
                    summon.EndDateAndTimeUTC = utcDateTime.AddHours(model.AllowedTime);
                    break;
                default:
                    break;
            }

            #endregion

            #region Handle Summon Log

            summon.SummonLogs = await repositoryManager
                    .EmployeeRepository.Get(e => !e.IsDeleted && e.CompanyId == companyId &&
                    (model.ForAllEmployees.HasValue && model.ForAllEmployees.Value ||
                    model.Employees != null && model.Employees.Contains(e.Id) ||
                    model.Groups != null && e.EmployeeGroups.Any(eg => model.Groups.Contains(eg.GroupId)) ||
                    model.Departments != null && e.DepartmentId > 0 && model.Departments.Contains(e.DepartmentId.Value)))
                    .Select(e => new SummonLog
                    {
                        CompanyId = companyId,
                        EmployeeId = e.Id
                    }).ToListAsync();

            #endregion

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
            /*
            var notificationNextCode = await repositoryManager.NotificationStoreRepository
               .Get(e => e.CompanyId == requestInfo.CompanyId)
               .Select(e => e.Code)
               .DefaultIfEmpty()
               .MaxAsync();

            #region For All Employee

            List<NotificationRecieverDTO> notificationRecieverDTO = new();
            if (model.ForAllEmployees.HasValue && model.ForAllEmployees.Value)
            {
                notificationRecieverDTO = repositoryManager.UserRepository
                    .Get(s => s.IsActive && !s.IsDeleted && s.CompanyId == requestInfo.CompanyId && s.EmployeeId != null)
                    .Select(u => new NotificationRecieverDTO { EmployeeId = u.EmployeeId ?? 0, UserId = u.Id })
                    .ToList();
            }
            else
            {
                if (model.Employees != null && model.Employees.Any())
                {
                    notificationRecieverDTO = repositoryManager.UserRepository.Get(s => s.IsActive && !s.IsDeleted && s.CompanyId == requestInfo.CompanyId && s.EmployeeId != null && model.Employees.Contains(s.EmployeeId ?? 0)).Select(u => new NotificationRecieverDTO { EmployeeId = u.EmployeeId ?? 0, UserId = u.Id }).ToList();
                }
                if (model.Groups != null && model.Groups.Any())
                {
                    List<int> groupEmployeeIds = repositoryManager.GroupEmployeeRepository.Get(s => s.IsActive && !s.IsDeleted && model.Groups.Contains(s.Id)).Select(g => g.EmployeeId).ToList();
                    notificationRecieverDTO.AddRange(repositoryManager.UserRepository.Get(s => s.IsActive && !s.IsDeleted && s.CompanyId == requestInfo.CompanyId && s.EmployeeId != null && groupEmployeeIds.Contains(s.EmployeeId ?? 0)).Select(u => new NotificationRecieverDTO { EmployeeId = u.EmployeeId ?? 0, UserId = u.Id }).ToList());
                }
                if (model.Departments != null && model.Departments.Any())
                {
                    List<int> departmentEmployeeIds = repositoryManager.EmployeeRepository.Get(s => s.IsActive && !s.IsDeleted && model.Departments.Contains(s.DepartmentId ?? 0)).Select(g => g.Id).ToList();
                    notificationRecieverDTO.AddRange(repositoryManager.UserRepository.Get(s => s.IsActive && !s.IsDeleted && s.CompanyId == requestInfo.CompanyId && s.EmployeeId != null && model.Departments.Contains(s.EmployeeId ?? 0)).Select(u => new NotificationRecieverDTO { EmployeeId = u.EmployeeId ?? 0, UserId = u.Id }).ToList());
                }
            }

            List<NotificationStore> notificationStores = new();
            for (int i = 0; i < notificationRecieverDTO.Count; i++)
            {
                var notificationStore = new NotificationStore()
                {
                    Code = notificationNextCode++,
                    EmployeeId = notificationRecieverDTO[i].EmployeeId,
                    CompanyId = requestInfo.CompanyId,
                    AddUserId = requestInfo.UserId,
                    AddedDate = DateTime.UtcNow,
                    Status = NotificationStatus.Info,
                    NotificationType = NotificationType.NewSummons,
                    ImageUrl = NotificationHelper.GetNotificationImage(NotificationStatus.Warning, uploadBLC),
                    IsRead = false,
                    IsActive = true,
                    IsViewed = false,
                    Priority = Priority.High
                };
                notificationStores.Add(notificationStore);
                #region Fire Notification & Email
                await notificationServiceByFireBaseAdmin.Send_Notification_Email(notificationRecieverDTO.Select(S => S.UserId).ToList(), NotificationType.NewSummons, NotificationStatus.Warning);
                #endregion
            }
            repositoryManager.NotificationStoreRepository.BulkInsert(notificationStores);
            await unitOfWork.SaveAsync();



            #endregion

            #region Save Notification In DB

            //var notificationStore = new NotificationStore()
            //{
            //    Code = notificationNextCode,
            //    EmployeeId = requestEmployee.DirectManagerId ?? 0,
            //    CompanyId = requestInfo.CompanyId,
            //    AddUserId = requestInfo.UserId,
            //    AddedDate = DateTime.UtcNow,
            //    Status = NotificationStatus.Info,
            //    NotificationType = NotificationType.NewVacationRequest,
            //    ImageUrl = NotificationHelper.GetNotificationImage(NotificationStatus.Info, uploadBLC),
            //    IsRead = false,
            //    IsActive = true,
            //    Priority = Priority.Medium

            //};
            //repositoryManager.NotificationStoreRepository.Insert(notificationStore);
            //await unitOfWork.SaveAsync();
            #endregion
            */
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

            var companyId = requestInfo.CompanyId;
            var getSummon = await repositoryManager.SummonRepository
                .GetEntityByConditionWithTrackingAsync(summon => !summon.IsDeleted
                && summon.Id == model.Id);

            #region Handle Summon Log

            var newSummonLogEmployees = await repositoryManager
                    .EmployeeRepository.Get(e => !e.IsDeleted && e.CompanyId == companyId &&
                    (model.ForAllEmployees.HasValue && model.ForAllEmployees.Value ||
                    model.Employees != null && model.Employees.Contains(e.Id) ||
                    model.Groups != null && e.EmployeeGroups.Any(eg => model.Groups.Contains(eg.GroupId)) ||
                    model.Departments != null && e.DepartmentId > 0 && model.Departments.Contains(e.DepartmentId.Value)))
                    .Select(e => e.Id).ToListAsync();

            #endregion


            if (getSummon != null)
            {
                getSummon.ForType = model.ForType;
                getSummon.ForAllEmployees = model.ForAllEmployees;
                getSummon.LocalDateAndTime = model.LocalDateAndTime;
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
                var existSummonLogsDbList = await repositoryManager.SummonLogRepository
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
                        SanctionId = actionId
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
                        NotifyWay = notifyWay
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

                #region Handle Summon Logs

                var existingSummonLogsIds = existSummonLogsDbList.Select(e => e.EmployeeId).ToList();

                var addedSummonLogs = newSummonLogEmployees
                    .Where(employeeId => !existingSummonLogsIds.Contains(employeeId))
                    .Select(employeeId => new SummonLog
                    {
                        CompanyId = requestInfo.CompanyId,
                        SummonId = model.Id,
                        EmployeeId = employeeId
                    }).ToList();

                var summonLogsToRemove = existSummonLogsDbList
                    .Where(sl => !newSummonLogEmployees.Contains(sl.EmployeeId))
                    .ToList();

                if (summonLogsToRemove.Count > 0)
                    repositoryManager.SummonLogRepository.BulkDeleteIfExist(summonLogsToRemove);
                if (addedSummonLogs.Count > 0)
                    repositoryManager.SummonLogRepository.BulkInsert(addedSummonLogs);

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
            var utcDate = DateTime.UtcNow;

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
                DateAndTime = s.LocalDateAndTime,
                ForTypeName = TranslationHelper.GetTranslation(s.ForType.ToString(), requestInfo.Lang),
                NumberOfTargetedEmployees = s.SummonLogs.Count,
                SummonStatus = utcDate > s.EndDateAndTimeUTC ?
                    SummonStatus.Finished : utcDate < s.StartDateAndTimeUTC ?
                    SummonStatus.NotStarted : SummonStatus.OnGoing,
                SummonStatusName = TranslationHelper.GetTranslation(nameof(SummonStatus) + (utcDate > s.EndDateAndTimeUTC ?
                    SummonStatus.Finished : utcDate < s.StartDateAndTimeUTC ?
                    SummonStatus.NotStarted : SummonStatus.OnGoing).ToString() + LeillaKeys.TimeType, requestInfo.Lang),
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
            var utcDate = DateTime.UtcNow;
            var summon = await repositoryManager.SummonRepository.Get(e => e.Id == summonId && !e.IsDeleted)
                .Select(s => new GetSummonInfoResponseModel
                {
                    Code = s.Code,
                    ForAllEmployees = s.ForAllEmployees,
                    DateAndTime = s.LocalDateAndTime,
                    AllowedTimeName = s.AllowedTime + LeillaKeys.Space + TranslationHelper.GetTranslation(s.TimeType.ToString() + LeillaKeys.TimeType, requestInfo.Lang),
                    ForTypeName = TranslationHelper.GetTranslation(s.ForType.ToString(), requestInfo.Lang),
                    NotifyWays = s.SummonNotifyWays.Count > 0 ? s.SummonNotifyWays.Select(n => TranslationHelper.GetTranslation(n.NotifyWay.ToString() + LeillaKeys.NotifyWay, requestInfo.Lang)).ToList() : null,
                    Employees = s.SummonEmployees.Count > 0 ? s.SummonEmployees.Select(e => e.Employee.Name).ToList() : null,
                    Groups = s.SummonGroups.Count > 0 ? s.SummonGroups.Select(e => e.Group.Name).ToList() : null,
                    Departments = s.SummonDepartments.Count > 0 ? s.SummonDepartments.Select(e => e.Department.Name).ToList() : null,
                    Sanctions = s.SummonSanctions.Count > 0 ? s.SummonSanctions.Select(e => e.Sanction.Name).ToList() : null,
                    NumberOfTargetedEmployees = s.SummonLogs.Count,
                    SummonStatus = utcDate > s.EndDateAndTimeUTC ?
                    SummonStatus.Finished : utcDate < s.StartDateAndTimeUTC ?
                    SummonStatus.NotStarted : SummonStatus.OnGoing,
                    SummonStatusName = TranslationHelper.GetTranslation(nameof(SummonStatus) + (utcDate > s.EndDateAndTimeUTC ?
                    SummonStatus.Finished : utcDate < s.StartDateAndTimeUTC ?
                    SummonStatus.NotStarted : SummonStatus.OnGoing).ToString() + LeillaKeys.TimeType, requestInfo.Lang),
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
                    DateAndTime = s.LocalDateAndTime,
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
        public async Task HandleSummonLog()
        {
            try
            {
                var utcDateTime = DateTime.UtcNow;

                var getEmployeesMissingList = await repositoryManager
                    .SummonLogRepository.GetWithTracking(summonLog => !summonLog.IsDeleted && !summonLog.Summon.IsDeleted && summonLog.Company.Country.TimeZoneId != null &&
                    !summonLog.DoneSummon && !summonLog.DoneTakeActions && utcDateTime >= summonLog.Summon.EndDateAndTimeUTC).
                    Select(summonLog => new
                    {
                        summonLog.Id,
                        SummonSanctions = summonLog.Summon.SummonSanctions
                            .Select(ss => new { ss.Id, SanctionType = ss.Sanction.Type }),
                        EmployeeId = summonLog.Id,
                        WillCanceledEmployeeAttendanceId = summonLog.Summon.SummonSanctions
                            .Any(ss => ss.Sanction.Type == SanctionType.CancelDayFingerprint) && summonLog.Employee.EmployeeAttendances
                            .Any(ea => !ea.IsDeleted && ea.IsActive && ea.LocalDate.Date == summonLog.Summon.LocalDateAndTime.Date) ?
                            summonLog.Employee.EmployeeAttendances.FirstOrDefault(ea => !ea.IsDeleted && ea.IsActive &&
                            ea.LocalDate.Date == summonLog.Summon.LocalDateAndTime.Date).Id : (int?)null
                    }).ToListAsync();

                if (getEmployeesMissingList != null && getEmployeesMissingList.Count > 0)
                {
                    #region Handle Cancel Employee Attendances

                    var willCanceledEmployeeAttendanceIds = getEmployeesMissingList.
                        Where(e => e.WillCanceledEmployeeAttendanceId > 0).
                        Select(e => e.WillCanceledEmployeeAttendanceId.Value).
                        ToList();

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

                    #region Handle Summons Logs

                    var getEmployeesMissingIds = getEmployeesMissingList.
                                    Select(m => m.Id).ToList();

                    var getSummonLogs = await repositoryManager.
                        SummonLogRepository.GetWithTracking(summonLog => getEmployeesMissingIds.Contains(summonLog.Id)).
                        ToListAsync();

                    getSummonLogs.ForEach(m =>
                    {
                        m.DoneTakeActions = true;
                    });

                    var addedSummonLogSanctions = new List<SummonLogSanction>();

                    foreach (var employeesMissingGroup in getEmployeesMissingList)
                    {
                        addedSummonLogSanctions.
                            AddRange(employeesMissingGroup.SummonSanctions.
                            Select(ss => new SummonLogSanction()
                            {
                                SummonSanctionId = ss.Id,
                                Done = ss.SanctionType == SanctionType.CancelDayFingerprint
                            }).ToList());
                    }

                    repositoryManager.SummonLogSanctionRepository.BulkInsert(addedSummonLogSanctions);
                    await unitOfWork.SaveAsync();

                    #endregion

                    #region Send Notification To Employees Missing Summon

                    // here

                    #endregion
                }
            }
            catch (Exception ex)
            {
                var exception = ex;
            }
        }
    }
}

