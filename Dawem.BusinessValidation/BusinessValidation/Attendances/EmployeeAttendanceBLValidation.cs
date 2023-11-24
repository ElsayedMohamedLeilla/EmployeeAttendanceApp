using Dawem.Contract.BusinessValidation.Attendances;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Attendances;
using Dawem.Models.Dtos.Schedules.Schedules;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Attendances;
using Dawem.Models.Response.Schedules.Schedules;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Attendances
{

    public class EmployeeAttendanceBLValidation : IEmployeeAttendanceBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public EmployeeAttendanceBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<FingerPrintValidationResponseModel> FingerPrintValidation(FingerprintModel model)
        {
            var getEmployeeId = (requestInfo?.User?.EmployeeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryCurrentUserNotEmployee);

            var getScheduleId = await repositoryManager.EmployeeRepository
                .Get(e => e.Id == getEmployeeId && !e.IsDeleted)
                .Select(e => e.ScheduleId)
                .FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryEmployeeDoNotHaveSchedule);

            var getSchedule = await repositoryManager.ScheduleRepository.Get(schedule => schedule.Id == getScheduleId && !schedule.IsDeleted)
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
                       ShiftId = weekShift.ShiftId
                   }).ToList()
               }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryScheduleNotFound);

            var getTimeZoneId = await repositoryManager.CompanyRepository
                .Get(c => c.Id == requestInfo.CompanyId && !c.IsDeleted)
                .Select(c => c.Country.TimeZoneId)
                .FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryScheduleNotFound);

            var clientLocalDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.UtcNow, getTimeZoneId).DateTime;

            var shiftId = getSchedule.ScheduleDays.FirstOrDefault(d => (DayOfWeek)d.WeekDay == clientLocalDate.DayOfWeek)
                .ShiftId ?? throw new BusinessValidationException(LeillaKeys.SorryYouDoNotHaveScheduleToday);

            var shiftInfo = await repositoryManager.ShiftWorkingTimeRepository
                .Get(s => s.Id == shiftId)
                .Select(s => new
                {
                    s.CheckInTime,
                    s.CheckOutTime,
                    s.AllowedMinutes
                }).FirstOrDefaultAsync();

            return new FingerPrintValidationResponseModel
            {
                EmployeeId = getEmployeeId,
                ScheduleId = getScheduleId,
                ShiftId = shiftId,
                LocalDate = clientLocalDate,
                ShiftCheckInTime = shiftInfo.CheckInTime,
                ShiftCheckOutTime = shiftInfo.CheckOutTime,
                AllowedMinutes = shiftInfo.AllowedMinutes
            };
        }
        public async Task<GetCurrentFingerPrintInfoResponseModel> GetCurrentFingerPrintInfoValidation()
        {
            var getEmployeeId = (requestInfo?.User?.EmployeeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryCurrentUserNotEmployee);

            var getTimeZoneId = await repositoryManager.CompanyRepository
                .Get(c => c.Id == requestInfo.CompanyId && !c.IsDeleted)
                .Select(c => c.Country.TimeZoneId)
                .FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryScheduleNotFound);

            var clientLocalDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.UtcNow, getTimeZoneId).DateTime;

            var response = await repositoryManager.EmployeeAttendanceRepository
                .Get(a => !a.IsDeleted && a.EmployeeId == getEmployeeId
                && a.LocalDate.Date == clientLocalDate.Date)
                .Select(a => new GetCurrentFingerPrintInfoResponseModel
                {
                    Code = a.Code,
                    CheckInTime = a.EmployeeAttendanceChecks.FirstOrDefault() != null ?
                     a.EmployeeAttendanceChecks.FirstOrDefault().Time.ToString("HH:mm:ss") : null,
                    CheckOutTime = a.EmployeeAttendanceChecks.FirstOrDefault() != null && a.EmployeeAttendanceChecks.Count > 1 ?
                     a.EmployeeAttendanceChecks.OrderByDescending(c => c.Id).FirstOrDefault().Time.ToString("HH:mm:ss") : null,
                    LocalDate = clientLocalDate
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCurrentAttendanceInformationNotFound);

            return response;
        }

        public async Task<bool> GetEmployeeAttendancesValidation(GetEmployeeAttendancesCriteria model)
        {
            var getEmployeeId = (requestInfo?.User?.EmployeeId) ??
                 throw new BusinessValidationException(LeillaKeys.SorryCurrentUserNotEmployee);

            var checkIfHasAttendances = await repositoryManager.EmployeeAttendanceRepository
                .Get(a => !a.IsDeleted && a.EmployeeId == getEmployeeId
                && a.LocalDate.Date.Month == model.Month
                && a.LocalDate.Date.Year == model.Year)
                .AnyAsync();

            if (!checkIfHasAttendances)
                throw new BusinessValidationException(LeillaKeys.SorryThereIsNoAttendancesInSelectedYearAndMonth);

            return true;
        }
    }
}
