using AutoMapper;
using Dawem.Contract.BusinessLogic.Attendances.Schedules;
using Dawem.Contract.BusinessValidation.Attendances.Schedules;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Attendance;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Attendance;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees.Attendances.WeeksAttendances;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Attendances.Schedules
{
    public class ScheduleBL : IScheduleBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IScheduleBLValidation scheduleBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public ScheduleBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IScheduleBLValidation _scheduleBLValidation)
        {
            unitOfWork = _unitOfWork;
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

            var getDBScheduleDays = await repositoryManager.ScheduleDayRepository
                .GetWithTracking(w => w.ScheduleId == model.Id)
                .ToListAsync();

            foreach (var getDBScheduleDay in getDBScheduleDays)
            {
                getDBScheduleDay.ShiftId = model?.ScheduleDays?.FirstOrDefault(w => w.WeekDay == getDBScheduleDay.WeekDay)?.ShiftId;
            }

            await unitOfWork.SaveAsync();

            #endregion

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

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var schedulesList = await queryPaged.Select(schedule => new GetSchedulesResponseModel
            {
                Id = schedule.Id,
                Code = schedule.Code,
                Name = schedule.Name,
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

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

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
    }
}