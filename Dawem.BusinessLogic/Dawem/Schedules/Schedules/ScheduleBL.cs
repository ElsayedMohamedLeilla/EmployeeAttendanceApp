using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessLogic.Dawem.Schedules.Schedules;
using Dawem.Contract.BusinessValidation.Dawem.Schedules.Schedules;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Schedules;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Schedules.Schedules;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Schedules.Schedules;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Schedules.Schedules
{
    public class ScheduleBL : IScheduleBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IScheduleBLValidation scheduleBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly INotificationHandleBL notificationHandleBL;
        public ScheduleBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper, INotificationHandleBL _notificationHandleBL,
           RequestInfo _requestHeaderContext,
           IScheduleBLValidation _scheduleBLValidation)
        {
            unitOfWork = _unitOfWork;
            notificationHandleBL = _notificationHandleBL;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            scheduleBLValidation = _scheduleBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateScheduleModel model)
        {
            #region Business Validation
            await scheduleBLValidation.CreateValidation(model);
            #endregion

            unitOfWork.CreateTransaction();

            #region Insert Schedule

            #region Set Schedule code

            var getNextCode = await repositoryManager.ScheduleRepository
                .Get(schedule => schedule.CompanyId == requestInfo.CompanyId)
                .Select(schedule => schedule.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var schedule = mapper.Map<Schedule>(model);
            schedule.CompanyId = requestInfo.CompanyId;
            schedule.AddUserId = requestInfo.UserId;
            schedule.Code = getNextCode;
            repositoryManager.ScheduleRepository.Insert(schedule);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return schedule.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateScheduleModel model)
        {
            #region Business Validation

            await scheduleBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update Schedule
            var getSchedule = await repositoryManager.ScheduleRepository
                .GetEntityByConditionWithTrackingAsync(schedule => !schedule.IsDeleted
            && schedule.Id == model.Id);

            getSchedule.Name = model.Name;
            getSchedule.Notes = model.Notes;
            getSchedule.IsActive = model.IsActive;
            getSchedule.ModifiedDate = DateTime.Now;
            getSchedule.ModifyUserId = requestInfo.UserId;

            await unitOfWork.SaveAsync();

            #region Handle Week Shifts

            var scheduleDaysChangedCount = 0;

            var getDBScheduleDays = await repositoryManager.ScheduleDayRepository
                .GetWithTracking(w => w.ScheduleId == model.Id)
                .ToListAsync();

            foreach (var getDBScheduleDay in getDBScheduleDays)
            {
                var modelShiftId = model?.ScheduleDays?.FirstOrDefault(w => w.WeekDay == getDBScheduleDay.WeekDay)?.ShiftId;

                if (getDBScheduleDay.ShiftId != modelShiftId)
                {
                    scheduleDaysChangedCount++;
                }
                getDBScheduleDay.ShiftId = modelShiftId;
            }

            await unitOfWork.SaveAsync();

            #endregion

            #endregion

            #region Handle Notifications

            if (scheduleDaysChangedCount > 0)
            {

                var employees = await repositoryManager.EmployeeRepository.
                    Get(e => !e.IsDeleted && e.IsActive && e.CompanyId == requestInfo.CompanyId &&
                    e.ScheduleId == model.Id).
                    Select(e => new
                    {
                        e.Id,
                        Users = e.Users != null ? e.Users.Where(u => !u.IsDeleted).Select(u => u.Id).ToList() : null
                    }).ToListAsync();

                var employeeIds = employees.Select(e => e.Id).ToList();
                var userIds = employees.Where(e => e.Users != null).SelectMany(e => e.Users).Distinct().ToList();
                var scheduleName = model.Name;

                #region Handle Notification Description

                var notificationDescriptions = new List<NotificationDescriptionModel>();

                var getActiveLanguages = await repositoryManager.LanguageRepository.Get(l => !l.IsDeleted && l.IsActive).
                       Select(l => new ActiveLanguageModel
                       {
                           Id = l.Id,
                           ISO2 = l.ISO2
                       }).ToListAsync();

                foreach (var language in getActiveLanguages)
                {
                    notificationDescriptions.Add(new NotificationDescriptionModel
                    {
                        LanguageIso2 = language.ISO2,
                        Description = TranslationHelper.GetTranslation(LeillaKeys.YourScheduleHaveBeenChangedToNewSchedule, language.ISO2) +
                            LeillaKeys.Space +
                            TranslationHelper.GetTranslation(LeillaKeys.ScheduleName, language.ISO2) +
                            LeillaKeys.ColonsThenSpace +
                            scheduleName + LeillaKeys.Space +
                            TranslationHelper.GetTranslation(LeillaKeys.UpdatedScheduleDaysCount, language.ISO2) +
                            LeillaKeys.ColonsThenSpace + scheduleDaysChangedCount
                    });
                }

                #endregion

                var handleNotificationModel = new HandleNotificationModel
                {
                    UserIds = userIds,
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

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetSchedulesResponse> Get(GetSchedulesCriteria criteria)
        {
            var scheduleRepository = repositoryManager.ScheduleRepository;
            var query = scheduleRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = scheduleRepository.OrderBy(query, nameof(Schedule.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var schedulesList = await queryPaged.Select(schedule => new GetSchedulesResponseModel
            {
                Id = schedule.Id,
                Code = schedule.Code,
                Name = schedule.Name,
                EmployeesNumber = schedule.Employees != null
                && schedule.Employees.Count > 0 ? schedule.Employees.Count : null,
                IsActive = schedule.IsActive
            }).ToListAsync();

            return new GetSchedulesResponse
            {
                Schedules = schedulesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetSchedulesForDropDownResponse> GetForDropDown(GetSchedulesCriteria criteria)
        {
            criteria.IsActive = true;
            var scheduleRepository = repositoryManager.ScheduleRepository;
            var query = scheduleRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = scheduleRepository.OrderBy(query, nameof(Schedule.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var schedulesList = await queryPaged.Select(schedule => new GetSchedulesForDropDownResponseModel
            {
                Id = schedule.Id,
                Name = schedule.Name
            }).ToListAsync();

            return new GetSchedulesForDropDownResponse
            {
                Schedules = schedulesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetScheduleInfoResponseModel> GetInfo(int scheduleId)
        {
            var schedule = await repositoryManager.ScheduleRepository.Get(schedule => schedule.Id == scheduleId && !schedule.IsDeleted)
                .Select(schedule => new GetScheduleInfoResponseModel
                {
                    Code = schedule.Code,
                    Name = schedule.Name,
                    Notes = schedule.Notes,
                    IsActive = schedule.IsActive,
                    ScheduleDays = schedule.ScheduleDays.Select(scheduleDay => new ScheduleShiftTextModel
                    {
                        WeekDayName = TranslationHelper.GetTranslation(scheduleDay.WeekDay.ToString(), requestInfo.Lang),
                        ShiftName = scheduleDay.Shift != null ? scheduleDay.Shift.Name : null
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryScheduleNotFound);

            return schedule;
        }
        public async Task<GetScheduleByIdResponseModel> GetById(int scheduleId)
        {
            var schedule = await repositoryManager.ScheduleRepository.Get(schedule => schedule.Id == scheduleId && !schedule.IsDeleted)
                .Select(schedule => new GetScheduleByIdResponseModel
                {
                    Id = schedule.Id,
                    Code = schedule.Code,
                    Name = schedule.Name,
                    IsActive = schedule.IsActive,
                    ScheduleDays = schedule.ScheduleDays.Select(weekShift => new ScheduleDayUpdateModel
                    {
                        Id = weekShift.Id,
                        WeekDay = weekShift.WeekDay,
                        ShiftId = weekShift.ShiftId,
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryScheduleNotFound);

            return schedule;

        }
        public async Task<bool> Delete(int scheduleId)
        {
            var schedule = await repositoryManager.ScheduleRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == scheduleId) ??
                throw new BusinessValidationException(LeillaKeys.SorryScheduleNotFound);

            schedule.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetSchedulesInformationsResponseDTO> GetSchedulesInformations()
        {
            var scheduleRepository = repositoryManager.ScheduleRepository;
            var query = scheduleRepository.Get(schedule => schedule.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetSchedulesInformationsResponseDTO
            {
                TotalCount = await query.CountAsync(),
                ActiveCount = await query.Where(schedule => !schedule.IsDeleted && schedule.IsActive).CountAsync(),
                NotActiveCount = await query.Where(schedule => !schedule.IsDeleted && !schedule.IsActive).CountAsync(),
                DeletedCount = await query.Where(schedule => schedule.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}