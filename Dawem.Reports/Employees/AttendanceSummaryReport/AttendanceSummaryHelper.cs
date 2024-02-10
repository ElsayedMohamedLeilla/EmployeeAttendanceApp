using Dawem.Contract.Repository.Manager;
using Dawem.Domain.Entities.Attendances;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Reports.AttendanceSummaryReport;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Reports.Employees.AttendanceSummaryReport
{
    public class AttendanceSummaryHelper
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;

        public AttendanceSummaryHelper(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public async Task<AttendanceSummaryForGridDTO> Get(AttendanceSummaryCritria model)
        {
            var result = await repositoryManager.EmployeeRepository.Get(
     employee =>
         (model.EmployeeId == null || employee.Id == model.EmployeeId) && // Check if employee is selected
         !employee.IsDeleted &&
         employee.IsActive &&
         employee.CompanyId == 7 /*requestInfo.CompanyId*/ &&
         employee.EmployeeAttendances
             .Any(ea =>
                 ea.EmployeeAttendanceChecks
                     .Any(eac =>
                         eac != null && eac.IsActive && !eac.IsDeleted &&
                         (!model.FromDate.HasValue || ea.LocalDate >= model.FromDate.Value) &&
                         (!model.ToDate.HasValue || ea.LocalDate <= model.ToDate.Value)
                     )
             ) &&
         employee.SchedulePlanEmployees
             .Any(spe =>
                 spe.IsActive &&
                 spe.SchedulePlan.DateFrom <= (model.ToDate ?? DateTime.MaxValue) &&
                 spe.SchedulePlan.DateFrom >= (model.FromDate ?? DateTime.MinValue)
             )
 ).Select(employee => new AttendanceSummaryModelDTO
 {
     EmployeeId = employee.Id,
     EmployeeNumber = employee.EmployeeNumber,
     EmployeeName = employee.Name,
     Total_Working_Hours = CalculateTotalWorkingHours(employee.EmployeeAttendances, model.FromDate ?? DateTime.MinValue, model.ToDate ?? DateTime.MaxValue),
     Total_Absences = employee.EmployeeAttendances
         .Select(ea => ea.LocalDate) // Select attendance dates
         .GroupBy(date => date) // Group by attendance date
         .Count(group => !group.Any(date => employee.EmployeeAttendances
             .Any(ea => ea.LocalDate == date && ea.EmployeeAttendanceChecks.All(eac =>
                 eac != null && eac.IsActive && !eac.IsDeleted))))
         ,
     Total_Early_Departures = employee.EmployeeAttendances
    .SelectMany(ea => ea.EmployeeAttendanceChecks)
    .Where(eac =>
        eac != null && eac.IsActive && !eac.IsDeleted &&
        eac.EmployeeAttendance != null &&
        eac.EmployeeAttendance.LocalDate >= (model.FromDate ?? DateTime.MinValue) &&
        eac.EmployeeAttendance.LocalDate <= (model.ToDate ?? DateTime.MaxValue) &&
        eac.Time < eac.EmployeeAttendance.ShiftCheckOutTime
    )
    .Count(),
     Total_Late_Arrivals = employee.EmployeeAttendances
        .SelectMany(ea => ea.EmployeeAttendanceChecks)
        .Count(eac =>
            eac != null && eac.IsActive && !eac.IsDeleted &&
            (eac.EmployeeAttendance != null && eac.EmployeeAttendance.LocalDate >= (model.FromDate ?? DateTime.MinValue)) &&
            (eac.EmployeeAttendance != null && eac.EmployeeAttendance.LocalDate <= (model.ToDate ?? DateTime.MaxValue)) &&
            (eac.EmployeeAttendance != null
            //&&
             //eac.Time > GetLateArrivalThreshold(eac.EmployeeAttendance.ShiftCheckInTime, model.FromDate ?? DateTime.MinValue)
             )
        )
 }).ToListAsync();
            if (!model.NeedToExport) //apply pagination if false
            {
                #region paging
                int skip = PagingHelper.Skip(model.PageNumber, model.PageSize);
                int take = PagingHelper.Take(model.PageSize);
                #region sorting
                var queryOrdered = result.OrderByDescending(s => s.EmployeeId);
                #endregion
                var queryPaged = model.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
                #endregion
                var output = queryPaged.Select(employee => new AttendanceSummaryModelDTO
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeNumber = employee.EmployeeNumber,
                    EmployeeName = employee.EmployeeName,
                    Total_Working_Hours = employee.Total_Working_Hours,
                    Total_Absences = employee.Total_Absences,
                    Total_Early_Departures = employee.Total_Early_Departures,
                    Total_Late_Arrivals = employee.Total_Late_Arrivals
                });
                return new AttendanceSummaryForGridDTO()
                {
                    AttendanceSummaryData = output.ToList(),
                    TotalCount = output.Count()
                };
            }

            return new AttendanceSummaryForGridDTO()
            {
                AttendanceSummaryData = result.ToList(),
                TotalCount = result.Count()
            };
        }


        private static double CalculateTotalWorkingHours(IEnumerable<EmployeeAttendance> attendances, DateTime fromDate, DateTime toDate)
        {
            var checks = attendances
            .Where(ea => ea.EmployeeAttendanceChecks != null)
            .SelectMany(ea => ea.EmployeeAttendanceChecks)
            .Where(eac =>
                eac != null && eac.IsActive && !eac.IsDeleted &&
                (fromDate != DateTime.MinValue || eac.EmployeeAttendance != null && eac.EmployeeAttendance.LocalDate >= fromDate) &&
                (toDate != DateTime.MaxValue || eac.EmployeeAttendance != null && eac.EmployeeAttendance.LocalDate <= toDate)
            );

            var totalWorkingHours = checks
                .GroupBy(eac => eac.EmployeeAttendanceId)
                .Select(g => CalculateWorkingHours(g))
                .Sum();
            return totalWorkingHours;
        }
        private static double CalculateWorkingHours(IEnumerable<EmployeeAttendanceCheck> checks)
        {
            var orderedChecks = checks.OrderBy(e => e.Time).ToList();
            if (orderedChecks.Count < 2)
            {
                return 0;
            }
            // Assuming endTime is always greater than or equal to startTime
            var startTime = orderedChecks.First().Time;
            var endTime = orderedChecks.Last().Time;

            return (endTime - startTime).TotalMinutes;
        }

        TimeOnly GetLateArrivalThreshold(TimeOnly shiftCheckInTime, DateTime fromDate)
        {
            TimeSpan shiftTimeSpan = shiftCheckInTime.ToTimeSpan();
            DateTime lateArrivalThreshold = fromDate.Date.Add(shiftTimeSpan);
            if (lateArrivalThreshold.TimeOfDay < shiftTimeSpan)
            {
                lateArrivalThreshold = lateArrivalThreshold.AddDays(1);
            }
            return TimeOnlyHelper.ToTimeOnly(lateArrivalThreshold.TimeOfDay); 
        }
       

    }





}
