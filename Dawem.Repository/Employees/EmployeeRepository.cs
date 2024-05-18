using Dawem.Contract.Repository.Employees;
using Dawem.Contract.Repository.UserManagement;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Schedules;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Reports.AttendanceSummaryReport;
using Dawem.Models.DTOs.Dawem.Employees.Employees;
using Dawem.Models.DTOs.Dawem.Generic;
using DocumentFormat.OpenXml.InkML;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Repository.Employees
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly RequestInfo requestInfo;
        private readonly IUserRepository userRepository;

        public EmployeeRepository(IUserRepository _userRepository, IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
            userRepository =_userRepository ;
        }
        public IQueryable<Employee> GetAsQueryable(GetEmployeesCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Employee>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Employee>(true);

            predicate = predicate.And(e => e.CompanyId == requestInfo.CompanyId);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.Department.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.JobTitle.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.Email.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.MobileNumber.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.Address.ToLower().Trim().StartsWith(criteria.FreeText));

                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }
            if (criteria.Id != null)
            {
                predicate = predicate.And(e => e.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(e => criteria.Ids.Contains(e.Id));
            }
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(e => e.Code == criteria.Code);
            }
            if (criteria.DepartmentId is not null)
            {
                predicate = predicate.And(e => e.DepartmentId == criteria.DepartmentId);
            }
            if (criteria.JobTitleId is not null)
            {
                predicate = predicate.And(e => e.JobTitleId == criteria.JobTitleId);
            }
            if (criteria.ScheduleId is not null)
            {
                predicate = predicate.And(e => e.ScheduleId == criteria.ScheduleId);
            }
            if (criteria.DirectManagerId is not null)
            {
                predicate = predicate.And(e => e.DirectManagerId == criteria.DirectManagerId);
            }
            if (criteria.EmployeeNumber is not null)
            {
                predicate = predicate.And(e => e.EmployeeNumber == criteria.EmployeeNumber);
            }
            if (criteria.Status != null)
            {
                var clientLocalDate = requestInfo.LocalDateTime;

                switch (criteria.Status.Value)
                {
                    case FilterEmployeeStatus.Available:
                        predicate = predicate.And(employee => !employee.EmployeeTasks.Any(task => !task.IsDeleted && !task.RequestTask.Request.IsDeleted
                        && (task.RequestTask.Request.Status == RequestStatus.Accepted || task.RequestTask.Request.Status == RequestStatus.Pending)
                        && clientLocalDate.Date >= task.RequestTask.Request.Date && clientLocalDate.Date <= task.RequestTask.DateTo)
                        &&
                        !employee.EmployeeRequests.Any(request => !request.IsDeleted && !request.RequestTask.Request.IsDeleted
                        && (request.RequestTask.Request.Status == RequestStatus.Accepted || request.RequestTask.Request.Status == RequestStatus.Pending)
                        && (request.RequestTask.Request.Type == RequestType.Assignment || request.RequestTask.Request.Type == RequestType.Vacation)
                        && clientLocalDate.Date >= request.Date.Date
                        && clientLocalDate.Date <= request.Date.Date));

                        break;
                    case FilterEmployeeStatus.InTaskOrAssignment:
                        predicate = predicate.And(employee => employee.EmployeeTasks.Any(task => !task.IsDeleted
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
                        && clientLocalDate.Date <= request.RequestAssignment.DateTo.Date));

                        break;
                    case FilterEmployeeStatus.InVacationOrOutside:
                        predicate = predicate.And(employee =>
                        employee.EmployeeRequests.Any(request => !request.IsDeleted && !request.RequestVacation.IsDeleted
                        && (request.Status == RequestStatus.Accepted ||
                        request.Status == RequestStatus.Pending)
                        && request.Type == RequestType.Vacation
                        && clientLocalDate.Date >= request.Date.Date
                        && clientLocalDate.Date <= request.RequestVacation.DateTo.Date));

                        break;
                    default:
                        break;
                }
            }
            if (criteria.IsFreeEmployee)
            {
                criteria.FreeEmployeeIds = userRepository.GetEmployeeIdsNotConnectedToUser();
                if (criteria.FreeEmployeeIds != null && criteria.FreeEmployeeIds.Count() > 0)
                {
                    predicate = predicate.And(e => criteria.FreeEmployeeIds.Contains(e.Id));
                }
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
        public IQueryable<Employee> GetAsQueryableForAttendanceSummary(AttendanceSummaryCritria criteria)
        {
            var predicate = PredicateBuilder.New<Employee>(a => !a.IsDeleted);
            predicate = predicate.And(e => e.CompanyId == requestInfo.CompanyId);

            var inner = PredicateBuilder.New<Employee>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.Department.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.JobTitle.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.Schedule.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.DirectManager.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.Email.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.MobileNumber.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.Address.ToLower().Trim().StartsWith(criteria.FreeText));

                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }

            predicate = predicate.And(employee =>

            (employee.ScheduleId > 0 && employee.Schedule.ScheduleDays != null &&
            employee.Schedule.ScheduleDays.Any(es => !es.IsDeleted && es.ShiftId > 0)) ||

            employee.Company.SchedulePlans
                .Any(sp => !sp.IsDeleted &&
                ((sp.SchedulePlanEmployee != null && !sp.SchedulePlanEmployee.IsDeleted &&
                sp.SchedulePlanEmployee.EmployeeId == employee.Id) || (employee.DepartmentId != null &&
                sp.SchedulePlanDepartment != null && !sp.SchedulePlanDepartment.IsDeleted &&
                sp.SchedulePlanDepartment.DepartmentId == employee.DepartmentId) || (employee.EmployeeGroups.Any(eg => !eg.IsDeleted) &&
                sp.SchedulePlanGroup != null && !sp.SchedulePlanGroup.IsDeleted &&
                employee.EmployeeGroups.Any(eg => !eg.IsDeleted && eg.GroupId == sp.SchedulePlanGroup.GroupId))) && sp.DateFrom.Date <= criteria.DateTo.Date &&
                (sp.DateFrom.Date >= criteria.DateFrom.Date ||
                sp.DateFrom.Date == employee.Company.SchedulePlans.Select(csp => csp.DateFrom.Date).Where(date => date < criteria.DateFrom.Date).Max())));

            if (criteria.Id != null)
            {
                predicate = predicate.And(e => e.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(e => criteria.Ids.Contains(e.Id));
            }
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(e => e.Code == criteria.Code);
            }
            if (criteria.EmployeesIds is not null && criteria.EmployeesIds.Count > 0)
            {
                predicate = predicate.And(e => criteria.EmployeesIds.Contains(e.Id));
            }
            if (criteria.FilterType is not null)
            {
                switch (criteria.FilterType.Value)
                {
                    case ReportFilterType.ActualAttend:

                        if (criteria.FilterTypeFrom > 0)
                            predicate = predicate.And(e => e.EmployeeAttendances != null &&
                            e.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                            ea.LocalDate.Date >= criteria.DateFrom.Date &&
                            ea.LocalDate.Date <= criteria.DateTo.Date &&
                            ea.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted &&
                            (eac.FingerPrintType == FingerPrintType.CheckIn || eac.FingerPrintType == FingerPrintType.CheckOut))).
                            Count() >= criteria.FilterTypeFrom);

                        if (criteria.FilterTypeTo > 0)
                            predicate = predicate.And(e => e.EmployeeAttendances != null &&
                            e.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                            ea.LocalDate.Date >= criteria.DateFrom.Date &&
                            ea.LocalDate.Date <= criteria.DateTo.Date &&
                            ea.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted &&
                            (eac.FingerPrintType == FingerPrintType.CheckIn || eac.FingerPrintType == FingerPrintType.CheckOut))).
                            Count() >= criteria.FilterTypeTo);
                        break;
                    case ReportFilterType.EarlyDepartures:

                        if (criteria.FilterTypeFrom > 0)
                            predicate = predicate.And(employee => employee.EmployeeAttendances != null &&
                            employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                            ea.LocalDate.Date >= criteria.DateFrom.Date &&
                            ea.LocalDate.Date <= criteria.DateTo.Date &&
                            ea.TotalEarlyDeparturesHours > 0).
                            Sum(e => e.TotalEarlyDeparturesHours) >= criteria.FilterTypeFrom);

                        if (criteria.FilterTypeTo > 0)
                            predicate = predicate.And(employee => employee.EmployeeAttendances != null &&
                            employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                            ea.LocalDate.Date >= criteria.DateFrom.Date &&
                            ea.LocalDate.Date <= criteria.DateTo.Date &&
                            ea.TotalEarlyDeparturesHours > 0).
                            Sum(e => e.TotalEarlyDeparturesHours) <= criteria.FilterTypeTo);

                        break;
                    case ReportFilterType.LateArrivals:

                        if (criteria.FilterTypeFrom > 0)
                            predicate = predicate.And(employee => employee.EmployeeAttendances != null &&
                            employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                            ea.LocalDate.Date >= criteria.DateFrom.Date &&
                            ea.LocalDate.Date <= criteria.DateTo.Date &&
                            ea.TotalLateArrivalsHours > 0).
                            Sum(e => e.TotalLateArrivalsHours) >= criteria.FilterTypeFrom);

                        if (criteria.FilterTypeTo > 0)
                            predicate = predicate.And(employee => employee.EmployeeAttendances != null &&
                            employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                            ea.LocalDate.Date >= criteria.DateFrom.Date &&
                            ea.LocalDate.Date <= criteria.DateTo.Date &&
                            ea.TotalLateArrivalsHours > 0).
                            Sum(e => e.TotalLateArrivalsHours) <= criteria.FilterTypeTo);

                        break;
                    case ReportFilterType.OverTime:

                        if (criteria.FilterTypeFrom > 0)
                            predicate = predicate.And(employee => employee.EmployeeAttendances != null &&
                            employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                            ea.LocalDate.Date >= criteria.DateFrom.Date &&
                            ea.LocalDate.Date <= criteria.DateTo.Date &&
                            ea.TotalOverTimeHours > 0).
                            Sum(e => e.TotalOverTimeHours) >= criteria.FilterTypeFrom);

                        if (criteria.FilterTypeTo > 0)
                            predicate = predicate.And(employee => employee.EmployeeAttendances != null &&
                            employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                            ea.LocalDate.Date >= criteria.DateFrom.Date &&
                            ea.LocalDate.Date <= criteria.DateTo.Date &&
                            ea.TotalOverTimeHours > 0).
                            Sum(e => e.TotalOverTimeHours) <= criteria.FilterTypeTo);

                        break;
                    case ReportFilterType.Vacations:

                        if (criteria.FilterTypeFrom > 0)
                            predicate = predicate.And(employee => employee.EmployeeRequests != null &&
                            employee.EmployeeRequests.Any(er => !er.IsDeleted &&
                            er.Status == RequestStatus.Accepted && er.Type == RequestType.Vacation) &&
                            employee.EmployeeRequests.Where(er => !er.IsDeleted && er.Type == RequestType.Vacation &&
                            er.Status == RequestStatus.Accepted &&
                            (er.Date.Date >= criteria.DateFrom && er.RequestVacation.DateTo.Date <= criteria.DateTo ||
                            er.Date.Date <= criteria.DateFrom && er.RequestVacation.DateTo.Date >= criteria.DateFrom ||
                            er.Date.Date <= criteria.DateTo && er.RequestVacation.DateTo.Date >= criteria.DateTo)).
                            Select(ev => EF.Functions.DateDiffDay(ev.Date < criteria.DateFrom ? criteria.DateFrom : ev.Date,
                            ev.RequestVacation.DateTo > criteria.DateTo ? criteria.DateTo : ev.Date) + 1
                            ).Sum() >= criteria.FilterTypeFrom);

                        if (criteria.FilterTypeTo > 0)
                            predicate = predicate.And(employee => employee.EmployeeRequests != null &&
                            employee.EmployeeRequests.Any(er => !er.IsDeleted &&
                            er.Status == RequestStatus.Accepted && er.Type == RequestType.Vacation) &&
                            employee.EmployeeRequests.Where(er => !er.IsDeleted && er.Type == RequestType.Vacation &&
                            er.Status == RequestStatus.Accepted &&
                            (er.Date.Date >= criteria.DateFrom && er.RequestVacation.DateTo.Date <= criteria.DateTo ||
                            er.Date.Date <= criteria.DateFrom && er.RequestVacation.DateTo.Date >= criteria.DateFrom ||
                            er.Date.Date <= criteria.DateTo && er.RequestVacation.DateTo.Date >= criteria.DateTo)).
                            Select(ev => EF.Functions.DateDiffDay(ev.Date < criteria.DateFrom ? criteria.DateFrom : ev.Date,
                            ev.RequestVacation.DateTo > criteria.DateTo ? criteria.DateTo : ev.Date) + 1
                            ).Sum() <= criteria.FilterTypeTo);

                        break;
                    case ReportFilterType.WorkingHours:

                        if (criteria.FilterTypeFrom > 0)
                            predicate = predicate.And(employee => employee.EmployeeAttendances != null &&
                            employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                            ea.LocalDate.Date >= criteria.DateFrom.Date &&
                            ea.LocalDate.Date <= criteria.DateTo.Date &&
                            ea.TotalWorkingHours > 0).
                            Sum(e => e.TotalWorkingHours) >= criteria.FilterTypeFrom);

                        if (criteria.FilterTypeTo > 0)
                            predicate = predicate.And(employee => employee.EmployeeAttendances != null &&
                            employee.EmployeeAttendances.Where(ea => !ea.IsDeleted &&
                            ea.LocalDate.Date >= criteria.DateFrom.Date &&
                            ea.LocalDate.Date <= criteria.DateTo.Date &&
                            ea.TotalWorkingHours > 0).
                            Sum(e => e.TotalWorkingHours) <= criteria.FilterTypeTo);

                        break;
                    default:
                        break;
                }
            }
            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }

    }
}
