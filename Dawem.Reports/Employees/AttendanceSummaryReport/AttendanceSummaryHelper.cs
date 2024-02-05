using Dawem.Contract.Repository.Manager;
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
                      (model.EmployeeId == null || employee.Id == model.EmployeeId) && //check if employee is selected
                     !employee.IsDeleted &&
                     employee.IsActive &&
                     employee.CompanyId == requestInfo.CompanyId &&
                     employee.EmployeeAttendances
                         .Any(ea =>
                         ea.EmployeeAttendanceChecks
                         .Any(eac =>
                         eac != null && eac.IsActive && !eac.IsDeleted &&
                         (!model.FromDate.HasValue || ea.LocalDate >= model.FromDate.Value) &&
                         (!model.ToDate.HasValue || ea.LocalDate <= model.ToDate.Value)))
                     ).Select(employee => new AttendanceSummaryModelDTO
                     {
                         EmployeeId = employee.Id,
                         EmployeeNumber = employee.EmployeeNumber,
                         EmployeeName = employee.Name,
                         Total_Working_Hours = employee.EmployeeAttendances
                         .SelectMany(ea => ea.EmployeeAttendanceChecks)
                         .Where(eac =>
                         eac != null && eac.IsActive && !eac.IsDeleted &&
                         (model.FromDate != DateTime.MinValue || eac.EmployeeAttendance != null && eac.EmployeeAttendance.LocalDate >= model.FromDate.Value) &&
                         (model.ToDate != DateTime.MinValue || eac.EmployeeAttendance != null && eac.EmployeeAttendance.LocalDate <= model.ToDate.Value)
                         )
                        .Sum(eac => (eac.EmployeeAttendance.ShiftCheckOutTime - eac.EmployeeAttendance.ShiftCheckInTime).TotalMinutes),
                         Total_Absences = employee.SchedulePlanEmployees
                         .Where(spe =>
                         spe.SchedulePlanId != 0 &&
                         spe.SchedulePlan.DateFrom >= (model.FromDate ?? DateTime.MinValue) &&
                         spe.SchedulePlan.DateFrom <= (model.ToDate ?? DateTime.MaxValue) &&
                         !employee.EmployeeAttendances.Any(ea =>
                             ea.LocalDate >= (model.FromDate ?? DateTime.MinValue) &&
                             ea.LocalDate <= (model.ToDate ?? DateTime.MaxValue) &&
                             ea.EmployeeAttendanceChecks.Any()
                         )
                         )
                        .Sum(spe => 1),
                         Total_Early_Departures = employee.EmployeeAttendances
                          .SelectMany(ea => ea.EmployeeAttendanceChecks)
                          .Where(eac =>
                              eac != null && eac.IsActive && !eac.IsDeleted &&
                              (eac.EmployeeAttendance != null && eac.EmployeeAttendance.LocalDate >= (model.FromDate ?? DateTime.MinValue)) &&
                              (eac.EmployeeAttendance != null && eac.EmployeeAttendance.LocalDate <= (model.ToDate ?? DateTime.MaxValue)) &&
                              (eac.EmployeeAttendance != null &&
                               eac.EmployeeAttendance.ShiftCheckOutTime < eac.EmployeeAttendance.EmployeeAttendanceChecks.Max(eac => eac.Time))
                          )
                          .Count(),
                         Total_Late_Arrivals = employee.EmployeeAttendances
                           .SelectMany(ea => ea.EmployeeAttendanceChecks)
                           .Count(eac =>
                               eac != null && eac.IsActive && !eac.IsDeleted &&
                               (eac.EmployeeAttendance != null && eac.EmployeeAttendance.LocalDate >= (model.FromDate ?? DateTime.MinValue)) &&
                               (eac.EmployeeAttendance != null && eac.EmployeeAttendance.LocalDate <= (model.ToDate ?? DateTime.MaxValue)) &&
                               (eac.EmployeeAttendance != null &&
                                eac.EmployeeAttendance.ShiftCheckOutTime < eac.EmployeeAttendance.EmployeeAttendanceChecks.Min(eac => eac.Time))
                           )
                     }).ToListAsync();

            if (!model.NeedToExport) //apply pagination if false
            {
                #region paging
                int skip = PagingHelper.Skip(model.PageNumber, model.PageSize);
                int take = PagingHelper.Take(model.PageSize);
                #region sorting
                result.OrderByDescending(s => s.EmployeeId);
                #endregion
                var queryPaged = model.PagingEnabled ? result.Skip(skip).Take(take) : result;
                #endregion
            }

            return new AttendanceSummaryForGridDTO()
            {
                AttendanceSummaryData = result,
                TotalCount = result.Count
            };
        }
    }
}
