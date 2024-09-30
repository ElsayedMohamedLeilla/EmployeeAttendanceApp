using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessLogic.Dawem.Schedules.SchedulePlans;
using Dawem.Contract.BusinessValidation.Dawem.Schedules.SchedulePlans;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Schedules;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Schedules.SchedulePlans;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.DTOs.Dawem.RealTime.Firebase;
using Dawem.Models.Response.Dawem.Schedules.SchedulePlanLogs;
using Dawem.Models.Response.Dawem.Schedules.SchedulePlans;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Schedules.SchedulePlans
{
    public class SchedulePlanBL : ISchedulePlanBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly ISchedulePlanBLValidation schedulePlanBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly INotificationHandleBL notificationHandleBL;
        public SchedulePlanBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper, INotificationHandleBL _notificationHandleBL,
           RequestInfo _requestHeaderContext,
           ISchedulePlanBLValidation _schedulePlanBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            schedulePlanBLValidation = _schedulePlanBLValidation;
            mapper = _mapper;
            notificationHandleBL = _notificationHandleBL;
        }
        public async Task<int> Create(CreateSchedulePlanModel model)
        {
            #region Business Validation

            await schedulePlanBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert SchedulePlan

            #region Set SchedulePlan code

            var getNextCode = await repositoryManager.SchedulePlanRepository
                .Get(schedulePlan => schedulePlan.CompanyId == requestInfo.CompanyId)
                .Select(schedulePlan => schedulePlan.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var schedulePlan = mapper.Map<SchedulePlan>(model);
            schedulePlan.CompanyId = requestInfo.CompanyId;
            schedulePlan.AddUserId = requestInfo.UserId;
            schedulePlan.Code = getNextCode;
            repositoryManager.SchedulePlanRepository.Insert(schedulePlan);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return schedulePlan.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateSchedulePlanModel model)
        {
            #region Business Validation

            await schedulePlanBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update SchedulePlan

            var getSchedulePlan = await repositoryManager.SchedulePlanRepository
                .GetWithTracking(schedulePlan => !schedulePlan.IsDeleted
            && schedulePlan.Id == model.Id)
                .Include(s => s.SchedulePlanEmployee)
                .Include(s => s.SchedulePlanGroup)
                .Include(s => s.SchedulePlanDepartment)
                .FirstOrDefaultAsync();


            getSchedulePlan.ScheduleId = model.ScheduleId;
            getSchedulePlan.SchedulePlanType = model.SchedulePlanType;
            getSchedulePlan.DateFrom = model.DateFrom;
            getSchedulePlan.Notes = model.Notes;
            getSchedulePlan.IsActive = model.IsActive;
            getSchedulePlan.ModifiedDate = DateTime.Now;
            getSchedulePlan.ModifyUserId = requestInfo.UserId;

            #region Handle Related

            #region Employee

            if (model.EmployeeId != null)
            {
                if (getSchedulePlan.SchedulePlanEmployee != null)
                {
                    getSchedulePlan.SchedulePlanEmployee.EmployeeId = model.EmployeeId ?? 0;
                }
                else
                {
                    getSchedulePlan.SchedulePlanEmployee = new SchedulePlanEmployee { EmployeeId = model.EmployeeId ?? 0 };
                }
            }
            else
            {
                getSchedulePlan.SchedulePlanEmployee = null;
            }

            #endregion

            #region Group

            if (model.GroupId != null)
            {
                if (getSchedulePlan.SchedulePlanGroup != null)
                {
                    getSchedulePlan.SchedulePlanGroup.GroupId = model.GroupId ?? 0;
                }
                else
                {
                    getSchedulePlan.SchedulePlanGroup = new SchedulePlanGroup { GroupId = model.GroupId ?? 0 };
                }
            }
            else
            {
                getSchedulePlan.SchedulePlanGroup = null;
            }

            #endregion

            #region Department

            if (model.DepartmentId != null)
            {
                if (getSchedulePlan.SchedulePlanDepartment != null)
                {
                    getSchedulePlan.SchedulePlanDepartment.DepartmentId = model.DepartmentId ?? 0;
                }
                else
                {
                    getSchedulePlan.SchedulePlanDepartment = new SchedulePlanDepartment { DepartmentId = model.DepartmentId ?? 0 };
                }
            }
            else
            {
                getSchedulePlan.SchedulePlanDepartment = null;
            }

            #endregion

            #endregion

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetSchedulePlansResponse> Get(GetSchedulePlansCriteria criteria)
        {
            var schedulePlanRepository = repositoryManager.SchedulePlanRepository;
            var query = schedulePlanRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = schedulePlanRepository.OrderBy(query, nameof(SchedulePlan.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var schedulePlansList = await queryPaged.Select(schedulePlan => new GetSchedulePlansResponseModel
            {
                Id = schedulePlan.Id,
                Code = schedulePlan.Code,
                ScheduleName = schedulePlan.Schedule.Name,
                SchedulePlanTypeName = TranslationHelper.GetTranslation(schedulePlan.SchedulePlanType.ToString(), requestInfo.Lang),
                DateFrom = schedulePlan.DateFrom,
                IsActive = schedulePlan.IsActive
            }).ToListAsync();

            return new GetSchedulePlansResponse
            {
                SchedulePlans = schedulePlansList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetSchedulePlansForDropDownResponse> GetForDropDown(GetSchedulePlansCriteria criteria)
        {
            criteria.IsActive = true;
            var schedulePlanRepository = repositoryManager.SchedulePlanRepository;
            var query = schedulePlanRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = schedulePlanRepository.OrderBy(query, nameof(SchedulePlan.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var schedulePlansList = await queryPaged.Select(schedulePlan => new GetSchedulePlansForDropDownResponseModel
            {
                Id = schedulePlan.Id,
                Name = schedulePlan.Schedule.Name
            }).ToListAsync();

            return new GetSchedulePlansForDropDownResponse
            {
                SchedulePlans = schedulePlansList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetSchedulePlanInfoResponseModel> GetInfo(int schedulePlanId)
        {
            var schedulePlan = await repositoryManager.SchedulePlanRepository
                .Get(schedulePlan => schedulePlan.Id == schedulePlanId && !schedulePlan.IsDeleted)
                .Select(schedulePlan => new GetSchedulePlanInfoResponseModel
                {
                    Code = schedulePlan.Code,
                    SchedulePlanType = schedulePlan.SchedulePlanType,
                    SchedulePlanTypeName = TranslationHelper.GetTranslation(schedulePlan.SchedulePlanType.ToString(), requestInfo.Lang),
                    ScheduleName = schedulePlan.Schedule.Name,
                    EmployeeName = schedulePlan.SchedulePlanEmployee.Employee.Name,
                    GroupName = schedulePlan.SchedulePlanGroup.Group.Name,
                    DepartmentName = schedulePlan.SchedulePlanDepartment.Department.Name,
                    DateFrom = schedulePlan.DateFrom,
                    Notes = schedulePlan.Notes,
                    IsActive = schedulePlan.IsActive
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySchedulePlanNotFound);

            return schedulePlan;
        }
        public async Task<GetSchedulePlanByIdResponseModel> GetById(int schedulePlanId)
        {
            var schedulePlan = await repositoryManager.SchedulePlanRepository.Get(schedulePlan => schedulePlan.Id == schedulePlanId && !schedulePlan.IsDeleted)
                .Select(schedulePlan => new GetSchedulePlanByIdResponseModel
                {
                    Id = schedulePlan.Id,
                    Code = schedulePlan.Code,
                    SchedulePlanType = schedulePlan.SchedulePlanType,
                    ScheduleId = schedulePlan.ScheduleId,
                    EmployeeId = schedulePlan.SchedulePlanEmployee.EmployeeId,
                    GroupId = schedulePlan.SchedulePlanGroup.GroupId,
                    DepartmentId = schedulePlan.SchedulePlanDepartment.DepartmentId,
                    DateFrom = schedulePlan.DateFrom,
                    IsActive = schedulePlan.IsActive,
                    Notes = schedulePlan.Notes
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySchedulePlanNotFound);

            return schedulePlan;

        }
        public async Task<bool> Delete(int schedulePlanId)
        {
            var schedulePlan = await repositoryManager.SchedulePlanRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == schedulePlanId) ??
                throw new BusinessValidationException(LeillaKeys.SorrySchedulePlanNotFound);

            schedulePlan.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task HandleSchedulePlans()
        {
            try
            {
                var utcDate = DateTime.UtcNow;

                var getNextSchedulePlansFromDB = await repositoryManager.SchedulePlanRepository.
                    GetWithTracking(p => !p.IsDeleted && p.IsActive &&
                    !p.SchedulePlanLogs.Any() && p.DoneRetryCount < 2 &&
                    utcDate.AddHours(requestInfo.CompanyTimeZoneToUTC).Date >= p.DateFrom.Date).
                    Include(p => p.Schedule).
                    Include(p => p.SchedulePlanEmployee).
                    Include(p => p.SchedulePlanGroup).
                    Include(p => p.SchedulePlanDepartment).
                    ToListAsync();

                var getNextSchedulePlans = getNextSchedulePlansFromDB.
                    Select(p => new GetSchedulePlanLogModel
                    {
                        Id = p.Id,
                        CompanyId = p.CompanyId,
                        SchedulePlanId = p.Id,
                        NewScheduleId = p.ScheduleId,
                        ScheduleName = p.Schedule.Name,
                        SchedulePlanType = p.SchedulePlanType,
                        EmployeeId = p.SchedulePlanEmployee?.EmployeeId,
                        GroupId = p.SchedulePlanGroup?.GroupId,
                        DepartmentId = p.SchedulePlanDepartment?.DepartmentId
                    }).ToList();

                if (getNextSchedulePlans is not null && getNextSchedulePlans.Count > 0)
                {
                    foreach (var nextSchedulePlan in getNextSchedulePlans)
                    {
                        var startDate = DateTime.UtcNow;
                        switch (nextSchedulePlan.SchedulePlanType)
                        {
                            case ForType.Employees:


                                var employeeId = nextSchedulePlan.EmployeeId ?? 0;
                                var getEmployees = await repositoryManager.EmployeeRepository.
                                    GetWithTracking(e => !e.IsDeleted && e.Id == employeeId &&
                                    e.ScheduleId != nextSchedulePlan.NewScheduleId).
                                    ToListAsync();

                                if (getEmployees != null && getEmployees.Count > 0)
                                {
                                    await HandleSchedulePlanLogEmployee(getEmployees, nextSchedulePlan, startDate);
                                }

                                break;
                            case ForType.Groups:

                                var groupId = nextSchedulePlan.GroupId ?? 0;
                                var getEmployeesByGroup = await repositoryManager.EmployeeRepository
                                    .GetWithTracking(e => !e.IsDeleted && e.EmployeeGroups != null &&
                                    e.EmployeeGroups.Any(g => g.GroupId == nextSchedulePlan.GroupId) &&
                                    e.ScheduleId != nextSchedulePlan.NewScheduleId).
                                    ToListAsync();

                                if (getEmployeesByGroup != null && getEmployeesByGroup.Count > 0)
                                {
                                    await HandleSchedulePlanLogEmployee(getEmployeesByGroup, nextSchedulePlan, startDate);
                                }

                                break;
                            case ForType.Departments:

                                var departmentId = nextSchedulePlan.DepartmentId ?? 0;
                                var getEmployeesByDepartment = await repositoryManager.EmployeeRepository
                                    .GetWithTracking(e => !e.IsDeleted && e.DepartmentId == departmentId &&
                                    e.ScheduleId != nextSchedulePlan.NewScheduleId).
                                    ToListAsync();

                                if (getEmployeesByDepartment != null && getEmployeesByDepartment.Count > 0)
                                {
                                    await HandleSchedulePlanLogEmployee(getEmployeesByDepartment, nextSchedulePlan, startDate);
                                }

                                break;
                            default:
                                break;
                        }
                        var getSchedulePlansFromDBForUpdate = getNextSchedulePlansFromDB.
                            FirstOrDefault(p => p.Id == nextSchedulePlan.Id);
                        if (getSchedulePlansFromDBForUpdate != null)
                        {
                            getSchedulePlansFromDBForUpdate.DoneRetryCount++;
                        }
                    }
                    await unitOfWork.SaveAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public async Task HandleSchedulePlanLogEmployee(List<Employee> employees, GetSchedulePlanLogModel model, DateTime startDate)
        {
            var schedulePlanLog = new SchedulePlanLog
            {
                CompanyId = model.CompanyId,
                IsActive = true,
                SchedulePlanId = model.SchedulePlanId,
                SchedulePlanType = model.SchedulePlanType,
                StartDate = startDate
            };

            foreach (var employee in employees)
            {
                schedulePlanLog.SchedulePlanLogEmployees.Add(new SchedulePlanLogEmployee
                {
                    EmployeeId = employee.Id,
                    OldScheduleId = employee.ScheduleId,
                    NewScheduleId = model.NewScheduleId,
                    IsActive = true
                });
                employee.ScheduleId = model.NewScheduleId;
            }
            await unitOfWork.SaveAsync();

            #region Handle Notifications

            requestInfo.Lang = LeillaKeys.Ar;

            var getActiveLanguages = await repositoryManager.LanguageRepository.Get(l => !l.IsDeleted && l.IsActive).
                   Select(l => new ActiveLanguageModel
                   {
                       Id = l.Id,
                       ISO2 = l.ISO2
                   }).ToListAsync();

            var employeesGroupedBySchedule = employees.GroupBy(e => e.ScheduleId).ToList();

            foreach (var employeesGroup in employeesGroupedBySchedule)
            {
                var employeeIds = employeesGroup.Select(e => e.Id).ToList();
                var oldScheduleId = employeesGroup.First().ScheduleId;

                var oldScheduleName = await repositoryManager.ScheduleRepository.
                    Get(s => !s.IsDeleted && s.Id == oldScheduleId).
                    Select(s => s.Name).
                    FirstOrDefaultAsync();
                var newScheduleName = model.ScheduleName;

                var notificationUsers = await repositoryManager.UserRepository.
                Get(s => !s.IsDeleted && s.IsActive & s.EmployeeId > 0 &
                employeeIds.Contains(s.EmployeeId.Value)).
                Select(u => new NotificationUserModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserTokens = u.NotificationUsers.
                    Where(nu => !nu.IsDeleted && nu.NotificationUserFCMTokens.
                    Any(f => !f.IsDeleted)).
                    SelectMany(nu => nu.NotificationUserFCMTokens.Where(f => !f.IsDeleted).
                    Select(f => new NotificationUserTokenModel
                    {
                        ApplicationType = f.DeviceType,
                        Token = f.FCMToken
                    })).ToList()
                }).ToListAsync();

                #region Handle Notification Description

                var notificationDescriptions = new List<NotificationDescriptionModel>();

                foreach (var language in getActiveLanguages)
                {
                    notificationDescriptions.Add(new NotificationDescriptionModel
                    {
                        LanguageIso2 = language.ISO2,
                        Description = TranslationHelper.GetTranslation(LeillaKeys.YourScheduleHaveBeenChangedToNewSchedule, language.ISO2) +
                            LeillaKeys.Space +
                            TranslationHelper.GetTranslation(LeillaKeys.OldSchedule, language.ISO2) +
                            LeillaKeys.ColonsThenSpace +
                            oldScheduleName +
                            LeillaKeys.Space +
                            TranslationHelper.GetTranslation(LeillaKeys.NewSchedule, language.ISO2) +
                            LeillaKeys.ColonsThenSpace +
                            newScheduleName
                    });
                }

                #endregion

                var handleNotificationModel = new HandleNotificationModel
                {
                    CompanyId = model.CompanyId,
                    NotificationUsers = notificationUsers,
                    EmployeeIds = employeeIds,
                    NotificationType = NotificationType.NewChangeInSchedule,
                    NotificationStatus = NotificationStatus.Info,
                    Priority = NotificationPriority.Medium,
                    NotificationDescriptions = notificationDescriptions,
                    ActiveLanguages = getActiveLanguages
                };

                await notificationHandleBL.HandleNotifications(handleNotificationModel);
            }

            #endregion

            schedulePlanLog.FinishDate = DateTime.UtcNow;
            repositoryManager.SchedulePlanLogRepository.Insert(schedulePlanLog);

            await unitOfWork.SaveAsync();

        }
        public async Task<GetSchedulePlansInformationsResponseDTO> GetSchedulePlansInformations()
        {
            var schedulePlanRepository = repositoryManager.SchedulePlanRepository;
            var query = schedulePlanRepository.Get(schedulePlan => schedulePlan.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetSchedulePlansInformationsResponseDTO
            {
                TotalCount = await query.CountAsync(),
                ActiveCount = await query.Where(schedulePlan => !schedulePlan.IsDeleted && schedulePlan.IsActive).CountAsync(),
                NotActiveCount = await query.Where(schedulePlan => !schedulePlan.IsDeleted && !schedulePlan.IsActive).CountAsync(),
                DeletedCount = await query.Where(schedulePlan => schedulePlan.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}