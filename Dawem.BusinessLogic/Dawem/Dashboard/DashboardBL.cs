using Dawem.Contract.BusinessLogic.Dawem.Dashboard;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.Repository.Manager;
using Dawem.Domain.Entities.Employees;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Dashboard;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Dashboard;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Dashboard
{
    public class DashboardBL : IDashboardBL
    {
        private readonly RequestInfo requestInfo;
        private readonly IRepositoryManager repositoryManager;
        private readonly IUploadBLC uploadBLC;
        public DashboardBL(IRepositoryManager _repositoryManager,
            IUploadBLC _uploadBLC,
           RequestInfo _requestHeaderContext)
        {
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            uploadBLC = _uploadBLC;
        }
        public async Task<GetHeaderInformationsResponseModel> GetHeaderInformations()
        {
            var employeeId = await repositoryManager.UserRepository.Get(u => !u.IsDeleted && u.Id == requestInfo.UserId)
               .Select(u => u.EmployeeId)
               .FirstOrDefaultAsync();

            var currentEmployeeName = await repositoryManager.EmployeeRepository.Get(e => e.Id == employeeId && !e.IsDeleted)
                .Select(employee => employee.Name).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);

            var currentCompanyId = requestInfo.CompanyId;
            var clientLocalDateTime = requestInfo.LocalDateTime;

            decimal mustAttendTodayCount = await repositoryManager.EmployeeRepository
                .Get(employee => !employee.IsDeleted &&
                employee.CompanyId == currentCompanyId &&
                employee.ScheduleId > 0 &&
                employee.Schedule.ScheduleDays != null &&
                employee.Schedule.ScheduleDays.FirstOrDefault(day => day.WeekDay == (WeekDay)clientLocalDateTime.DayOfWeek && day.ShiftId > 0) != null)
                .CountAsync();

            decimal attendedTodayCount = await repositoryManager.EmployeeAttendanceRepository
                .Get(employee => !employee.IsDeleted &&
                employee.LocalDate.Date == clientLocalDateTime.Date &&
                employee.CompanyId == currentCompanyId)
                .CountAsync();

            var employeesAttendanceRateToday = mustAttendTodayCount <= 0 ? 0 :
                Math.Round(attendedTodayCount / mustAttendTodayCount * 100, 2);

            #region Handle Response

            return new GetHeaderInformationsResponseModel
            {
                Name = currentEmployeeName,
                EmployeesAttendanceRateToday = employeesAttendanceRateToday
            };

            #endregion
        }
        public async Task<GetHeaderInformationsResponseForAdminPanelModel> GetHeaderInformationsForAdminPanel()
        {
            var userName = await repositoryManager.UserRepository.Get(u => !u.IsDeleted && u.Id == requestInfo.UserId)
               .Select(u => u.Name)
               .FirstOrDefaultAsync();

            #region Handle Response

            return new GetHeaderInformationsResponseForAdminPanelModel
            {
                Name = userName
            };

            #endregion
        }
        public async Task<EmployeeGetHeaderInformationsResponseModel> EmployeeGetHeaderInformations()
        {
            var employeeId = await repositoryManager.UserRepository.Get(u => !u.IsDeleted && u.Id == requestInfo.UserId)
               .Select(u => u.EmployeeId)
               .FirstOrDefaultAsync();

            var currentEmployeeJobTitleName = await repositoryManager.EmployeeRepository.Get(e => e.Id == employeeId && !e.IsDeleted)
                .Select(employee => employee.JobTitle.Name)
                .FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);


            if (requestInfo.User == null)
                throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);

            var currentCompanyId = requestInfo.CompanyId;
            var clientLocalDateTime = requestInfo.LocalDateTime;

            decimal mustAttendTodayCount = await repositoryManager.EmployeeRepository
                .Get(employee => !employee.IsDeleted &&
                employee.CompanyId == currentCompanyId &&
                employee.ScheduleId > 0 &&
                employee.Schedule.ScheduleDays != null &&
                employee.Schedule.ScheduleDays.FirstOrDefault(day => day.WeekDay == (WeekDay)clientLocalDateTime.DayOfWeek && day.ShiftId > 0) != null)
                .CountAsync();

            decimal attendedTodayCount = await repositoryManager.EmployeeAttendanceRepository
                .Get(employee => !employee.IsDeleted &&
                employee.LocalDate.Date == clientLocalDateTime.Date &&
                employee.CompanyId == currentCompanyId)
                .CountAsync();

            var employeesAttendanceRateToday = mustAttendTodayCount <= 0 ? 0 :
                Math.Round(attendedTodayCount / mustAttendTodayCount * 100, 2);

            #region Handle Response

            return new EmployeeGetHeaderInformationsResponseModel
            {
                Name = requestInfo.User.Name,
                JobTitleName = currentEmployeeJobTitleName,
                ProfileImagePath = uploadBLC.GetFilePath(requestInfo.User.ProfileImageName, LeillaKeys.Employees)
            };

            #endregion
        }
        public async Task<GetEmployeesAttendancesInformationsResponseModel> GetEmployeesAttendancesInformations()
        {
            var currentCompanyId = requestInfo.CompanyId;
            var clientLocalDateTime = requestInfo.LocalDateTime;
            var clientLocalDate = clientLocalDateTime.Date;
            var clientLocalDateWeekDay = (WeekDay)clientLocalDateTime.DayOfWeek;
            var clientLocalTimeOnly = clientLocalDateTime.TimeOfDay;
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

            EF.Functions.DateDiffMinute((DateTime)(object)employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift.CheckInTime,
            employee.EmployeeAttendances.FirstOrDefault(e => !e.IsDeleted && e.LocalDate.Date == clientLocalDate).EmployeeAttendanceChecks
            .FirstOrDefault(e => !e.IsDeleted && e.FingerPrintType == FingerPrintType.CheckIn).FingerPrintDate)
            > employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift.AllowedMinutes)
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
        public async Task<GetRequestsStatusResponseModel> GetRequestsStatus(GetRequestsStatusModel model)
        {
            model.LocalDate = requestInfo.LocalDateTime;

            var query = repositoryManager.RequestRepository.GetForStatusAsQueryable(model);

            #region Handle Response

            return new GetRequestsStatusResponseModel
            {
                AcceptedCount = await query.Where(request => request.Status == RequestStatus.Accepted).CountAsync(),
                RejectedCount = await query.Where(request => request.Status == RequestStatus.Rejected).CountAsync(),
                PendingCount = await query.Where(request => request.Status == RequestStatus.Pending).CountAsync()
            };

            #endregion
        }
        public async Task<GetEmployeesStatusResponseModel> GetEmployeesStatus()
        {
            var clientLocalDate = requestInfo.LocalDateTime;
            var query = repositoryManager.EmployeeRepository.Get(employee => employee.CompanyId == requestInfo.CompanyId &&
            !employee.IsDeleted);

            #region Available

            var availableCount = await query.Where(employee => !employee.EmployeeTasks.Any(task => !task.IsDeleted && !task.RequestTask.Request.IsDeleted
                && (task.RequestTask.Request.Status == RequestStatus.Accepted || task.RequestTask.Request.Status == RequestStatus.Pending)
                && clientLocalDate.Date >= task.RequestTask.Request.Date
                 && clientLocalDate.Date <= task.RequestTask.DateTo)

                &&

                !employee.EmployeeRequests.Any(request => !request.IsDeleted && !request.RequestTask.Request.IsDeleted
                && (request.RequestTask.Request.Status == RequestStatus.Accepted || request.RequestTask.Request.Status == RequestStatus.Pending)
                && request.Type == RequestType.Assignment
                && clientLocalDate.Date >= request.Date.Date
                 && clientLocalDate.Date <= request.RequestAssignment.DateTo)

                &&

                !employee.EmployeeRequests.Any(request => !request.IsDeleted && !request.RequestTask.Request.IsDeleted
                && (request.RequestTask.Request.Status == RequestStatus.Accepted || request.RequestTask.Request.Status == RequestStatus.Pending)
                && request.Type == RequestType.Vacation
                && clientLocalDate.Date >= request.Date.Date
                 && clientLocalDate.Date <= request.RequestVacation.DateTo)).CountAsync();

            #endregion

            #region Task Or Assignment

            var inTaskOrAssignmentCount = await query.Where(employee =>
            employee.EmployeeTasks.Any(task => !task.IsDeleted
                && (task.RequestTask.Request.Status == RequestStatus.Accepted || 
                task.RequestTask.Request.Status == RequestStatus.Pending)
                && clientLocalDate.Date >= task.RequestTask.Request.Date
                 && clientLocalDate.Date <= task.RequestTask.DateTo)

                ||

                employee.EmployeeRequests.Any(request => !request.IsDeleted
                && (request.Status == RequestStatus.Accepted || 
                request.Status == RequestStatus.Pending)
                && request.Type == RequestType.Assignment
                && clientLocalDate.Date >= request.Date.Date
                 && clientLocalDate.Date <= request.RequestAssignment.DateTo)).CountAsync();

            #endregion

            #region Vacation Or Outside

            var inVacationOrOutsideCount = await query.Where(employee =>
            employee.EmployeeRequests.Any(request => !request.IsDeleted && !request.RequestVacation.Request.IsDeleted
            && (request.RequestVacation.Request.Status == RequestStatus.Accepted || request.RequestVacation.Request.Status == RequestStatus.Pending)
            && request.Type == RequestType.Vacation
            && clientLocalDate.Date >= request.Date.Date
             && clientLocalDate.Date <= request.RequestVacation.DateTo)).CountAsync();

            #endregion

            #region Handle Response

            return new GetEmployeesStatusResponseModel
            {
                AvailableCount = availableCount,
                InTaskOrAssignmentCount = inTaskOrAssignmentCount,
                InVacationOrOutsideCount = inVacationOrOutsideCount
            };

            #endregion
        }
        public async Task<GetDepartmentsInformationsResponseModel> GetDepartmentsInformations(GetDepartmentsInformationsCriteria model)
        {
            var query = repositoryManager.DepartmentRepository
                .Get(department => department.CompanyId == requestInfo.CompanyId && !department.IsDeleted);

            #region paging

            int skip = PagingHelper.Skip(model.PageNumber, model.PageSize);
            int take = PagingHelper.Take(model.PageSize);

            #region sorting

            var queryOrdered = repositoryManager.DepartmentRepository.OrderBy(query, nameof(Department.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = model.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var departmentsList = await queryPaged.Select(e => new DepartmentModel
            {
                Id = e.Id,
                Name = e.Name,
                EmployeesCount = e.Employees.Count,
                LastEditDate = e.ModifiedDate ?? e.AddedDate,
                Employees = e.Employees.OrderByDescending(employee => employee.Id).Take(5).Select(employee => new DepartmentEmployeeModel
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    ProfileImagePath = uploadBLC.GetFilePath(employee.ProfileImageName, LeillaKeys.Employees)
                }).ToList()
            }).ToListAsync();

            return new GetDepartmentsInformationsResponseModel
            {
                Departments = departmentsList,
                TotalCount = await query.CountAsync()
            };

            #endregion
        }
        public async Task<GetEmployeesAttendancesStatusResponseModel> GetEmployeesAttendancesStatus(GetEmployeesAttendancesStatusCriteria model)
        {
            var query = repositoryManager.EmployeeRepository
                .Get(employee => employee.CompanyId == requestInfo.CompanyId &&
                !employee.IsDeleted &&
                employee.Schedule.ScheduleDays.FirstOrDefault(d => d.ShiftId > 0) != null);

            model.LocalDate = requestInfo.LocalDateTime;

            var queryAttendance = repositoryManager.EmployeeAttendanceRepository
                .GetForStatusAsQueryable(model);

            #region paging

            int skip = PagingHelper.Skip(model.PageNumber, model.PageSize);
            int take = PagingHelper.Take(model.PageSize);

            #region sorting

            var queryOrdered = repositoryManager.EmployeeRepository.OrderBy(query, nameof(Employee.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = model.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var employeesList = await queryPaged.Select(employee => new EmployeeModel
            {
                Id = employee.Id,
                Name = employee.Name,
                ProfileImagePath = uploadBLC.GetFilePath(employee.ProfileImageName, LeillaKeys.Employees),
                AttendanceRate = GetMustAttendCount(employee.Schedule.ScheduleDays.Where(d => d.ShiftId > 0).Select(s => s.WeekDay).ToList(), model) <= 0 ? 0 :
                 Math.Round(queryAttendance.Where(a => a.EmployeeId == employee.Id).Count() /
                GetMustAttendCount(employee.Schedule.ScheduleDays.Where(d => d.ShiftId > 0).Select(s => s.WeekDay).ToList(), model) * 100, 2)

            }).ToListAsync();

            return new GetEmployeesAttendancesStatusResponseModel
            {
                Employees = employeesList,
                TotalCount = await query.CountAsync()
            };

            #endregion
        }
        public async Task<GetBestEmployeesResponseModel> GetBestEmployees(GetBestEmployeesCriteria model)
        {
            var query = repositoryManager.EmployeeRepository
                .Get(employee => employee.CompanyId == requestInfo.CompanyId &&
                !employee.IsDeleted &&
                employee.Schedule.ScheduleDays.FirstOrDefault(d => d.ShiftId > 0) != null);

            model.LocalDate = requestInfo.LocalDateTime;

            var queryAttendance = repositoryManager.EmployeeAttendanceRepository
                .GetForStatusAsQueryable(model);

            #region paging

            int skip = PagingHelper.Skip(model.PageNumber, model.PageSize);
            int take = PagingHelper.Take(model.PageSize);

            #region sorting

            var queryOrdered = query.OrderByDescending(q => queryAttendance.Where(a => a.EmployeeId == q.Id).Count());

            #endregion

            var queryPaged = model.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var employeesList = await queryPaged.Select(employee => new EmployeeModel
            {
                Id = employee.Id,
                Name = employee.Name,
                ProfileImagePath = uploadBLC.GetFilePath(employee.ProfileImageName, LeillaKeys.Employees),
                AttendanceRate = GetMustAttendCount(employee.Schedule.ScheduleDays.Where(d => d.ShiftId > 0).Select(s => s.WeekDay).ToList(), model) <= 0 ? 0 :
                 Math.Round(queryAttendance.Where(a => a.EmployeeId == employee.Id).Count() /
                GetMustAttendCount(employee.Schedule.ScheduleDays.Where(d => d.ShiftId > 0).Select(s => s.WeekDay).ToList(), model) * 100, 2)

            }).ToListAsync();

            return new GetBestEmployeesResponseModel
            {
                Employees = employeesList,
                TotalCount = await query.CountAsync()
            };

            #endregion
        }
        private static decimal GetMustAttendCount(List<WeekDay> weekDays, GetStatusBaseModel model)
        {
            DateTime startDate = DateTime.UtcNow;
            DateTime endDate = DateTime.UtcNow;

            switch (model.Type)
            {
                case GetRequestsStatusType.CurrentDay:
                    startDate = model.DateFrom == null ? model.LocalDate : model.DateFrom.Value;
                    endDate = model.DateTo == null ? model.LocalDate : model.DateTo.Value;
                    break;
                case GetRequestsStatusType.CurrentMonth:
                    startDate = model.DateFrom == null ? new DateTime(model.LocalDate.Year, model.LocalDate.Month, 01) : model.DateFrom.Value;
                    endDate = model.DateTo == null ? new DateTime(model.LocalDate.Year, model.LocalDate.Month, DateTime.DaysInMonth(model.LocalDate.Year, model.LocalDate.Month)) : model.DateTo.Value;
                    break;
                case GetRequestsStatusType.CurrentYear:
                    startDate = model.DateFrom == null ? new DateTime(model.LocalDate.Year, 01, 01) : model.DateFrom.Value;
                    endDate = model.DateTo == null ? new DateTime(model.LocalDate.Year, 12, 31) : model.DateTo.Value;
                    break;
                default:
                    break;
            }

            var dates = Enumerable.Range(0, (int)(endDate - startDate).TotalDays + 1)
                      .Select(n => startDate.AddDays(n))
                      .ToList();

            var count = dates.Count(x => weekDays.Contains((WeekDay)x.DayOfWeek));
            return count;
        }

    }
}

