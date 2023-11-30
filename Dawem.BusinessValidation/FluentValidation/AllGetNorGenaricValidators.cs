using Dawem.Models.Criteria.Core;
using Dawem.Models.Criteria.Lookups;
using Dawem.Models.Criteria.Others;
using Dawem.Models.Dtos.Employees.AssignmentType;
using Dawem.Models.Dtos.Employees.Department;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Employees.HolidayType;
using Dawem.Models.Dtos.Employees.JobTitle;
using Dawem.Models.Dtos.Employees.TaskType;
using Dawem.Models.Dtos.Employees.User;
using Dawem.Models.Dtos.Schedules.SchedulePlanBackgroundJobLogs;
using Dawem.Models.Dtos.Schedules.SchedulePlans;
using Dawem.Models.Dtos.Schedules.Schedules;
using Dawem.Models.Dtos.Schedules.ShiftWorkingTimes;
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

}
