using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Domain.Entities.Employees;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Dtos.Reports.AttendanceSummaryReport;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Employees
{
    public class AttendanceReportBL : IAttendanceReportBL
    {
        private readonly IRepositoryManager repositoryManager;
        public AttendanceReportBL(IRepositoryManager _repositoryManager)
        {
            repositoryManager = _repositoryManager;
        }
        public async Task<AttendanceSummaryResponseDTO> GetAttendanceSummary(AttendanceSummaryCritria criteria)
        {
            var employeeRepository = repositoryManager.EmployeeRepository;
            var query = employeeRepository.GetAsQueryableForAttendanceSummary(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = employeeRepository.OrderBy(query, nameof(Employee.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var employeesList = await queryPaged.Select(employee => new AttendanceSummaryModel
            {
                EmployeeId = employee.Id,
                EmployeeNumber = employee.EmployeeNumber,
                EmployeeName = employee.Name,

                AllWorkingDaysCount = ReportsHelper.GetShouldAttendCount(employee.Schedule.ScheduleDays
                .Where(es => !es.IsDeleted && es.ShiftId > 0)
                .Select(sd => sd.WeekDay).ToList(), criteria.DateFrom, criteria.DateTo),


                ActualWorkingDaysCount = employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                ea.EmployeeAttendanceChecks != null && ea.LocalDate.Date >= criteria.DateFrom.Date
                && ea.LocalDate.Date <= criteria.DateTo.Date &&
                ea.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted)).Count(),


                LateArrivalsCount = ReportsHelper.SumListOfNumber(employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                ea.EmployeeAttendanceChecks != null && ea.LocalDate.Date >= criteria.DateFrom.Date
                && ea.LocalDate.Date <= criteria.DateTo.Date &&
                ea.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckIn) &&
                ea.EmployeeAttendanceChecks.FirstOrDefault(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckIn).Time > ea.ShiftCheckInTime)
                .Select(ea => EF.Functions.DateDiffMinute((DateTime)(object)ea.ShiftCheckInTime,
                (DateTime)(object)ea.EmployeeAttendanceChecks
                .FirstOrDefault(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckIn).Time) / 60).ToList()),


                EarlyDeparturesCount = ReportsHelper.SumListOfNumber(employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                ea.EmployeeAttendanceChecks != null && ea.LocalDate.Date >= criteria.DateFrom.Date
                && ea.LocalDate.Date <= criteria.DateTo.Date &&
                ea.EmployeeAttendanceChecks
                .FirstOrDefault(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckOut).Time < ea.ShiftCheckOutTime &&
                ea.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckOut))
                .Select(ea => EF.Functions.DateDiffMinute((DateTime)(object)ea.EmployeeAttendanceChecks
                .FirstOrDefault(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckOut).Time,
                (DateTime)(object)ea.ShiftCheckOutTime) / 60).ToList()),


                WorkingHoursCount = ReportsHelper.SumListOfNumber(employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                ea.EmployeeAttendanceChecks != null && ea.LocalDate.Date >= criteria.DateFrom.Date
                && ea.LocalDate.Date <= criteria.DateTo.Date &&
                ea.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckIn) &&
                ea.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckOut))
                .Select(ea => EF.Functions.DateDiffMinute((DateTime)(object)ea.EmployeeAttendanceChecks
                .FirstOrDefault(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckIn).Time,
                (DateTime)(object)ea.EmployeeAttendanceChecks
                .FirstOrDefault(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckOut).Time) / 60).ToList()),


                AbsencesCount = ReportsHelper.GetShouldAttendCount(employee.Schedule.ScheduleDays
                .Where(es => !es.IsDeleted && es.ShiftId > 0)
                .Select(sd => sd.WeekDay).ToList(), criteria.DateFrom, criteria.DateTo) -  // - vacations in this period
                employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                ea.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted) && ea.LocalDate.Date >= criteria.DateFrom.Date
                && ea.LocalDate.Date <= criteria.DateTo.Date).Count(),

                Plans = (employee.SchedulePlanEmployees != null ? 
                employee.SchedulePlanEmployees.Select(ep => ep.SchedulePlanId).ToList() : new List<int>()).Concat 
                (employee.DepartmentId != null && employee.Department.SchedulePlanDepartments != null ?
                employee.Department.SchedulePlanDepartments.Select(ep => ep.SchedulePlanId).ToList() : new List<int>()).Concat
                (employee.EmployeeGroups != null && employee.EmployeeGroups.All(eg=>eg.GroupId > 0) ?
                employee.EmployeeGroups.Select(eg => eg.Group).SelectMany(ge => ge.SchedulePlanGroups).Select(ep => ep.SchedulePlanId).ToList() : new List<int>()),


            }).ToListAsync();

            return new AttendanceSummaryResponseDTO
            {
                AttendanceSmmaries = employeesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
    }
}

