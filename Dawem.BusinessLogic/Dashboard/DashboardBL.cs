using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees.Employee;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Employees
{
    public class DashboardBL : IDashboardBL
    {
        private readonly RequestInfo requestInfo;
        private readonly IRepositoryManager repositoryManager;
        public DashboardBL(IRepositoryManager _repositoryManager,
           RequestInfo _requestHeaderContext)
        {
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
        }
        private static int getsec(TimeOnly timeOnly)
        {
            return timeOnly.Second;
        }
        public async Task<GetEmployeesAttendancesInformationsResponseModel> GetEmployeesAttendancesInformations()
        {
            var currentCompanyId = requestInfo.CompanyId;

            var getTimeZoneId = await repositoryManager.CompanyRepository
                .Get(c => !c.IsDeleted && c.Id == currentCompanyId)
                .Select(c => c.Country.TimeZoneId)
                .FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryTimeZoneNotFound);

            var clientLocalDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.UtcNow, getTimeZoneId).DateTime;
            var clientLocalDate = clientLocalDateTime.Date;
            var clientLocalDateWeekDay = (WeekDay)clientLocalDateTime.DayOfWeek;
            var clientLocalTimeOnly = TimeOnly.FromTimeSpan(clientLocalDateTime.TimeOfDay);
            var newDateTime = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);

            var employeeAttendanceRepository = repositoryManager.EmployeeAttendanceRepository;
            var requestVacationRepository = repositoryManager.RequestVacationRepository;
            var employeeRepository = repositoryManager.EmployeeRepository;

            #region Attendances

            var dayTotalAttendanceCount = await employeeAttendanceRepository.Get(c => !c.IsDeleted &&
            c.CompanyId == currentCompanyId &&
            c.EmployeeAttendanceChecks.Count() > 0 &&
            c.LocalDate.Date == clientLocalDate)
                .CountAsync();

            #endregion

            #region Vacations

            var dayTotalVacationsCount = await requestVacationRepository.Get(c => !c.Request.IsDeleted &&
            c.Request.CompanyId == currentCompanyId &&
            clientLocalDate >= c.Request.Date && clientLocalDate <= c.DateTo)
                .CountAsync() + await employeeRepository.Get(employee => !employee.IsDeleted &&
            employee.CompanyId == currentCompanyId &&
            employee.ScheduleId != null &&
            employee.Schedule.ScheduleDays != null &&
            employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay) == null)
                .CountAsync();

            #endregion

            #region Absences

            var dayTotalAbsencesCount = await employeeRepository.Get(employee => !employee.IsDeleted &&
            employee.CompanyId == currentCompanyId &&
            employee.ScheduleId != null &&
            employee.Schedule.ScheduleDays != null &&
            employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay) != null &&
            employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift != null &&
            clientLocalTimeOnly > employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift.CheckInTime &&
            (employee.EmployeeAttendances == null || employee.EmployeeAttendances.FirstOrDefault(e => !e.IsDeleted && e.LocalDate.Date == clientLocalDate) == null))
                .CountAsync();

            #endregion

            #region Delays

            var dayTotalDelaysCount = await employeeRepository.Get(employee => !employee.IsDeleted &&
            employee.CompanyId == currentCompanyId &&
            employee.ScheduleId != null &&
            employee.Schedule.ScheduleDays != null &&
            employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay) != null &&
            employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift != null &&
            clientLocalTimeOnly >= employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift.CheckInTime &&
            employee.EmployeeAttendances != null &&
            employee.EmployeeAttendances.FirstOrDefault(e => !e.IsDeleted && e.LocalDate.Date == clientLocalDate) != null &&
            employee.EmployeeAttendances.FirstOrDefault(e => !e.IsDeleted && e.LocalDate.Date == clientLocalDate).EmployeeAttendanceChecks != null &&
            employee.EmployeeAttendances.FirstOrDefault(e => !e.IsDeleted && e.LocalDate.Date == clientLocalDate).EmployeeAttendanceChecks.FirstOrDefault(e => !e.IsDeleted && e.FingerPrintType == FingerPrintType.CheckIn) != null &&

            (EF.Functions.DateDiffMinute((DateTime)(object)employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift.CheckInTime,
            (DateTime)(object)employee.EmployeeAttendances.FirstOrDefault(e => !e.IsDeleted && e.LocalDate.Date == clientLocalDate).EmployeeAttendanceChecks
            .FirstOrDefault(e => !e.IsDeleted && e.FingerPrintType == FingerPrintType.CheckIn).Time)
            > employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift.AllowedMinutes))
                .CountAsync();

            #endregion

            #region Handle Response

            return new GetEmployeesAttendancesInformationsResponseModel
            {
                DayTotalAttendanceCount = dayTotalAttendanceCount,
                DayTotalVacationsCount = dayTotalVacationsCount,
                DayTotalAbsencesCount = dayTotalAbsencesCount,
                DayTotalDelaysCount = dayTotalDelaysCount
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
                    ProfileImagePath = null,/*uploadBLC.GetFilePath(e.ProfileImageName, LeillaKeys.Employees),*/
                    DisableReason = e.DisableReason,
                    Zones = e.Zones
                    .Select(d => d.Zone.Name)
                    .ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);

            return employee;
        }
    }
}

