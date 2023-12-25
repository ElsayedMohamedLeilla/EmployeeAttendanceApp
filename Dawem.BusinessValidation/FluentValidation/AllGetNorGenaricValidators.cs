using Dawem.Models.Criteria.Core;
using Dawem.Models.Criteria.Lookups;
using Dawem.Models.Criteria.Others;
using Dawem.Models.Dtos.Attendances;
using Dawem.Models.Dtos.Dashboard;
using Dawem.Models.Dtos.Employees.AssignmentTypes;
using Dawem.Models.Dtos.Employees.Departments;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Employees.HolidayTypes;
using Dawem.Models.Dtos.Employees.JobTitles;
using Dawem.Models.Dtos.Employees.TaskTypes;
using Dawem.Models.Dtos.Employees.Users;
using Dawem.Models.Dtos.Requests;
using Dawem.Models.Dtos.Requests.Assignments;
using Dawem.Models.Dtos.Requests.Justifications;
using Dawem.Models.Dtos.Requests.Permissions;
using Dawem.Models.Dtos.Requests.Tasks;
using Dawem.Models.Dtos.Requests.Vacations;
using Dawem.Models.Dtos.Schedules.SchedulePlanBackgroundJobLogs;
using Dawem.Models.Dtos.Schedules.SchedulePlans;
using Dawem.Models.Dtos.Schedules.Schedules;
using Dawem.Models.Dtos.Schedules.ShiftWorkingTimes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation
{
    public class GetDepartmentsCriteriaValidator : AbstractValidator<GetDepartmentsCriteria>
    {
        public GetDepartmentsCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetDepartmentsForTreeCriteriaValidator : AbstractValidator<GetDepartmentsForTreeCriteria>
    {
        public GetDepartmentsForTreeCriteriaValidator()
        {
            Include(new GetGenaricValidator());

            RuleFor(model => model.ParentId)
                .NotNull()
                .When(m => !m.IsBaseParent)
                .WithMessage(LeillaKeys.SorryYouMustEnterParentId);
        }
    }
    public class GetDepartmentsInformationsCriteriaValidator : AbstractValidator<GetDepartmentsInformationsCriteria>
    {
        public GetDepartmentsInformationsCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetBestEmployeesCriteriaValidator : AbstractValidator<GetBestEmployeesCriteria>
    {
        public GetBestEmployeesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetEmployeesAttendancesStatusCriteriaValidator : AbstractValidator<GetEmployeesAttendancesStatusCriteria>
    {
        public GetEmployeesAttendancesStatusCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetEmployeesCriteriaValidator : AbstractValidator<GetEmployeesCriteria>
    {
        public GetEmployeesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetSchedulePlanBackgroundJobLogsCriteriaValidator : AbstractValidator<GetSchedulePlanBackgroundJobLogsCriteria>
    {
        public GetSchedulePlanBackgroundJobLogsCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetSchedulePlansCriteriaValidator : AbstractValidator<GetSchedulePlansCriteria>
    {
        public GetSchedulePlansCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetSchedulesCriteriaValidator : AbstractValidator<GetSchedulesCriteria>
    {
        public GetSchedulesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetShiftWorkingTimesCriteriaValidator : AbstractValidator<GetShiftWorkingTimesCriteria>
    {
        public GetShiftWorkingTimesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetGroupCriteriaValidator : AbstractValidator<GetZoneCriteria>
    {
        public GetGroupCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetAssignmentTypesCriteriaValidator : AbstractValidator<GetAssignmentTypesCriteria>
    {
        public GetAssignmentTypesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetJustificationsTypesCriteriaValidator : AbstractValidator<GetJustificationsTypesCriteria>
    {
        public GetJustificationsTypesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetPermissionsTypesCriteriaValidator : AbstractValidator<GetPermissionsTypesCriteria>
    {
        public GetPermissionsTypesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetVacationsTypesCriteriaValidator : AbstractValidator<GetVacationsTypesCriteria>
    {
        public GetVacationsTypesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetHolidayTypesCriteriaValidator : AbstractValidator<GetHolidayTypesCriteria>
    {
        public GetHolidayTypesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetJobTitlesCriteriaValidator : AbstractValidator<GetJobTitlesCriteria>
    {
        public GetJobTitlesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetTaskTypesCriteriaValidator : AbstractValidator<GetTaskTypesCriteria>
    {
        public GetTaskTypesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetUsersCriteriaValidator : AbstractValidator<GetUsersCriteria>
    {
        public GetUsersCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetCountriesCriteriaValidator : AbstractValidator<GetCountriesCriteria>
    {
        public GetCountriesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetCurrenciesCriteriaValidator : AbstractValidator<GetCurrenciesCriteria>
    {
        public GetCurrenciesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetActionLogsCriteriaValidator : AbstractValidator<GetActionLogsCriteria>
    {
        public GetActionLogsCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetRoleCriteriaValidator : AbstractValidator<GetRolesCriteria>
    {
        public GetRoleCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetZoneCriteriaValidator : AbstractValidator<GetZoneCriteria>
    {
        public GetZoneCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetRequestsCriteriaValidator : AbstractValidator<GetRequestsCriteria>
    {
        public GetRequestsCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetRequestTasksCriteriaValidator : AbstractValidator<GetRequestTasksCriteria>
    {
        public GetRequestTasksCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetRequestAssignmentsCriteriaValidator : AbstractValidator<GetRequestAssignmentsCriteria>
    {
        public GetRequestAssignmentsCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetRequestVacationCriteriaValidator : AbstractValidator<GetRequestVacationsCriteria>
    {
        public GetRequestVacationCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetRequestJustificationCriteriaValidator : AbstractValidator<GetRequestJustificationCriteria>
    {
        public GetRequestJustificationCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class EmployeeGetRequestJustificationCriteriaValidator : AbstractValidator<EmployeeGetRequestJustificationCriteria>
    {
        public EmployeeGetRequestJustificationCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class EmployeeGetRequestAssignmentCriteriaValidator : AbstractValidator<Employee2GetRequestAssignmentsCriteria>
    {
        public EmployeeGetRequestAssignmentCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class EmployeeGetRequestPermissionCriteriaValidator : AbstractValidator<EmployeeGetRequestPermissionsCriteria>
    {
        public EmployeeGetRequestPermissionCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class EmployeeGetRequestVacationCriteriaValidator : AbstractValidator<EmployeeGetRequestVacationsCriteria>
    {
        public EmployeeGetRequestVacationCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class Employee2GetRequestTaskCriteriaValidator : AbstractValidator<Employee2GetRequestTasksCriteria>
    {
        public Employee2GetRequestTaskCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetEmployeeAttendancesForWebAdminCriteriaValidator : AbstractValidator<GetEmployeeAttendancesForWebAdminCriteria>
    {
        public GetEmployeeAttendancesForWebAdminCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetHolidayCriteriaValidator : AbstractValidator<GetHolidayCriteria>
    {
        public GetHolidayCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }


}
