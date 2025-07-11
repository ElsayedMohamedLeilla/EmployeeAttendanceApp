﻿using Dawem.Models.Criteria.Core;
using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Criteria.Lookups;
using Dawem.Models.Criteria.Providers;
using Dawem.Models.Dtos.Dawem.Attendances;
using Dawem.Models.Dtos.Dawem.Dashboard;
using Dawem.Models.Dtos.Dawem.Employees.AssignmentTypes;
using Dawem.Models.Dtos.Dawem.Employees.Departments;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Employees.HolidayTypes;
using Dawem.Models.Dtos.Dawem.Employees.JobTitles;
using Dawem.Models.Dtos.Dawem.Employees.TaskTypes;
using Dawem.Models.Dtos.Dawem.Employees.Users;
using Dawem.Models.Dtos.Dawem.Permissions.PermissionLogs;
using Dawem.Models.Dtos.Dawem.Permissions.Permissions;
using Dawem.Models.Dtos.Dawem.Providers.Companies;
using Dawem.Models.Dtos.Dawem.Reports.AttendanceSummaryReport;
using Dawem.Models.Dtos.Dawem.Schedules.SchedulePlanBackgroundJobLogs;
using Dawem.Models.Dtos.Dawem.Schedules.SchedulePlans;
using Dawem.Models.Dtos.Dawem.Schedules.Schedules;
using Dawem.Models.Dtos.Dawem.Schedules.ShiftWorkingTimes;
using Dawem.Models.Dtos.Dawem.Subscriptions;
using Dawem.Models.Dtos.Dawem.Subscriptions.Plans;
using Dawem.Models.Dtos.Dawem.Subscriptions.SubscriptionPayment;
using Dawem.Models.Dtos.Dawem.Summons.Sanctions;
using Dawem.Models.Dtos.Dawem.Summons.Summons;
using Dawem.Models.DTOs.Dawem.Screens.ScreenGroups;
using Dawem.Models.DTOs.Dawem.Screens.Screens;
using Dawem.Models.Requests;
using Dawem.Models.Requests.Assignments;
using Dawem.Models.Requests.Justifications;
using Dawem.Models.Requests.Permissions;
using Dawem.Models.Requests.Tasks;
using Dawem.Models.Requests.Vacations;
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
    public class GetSanctionsCriteriaValidator : AbstractValidator<GetSanctionsCriteria>
    {
        public GetSanctionsCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetSummonsCriteriaValidator : AbstractValidator<GetSummonsCriteria>
    {
        public GetSummonsCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetSummonLogsCriteriaValidator : AbstractValidator<GetSummonLogsCriteria>
    {
        public GetSummonLogsCriteriaValidator()
        {
            Include(new GetGenaricValidator());
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
    public class GetSchedulePlanBackgroundJobLogsCriteriaValidator : AbstractValidator<GetSchedulePlanLogCriteria>
    {
        public GetSchedulePlanBackgroundJobLogsCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetSchedulePlanLogEmployeesCriteriaValidator : AbstractValidator<GetSchedulePlanLogEmployeesCriteria>
    {
        public GetSchedulePlanLogEmployeesCriteriaValidator()
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
    public class GetLanguagesCriteriaValidator : AbstractValidator<GetLanguagesCriteria>
    {
        public GetLanguagesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetPermissionsCriteriaValidator : AbstractValidator<GetPermissionsCriteria>
    {
        public GetPermissionsCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetScreenPermissionLogsCriteriaValidator : AbstractValidator<GetPermissionLogsCriteria>
    {
        public GetScreenPermissionLogsCriteriaValidator()
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
    public class GetNotificationSCriteriaValidator : AbstractValidator<GetNotificationCriteria>
    {
        public GetNotificationSCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class AttendanceSummaryCritriaValidator : AbstractValidator<AttendanceSummaryCritria>
    {
        public AttendanceSummaryCritriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetCompaniesCriteriaValidator : AbstractValidator<GetCompaniesCriteria>
    {
        public GetCompaniesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetCompanyBranchesCriteriaValidator : AbstractValidator<GetCompanyBranchesCriteria>
    {
        public GetCompanyBranchesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetSubscriptionsCriteriaValidator : AbstractValidator<GetSubscriptionsCriteria>
    {
        public GetSubscriptionsCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetSubscriptionPaymentsCriteriaValidator : AbstractValidator<GetSubscriptionPaymentsCriteria>
    {
        public GetSubscriptionPaymentsCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetScreensCriteriaValidator : AbstractValidator<GetScreensCriteria>
    {
        public GetScreensCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetPlansCriteriaValidator : AbstractValidator<GetPlansCriteria>
    {
        public GetPlansCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }

    public class GetDefaultVacationsTypesCriteriaValidator : AbstractValidator<GetDefaultVacationTypeCriteria>
    {
        public GetDefaultVacationsTypesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetDefaultShiftsTypesCriteriaValidator : AbstractValidator<GetDefaultShiftTypeCriteria>
    {
        public GetDefaultShiftsTypesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }

    public class GetDefaultJustificationsTypesCriteriaValidator : AbstractValidator<GetDefaultJustificationTypeCriteria>
    {
        public GetDefaultJustificationsTypesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetDefaultOfficialHolidaysCriteriaValidator : AbstractValidator<GetDefaultOfficialHolidayCriteria>
    {
        public GetDefaultOfficialHolidaysCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetDefaultDepartmentsCriteriaValidator : AbstractValidator<GetDefaultDepartmentsCriteria>
    {
        public GetDefaultDepartmentsCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetDefaultJobTitlesCriteriaValidator : AbstractValidator<GetDefaultJobTitlesCriteria>
    {
        public GetDefaultJobTitlesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
    public class GetDefaultPenaltiesCriteriaValidator : AbstractValidator<GetDefaultPenaltiesCriteria>
    {
        public GetDefaultPenaltiesCriteriaValidator()
        {
            Include(new GetGenaricValidator());
        }
    }
}
