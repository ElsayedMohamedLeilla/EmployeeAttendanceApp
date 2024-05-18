using Dawem.Contract.BusinessLogic.Dawem.Reports;
using Dawem.Contract.Repository.Manager;
using Dawem.Domain.Entities.Attendances;
using Dawem.Domain.Entities.Employees;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Reports.AttendanceSummaryReport;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Reports
{
    public class AttendanceReportBL : IAttendanceSummaryReportBL
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public AttendanceReportBL(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        #region Old Amgad
        public async Task<AttendanceSummaryResponseDTO> OldAmgadGetAttendanceSummary(AttendanceSummaryCritria model)
        {
            var result = await repositoryManager.EmployeeRepository.Get(
     employee =>
         (model.EmployeesIds == null || model.EmployeesIds.Contains(employee.Id)) && // Check if employee is selected
         !employee.IsDeleted &&
         employee.IsActive &&
         employee.CompanyId == 7 /*requestInfo.CompanyId*/ &&

         /*employee.EmployeeAttendances
             .Any(ea =>
                 ea.EmployeeAttendanceChecks
                     .Any(eac =>
                         eac != null && eac.IsActive && !eac.IsDeleted &&
                         ( ea.LocalDate >= model.DateFrom) &&
                         ( ea.LocalDate <= model.DateTo)
                     )
             ) &&*/
         employee.SchedulePlanEmployees
             .Any(spe =>
                 spe.IsActive &&
                 spe.SchedulePlan.DateFrom <= model.DateTo &&
                 spe.SchedulePlan.DateFrom >= model.DateFrom
             )
 ).Select(employee => new AttendanceSummaryModel
 {
     EmployeeId = employee.Id,
     EmployeeNumber = employee.EmployeeNumber,
     EmployeeName = employee.Name,
     WorkingHoursCount = CalculateTotalWorkingHours(employee.EmployeeAttendances, model.DateFrom, model.DateTo) + LeillaKeys.Space + LeillaKeys.Hour,

     AbsencesCount = employee.EmployeeAttendances
         .Select(ea => ea.LocalDate) // Select attendance dates
         .GroupBy(date => date) // Group by attendance date
         .Count(group => !group.Any(date => employee.EmployeeAttendances
             .Any(ea => ea.LocalDate == date && ea.EmployeeAttendanceChecks.All(eac =>
                 eac != null && eac.IsActive && !eac.IsDeleted)))) + LeillaKeys.Space + LeillaKeys.Day
         ,
     EarlyDeparturesCount = employee.EmployeeAttendances
    .SelectMany(ea => ea.EmployeeAttendanceChecks)
    .Where(eac =>
        eac != null && eac.IsActive && !eac.IsDeleted &&
        eac.EmployeeAttendance != null &&
        eac.EmployeeAttendance.LocalDate >= model.DateFrom &&
        eac.EmployeeAttendance.LocalDate <= model.DateTo &&
        eac.FingerPrintDate.TimeOfDay < eac.EmployeeAttendance.ShiftCheckOutTime
    )
    .Count() + LeillaKeys.Space + LeillaKeys.Hour,
     LateArrivalsCount = employee.EmployeeAttendances
        .SelectMany(ea => ea.EmployeeAttendanceChecks)
        .Count(eac =>
            eac != null && eac.IsActive && !eac.IsDeleted &&
            eac.EmployeeAttendance != null && eac.EmployeeAttendance.LocalDate >= model.DateFrom &&
            eac.EmployeeAttendance != null && eac.EmployeeAttendance.LocalDate <= model.DateTo &&
            eac.EmployeeAttendance != null
        //&&
        //eac.Time > GetLateArrivalThreshold(eac.EmployeeAttendance.ShiftCheckInTime, model.FromDate ?? DateTime.MinValue)

        ) + LeillaKeys.Space + LeillaKeys.Hour
 }).ToListAsync();
            if (!model.IsExport) //apply pagination if false
            {
                #region paging
                int skip = PagingHelper.Skip(model.PageNumber, model.PageSize);
                int take = PagingHelper.Take(model.PageSize);
                #region sorting
                var queryOrdered = result.OrderByDescending(s => s.EmployeeId);
                #endregion
                var queryPaged = model.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
                #endregion
                var output = queryPaged.Select(employee => new AttendanceSummaryModel
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeNumber = employee.EmployeeNumber,
                    EmployeeName = employee.EmployeeName,
                    WorkingHoursCount = employee.WorkingHoursCount,
                    AbsencesCount = employee.AbsencesCount,
                    EarlyDeparturesCount = employee.EarlyDeparturesCount,
                    LateArrivalsCount = employee.LateArrivalsCount
                });
                return new AttendanceSummaryResponseDTO()
                {
                    AttendanceSmmaries = output.ToList(),
                    TotalCount = output.Count()
                };
            }

            return new AttendanceSummaryResponseDTO()
            {
                AttendanceSmmaries = result.ToList(),
                TotalCount = result.Count()
            };
        }
        private static decimal CalculateTotalWorkingHours(IEnumerable<EmployeeAttendance> attendances, DateTime fromDate, DateTime toDate)
        {
            var checks = attendances
            .Where(ea => ea.EmployeeAttendanceChecks != null)
            .SelectMany(ea => ea.EmployeeAttendanceChecks)
            .Where(eac =>
                eac != null && eac.IsActive && !eac.IsDeleted &&
                (fromDate != DateTime.MinValue || eac.EmployeeAttendance != null && eac.EmployeeAttendance.LocalDate >= fromDate) &&
                (toDate != DateTime.MaxValue || eac.EmployeeAttendance != null && eac.EmployeeAttendance.LocalDate <= toDate)
            );

            var totalWorkingHours = (decimal)checks
                .GroupBy(eac => eac.EmployeeAttendanceId)
                .Select(g => CalculateWorkingHours(g))
                .Sum();
            return totalWorkingHours;
        }
        private static double CalculateWorkingHours(IEnumerable<EmployeeAttendanceCheck> checks)
        {
            var orderedChecks = checks.OrderBy(e => e.FingerPrintDate).ToList();
            if (orderedChecks.Count < 2)
            {
                return 0;
            }
            // Assuming endTime is always greater than or equal to startTime
            var startDateTime = orderedChecks.First().FingerPrintDate;
            var endDateTime = orderedChecks.Last().FingerPrintDate;

            return (endDateTime - startDateTime).TotalMinutes;
        }
        TimeOnly GetLateArrivalThreshold(TimeOnly shiftCheckInTime, DateTime fromDate)
        {
            TimeSpan shiftTimeSpan = shiftCheckInTime.ToTimeSpan();
            DateTime lateArrivalThreshold = fromDate.Date.Add(shiftTimeSpan);
            if (lateArrivalThreshold.TimeOfDay < shiftTimeSpan)
            {
                lateArrivalThreshold = lateArrivalThreshold.AddDays(1);
            }
            return TimeHelper.ToTimeOnly(lateArrivalThreshold.TimeOfDay);
        }
        #endregion

        #region Old Elsayed
        public async Task<AttendanceSummaryResponseDTO> OldElsayedGetAttendanceSummary(AttendanceSummaryCritria criteria)
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

                ShouldAttendCount = ReportsHelper.GetShouldAttendCount(employee.Schedule.ScheduleDays
                .Where(es => !es.IsDeleted && es.ShiftId > 0)
                .Select(sd => sd.WeekDay).ToList(), criteria.DateFrom, criteria.DateTo) +
                    LeillaKeys.Space + TranslationHelper.GetTranslation(LeillaKeys.Day, requestInfo.Lang),


                ActualAttendCount = employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                ea.EmployeeAttendanceChecks != null && ea.LocalDate.Date >= criteria.DateFrom.Date
                && ea.LocalDate.Date <= criteria.DateTo.Date &&
                ea.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted)).Count() + LeillaKeys.Space + LeillaKeys.Day,


                LateArrivalsCount = ReportsHelper.SumListOfNumber(employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                ea.EmployeeAttendanceChecks != null && ea.LocalDate.Date >= criteria.DateFrom.Date
                && ea.LocalDate.Date <= criteria.DateTo.Date &&
                ea.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckIn) &&
                ea.EmployeeAttendanceChecks.FirstOrDefault(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckIn).FingerPrintDate.TimeOfDay > ea.ShiftCheckInTime)
                .Select(ea => EF.Functions.DateDiffMinute((DateTime)(object)ea.ShiftCheckInTime,
                (DateTime)(object)ea.EmployeeAttendanceChecks
                .FirstOrDefault(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckIn).FingerPrintDate.TimeOfDay) / 60).ToList()) + LeillaKeys.Space + LeillaKeys.Hour,


                EarlyDeparturesCount = ReportsHelper.SumListOfNumber(employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                ea.EmployeeAttendanceChecks != null && ea.LocalDate.Date >= criteria.DateFrom.Date
                && ea.LocalDate.Date <= criteria.DateTo.Date &&
                ea.EmployeeAttendanceChecks
                .FirstOrDefault(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckOut).FingerPrintDate.TimeOfDay < ea.ShiftCheckOutTime &&
                ea.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckOut))
                .Select(ea => EF.Functions.DateDiffMinute((DateTime)(object)ea.EmployeeAttendanceChecks
                .FirstOrDefault(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckOut).FingerPrintDate.TimeOfDay,
                (DateTime)(object)ea.ShiftCheckOutTime) / 60).ToList()) + LeillaKeys.Space + LeillaKeys.Hour,


                WorkingHoursCount = ReportsHelper.SumListOfNumber(employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                ea.EmployeeAttendanceChecks != null && ea.LocalDate.Date >= criteria.DateFrom.Date
                && ea.LocalDate.Date <= criteria.DateTo.Date &&
                ea.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckIn) &&
                ea.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckOut))
                .Select(ea => EF.Functions.DateDiffMinute((DateTime)(object)ea.EmployeeAttendanceChecks
                .FirstOrDefault(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckIn).FingerPrintDate.TimeOfDay,
                (DateTime)(object)ea.EmployeeAttendanceChecks
                .FirstOrDefault(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckOut).FingerPrintDate.TimeOfDay) / 60).ToList()) + LeillaKeys.Space + LeillaKeys.Hour,


                AbsencesCount = ReportsHelper.GetShouldAttendCount(employee.Schedule.ScheduleDays
                .Where(es => !es.IsDeleted && es.ShiftId > 0)
                .Select(sd => sd.WeekDay).ToList(), criteria.DateFrom, criteria.DateTo) -  // - vacations in this period
                employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                ea.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted) && ea.LocalDate.Date >= criteria.DateFrom.Date
                && ea.LocalDate.Date <= criteria.DateTo.Date).Count() + LeillaKeys.Space + LeillaKeys.Day,



            }).ToListAsync();

            return new AttendanceSummaryResponseDTO
            {
                AttendanceSmmaries = employeesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        #endregion

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

            #region Get Employees Data

            var employeesList = await queryPaged.Select(employee => new
            {
                employee.Id,
                employee.EmployeeNumber,
                employee.Name,

                Vacations = employee.EmployeeRequests.Any(er => !er.IsDeleted &&
                er.Status == RequestStatus.Accepted && er.Type == RequestType.Vacation) ?
                employee.EmployeeRequests.Where(er => !er.IsDeleted && er.Type == RequestType.Vacation &&
                er.Status == RequestStatus.Accepted &&
                (er.Date.Date >= criteria.DateFrom && er.RequestVacation.DateTo.Date <= criteria.DateTo ||
                er.Date.Date <= criteria.DateFrom && er.RequestVacation.DateTo.Date >= criteria.DateFrom ||
                er.Date.Date <= criteria.DateTo && er.RequestVacation.DateTo.Date >= criteria.DateTo))
                .Select(ev => new
                {
                    DateFrom = ev.Date < criteria.DateFrom ? criteria.DateFrom : ev.Date,
                    DateTo = ev.RequestVacation.DateTo > criteria.DateTo ? criteria.DateTo : ev.Date,
                    DaysCount = (int)((ev.RequestVacation.DateTo > criteria.DateTo ? criteria.DateTo : ev.Date)
                    - (ev.Date < criteria.DateFrom ? criteria.DateFrom : ev.Date)).TotalDays + 1
                }).ToList() : null,

                EmployeeAttendances = employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                ea.LocalDate.Date >= criteria.DateFrom.Date
                && ea.LocalDate.Date <= criteria.DateTo.Date &&
                ea.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted &&
                eac.FingerPrintType == FingerPrintType.CheckIn && eac.FingerPrintDate.TimeOfDay < ea.ShiftCheckOutTime) &&
                 ea.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted &&
                (eac.FingerPrintType == FingerPrintType.CheckOut && eac.FingerPrintDate.TimeOfDay > ea.ShiftCheckInTime)))
                .Select(ea => new
                {
                    ea.Id,
                    //ea.LocalDate,
                    ea.ShiftCheckInTime,
                    ea.ShiftCheckOutTime,
                    ea.AllowedMinutes,

                    ea.TotalWorkingHours,
                    ea.TotalEarlyDeparturesHours,
                    ea.TotalLateArrivalsHours,
                    ea.TotalOverTimeHours

                }).ToList(),

                Schedule = employee.Schedule.ScheduleDays.Any(es => !es.IsDeleted && es.ShiftId > 0) ? new
                {
                    ScheduleDays = employee.Schedule.ScheduleDays.Where(es => !es.IsDeleted && es.ShiftId > 0)
                    .Select(es => new
                    {
                        es.WeekDay,

                    }).ToList()
                } : null,

                SchedulePlans = employee.Company.SchedulePlans.Any(sp => !sp.IsDeleted) ?

                employee.Company.SchedulePlans
                .Where(sp => !sp.IsDeleted &&
                (sp.SchedulePlanEmployee != null && !sp.SchedulePlanEmployee.IsDeleted &&
                sp.SchedulePlanEmployee.EmployeeId == employee.Id || employee.DepartmentId != null &&
                sp.SchedulePlanDepartment != null && !sp.SchedulePlanDepartment.IsDeleted &&
                sp.SchedulePlanDepartment.DepartmentId == employee.DepartmentId || employee.EmployeeGroups.Any(eg => !eg.IsDeleted) &&
                sp.SchedulePlanGroup != null && !sp.SchedulePlanGroup.IsDeleted &&
                employee.EmployeeGroups.Any(eg => !eg.IsDeleted && eg.GroupId == sp.SchedulePlanGroup.GroupId)) && sp.DateFrom.Date <= criteria.DateTo.Date &&
                (sp.DateFrom.Date >= criteria.DateFrom.Date ||
                sp.DateFrom.Date == employee.Company.SchedulePlans.Select(csp => csp.DateFrom.Date).Where(date => date < criteria.DateFrom.Date).Max()))
                .Select(sp => new
                {
                    sp.DateFrom,
                    WeekDays = sp.Schedule.ScheduleDays
                    .Where(sd => !sd.IsDeleted && sd.ShiftId > 0)
                    .Select(sd => sd.WeekDay).ToList()
                }).ToList() : null,

            }).ToListAsync();

            #endregion

            #region Process Employees Data

            var attendanceSmmaries = new List<AttendanceSummaryModel>();

            foreach (var employee in employeesList)
            {
                #region First Model

                var shouldAttendNumber = 0;
                var model = new AttendanceSummaryModel
                {
                    EmployeeId = employee.Id,
                    EmployeeName = employee.Name,
                    EmployeeNumber = employee.EmployeeNumber,

                    ActualAttendCount = employee.EmployeeAttendances.Count() + LeillaKeys.Space + TranslationHelper.GetTranslation(LeillaKeys.Day, requestInfo.Lang),

                    LateArrivalsCount = Math.Round(employee.EmployeeAttendances.
                    Where(ea => ea.TotalLateArrivalsHours > 0).Sum(ea => ea.TotalLateArrivalsHours.Value), 2) +
                    LeillaKeys.Space + TranslationHelper.GetTranslation(LeillaKeys.Hour, requestInfo.Lang),

                    EarlyDeparturesCount = Math.Round(employee.EmployeeAttendances.
                    Where(ea => ea.TotalEarlyDeparturesHours > 0).Sum(ea => ea.TotalEarlyDeparturesHours.Value), 2) +
                    LeillaKeys.Space + TranslationHelper.GetTranslation(LeillaKeys.Hour, requestInfo.Lang),

                    WorkingHoursCount = Math.Round(employee.EmployeeAttendances.
                    Where(ea => ea.TotalWorkingHours > 0).Sum(ea => ea.TotalWorkingHours.Value), 2) +
                    LeillaKeys.Space + TranslationHelper.GetTranslation(LeillaKeys.Hour, requestInfo.Lang),

                    OverTimeCount = Math.Round(employee.EmployeeAttendances.
                    Where(ea => ea.TotalOverTimeHours > 0).Sum(ea => ea.TotalOverTimeHours.Value), 2) +
                    LeillaKeys.Space + TranslationHelper.GetTranslation(LeillaKeys.Hour, requestInfo.Lang),

                    VacationsCount = (employee.Vacations != null ? employee.Vacations.Count : 0)
                    + LeillaKeys.Space + TranslationHelper.GetTranslation(LeillaKeys.Day, requestInfo.Lang),

                };

                #endregion

                #region Handle Plan Props

                if (employee.SchedulePlans != null && employee.SchedulePlans.Count > 0)
                {
                    #region Prepare Dates Chunks

                    var schedulePlans = employee.SchedulePlans.OrderBy(sp => sp.DateFrom).ToList();
                    var schedule = employee.Schedule;

                    if (schedulePlans.First().DateFrom > criteria.DateFrom && schedule != null)
                    {
                        var periodDatesChunk = new AttendanceSummaryHelperModel
                        {
                            DateFrom = criteria.DateFrom,
                            DateTo = schedulePlans.First().DateFrom.AddDays(-1),
                            WeekDays = schedule.ScheduleDays.Select(sd => sd.WeekDay).ToList()
                        };

                        shouldAttendNumber += ReportsHelper
                            .GetShouldAttendCount(periodDatesChunk.WeekDays, periodDatesChunk.DateFrom, periodDatesChunk.DateTo);
                    }

                    for (int i = 0; i < schedulePlans.Count; i++)
                    {
                        var schedulePlan = schedulePlans[i];
                        var periodDatesChunk = new AttendanceSummaryHelperModel
                        {
                            DateFrom = schedulePlan.DateFrom < criteria.DateFrom ? criteria.DateFrom : schedulePlan.DateFrom,
                            DateTo = schedulePlans.Count == i + 1 ? criteria.DateTo : schedulePlans[i + 1].DateFrom.AddDays(-1),
                            WeekDays = schedulePlan.WeekDays
                        };

                        shouldAttendNumber += ReportsHelper
                            .GetShouldAttendCount(periodDatesChunk.WeekDays, periodDatesChunk.DateFrom, periodDatesChunk.DateTo);
                    }

                    #endregion

                    model.AbsencesCount += shouldAttendNumber -
                                  (employee.Vacations != null ? employee.Vacations.Sum(ev => ev.DaysCount) : 0) -
                                  employee.EmployeeAttendances.Count() + LeillaKeys.Space + TranslationHelper.GetTranslation(LeillaKeys.Day, requestInfo.Lang);
                }
                else
                {
                    shouldAttendNumber = ReportsHelper.GetShouldAttendCount(employee.Schedule.ScheduleDays
                              .Select(sd => sd.WeekDay).ToList(), criteria.DateFrom, criteria.DateTo);

                    model.AbsencesCount = shouldAttendNumber -
                              (employee.Vacations != null ? employee.Vacations.Sum(ev => ev.DaysCount) : 0) -
                              employee.EmployeeAttendances.Count() + LeillaKeys.Space + TranslationHelper.GetTranslation(LeillaKeys.Day, requestInfo.Lang);

                }

                model.ShouldAttendCount = shouldAttendNumber +
                    LeillaKeys.Space + TranslationHelper.GetTranslation(LeillaKeys.Day, requestInfo.Lang);

                #endregion

                attendanceSmmaries.Add(model);
            }

            #endregion

            return new AttendanceSummaryResponseDTO
            {
                AttendanceSmmaries = attendanceSmmaries,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
    }
}

