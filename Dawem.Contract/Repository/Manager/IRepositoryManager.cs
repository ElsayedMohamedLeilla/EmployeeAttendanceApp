using Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups;
using Dawem.Contract.RealTime.Firebase;
using Dawem.Contract.Repository.Attendances;
using Dawem.Contract.Repository.Core;
using Dawem.Contract.Repository.Core.DefaultLookups;
using Dawem.Contract.Repository.Employees;
using Dawem.Contract.Repository.Localization;
using Dawem.Contract.Repository.Lookups;
using Dawem.Contract.Repository.MenuItems;
using Dawem.Contract.Repository.Others;
using Dawem.Contract.Repository.Permissions;
using Dawem.Contract.Repository.Provider;
using Dawem.Contract.Repository.Requests;
using Dawem.Contract.Repository.Schedules.SchedulePlans;
using Dawem.Contract.Repository.Schedules.Schedules;
using Dawem.Contract.Repository.Settings;
using Dawem.Contract.Repository.Subscriptions;
using Dawem.Contract.Repository.Summons;
using Dawem.Contract.Repository.UserManagement;

namespace Dawem.Contract.Repository.Manager
{
    public interface IRepositoryManager
    {
        ICountryRepository CountryRepository { get; }
        ILanguageRepository LanguageRepository { get; }
        ICurrencyRepository CurrencyRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        ICompanyBranchRepository CompanyBranchRepository { get; }
        ICompanyAttachmentRepository CompanyAttachmentRepository { get; }
        ICompanyIndustryRepository CompanyIndustryRepository { get; }
        ISubscriptionRepository SubscriptionRepository { get; }
        ISubscriptionLogRepository SubscriptionLogRepository { get; }
        ISubscriptionPaymentRepository SubscriptionPaymentRepository { get; }
        IPlanRepository PlanRepository { get; }
        IPlanNameTranslationRepository PlanNameTranslationRepository { get; }
        ISettingRepository SettingRepository { get; }
        IUserRepository UserRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IPermissionScreenRepository PermissionScreenRepository { get; }
        IPermissionScreenActionRepository PermissionScreenActionRepository { get; }
        IPermissionLogRepository PermissionLogRepository { get; }
        IUserBranchRepository UserBranchRepository { get; }
        IUserTokenRepository UserTokenRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        ITranslationRepository TranslationRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IJustificationsTypeRepository JustificationsTypeRepository { get; }
        IResponsibilityRepository ResponsibilityRepository { get; }
        IUserResponsibilityRepository UserResponsibilityRepository { get; }
        IVacationsTypeRepository VacationsTypeRepository { get; }
        IPermissionsTypeRepository PermissionsTypeRepository { get; }
        IOvertimeTypeRepository OvertimeTypeRepository { get; }
        IAssignmentTypeRepository AssignmentTypeRepository { get; }
        ITaskTypeRepository TaskTypeRepository { get; }
        IJobTitleRepository JobTitleRepository { get; }
        IHolidayTypeRepository HolidayTypeRepository { get; }
        IScheduleRepository ScheduleRepository { get; }
        ISchedulePlanRepository SchedulePlanRepository { get; }
        ISchedulePlanEmployeeRepository SchedulePlanEmployeeRepository { get; }
        ISchedulePlanGroupRepository SchedulePlanGroupRepository { get; }
        ISchedulePlanDepartmentRepository SchedulePlanDepartmentRepository { get; }
        ISchedulePlanBackgroundJobLogRepository SchedulePlanLogRepository { get; }
        ISchedulePlanBackgroundJobLogEmployeeRepository SchedulePlanLogEmployeeRepository { get; }
        IEmployeeAttendanceRepository EmployeeAttendanceRepository { get; }
        IEmployeeAttendanceCheckRepository EmployeeAttendanceCheckRepository { get; }
        IScheduleDayRepository ScheduleDayRepository { get; }
        IRoleRepository RoleRepository { get; }
        IShiftWorkingTimeRepository ShiftWorkingTimeRepository { get; }
        IGroupRepository GroupRepository { get; }
        IGroupEmployeeRepository GroupEmployeeRepository { get; }
        IGroupManagerDelegatorRepository GroupManagerDelegatorRepository { get; }
        IDepartmentManagerDelegatorRepository DepartmentManagerDelegatorRepository { get; }
        IZoneDepartmentRepository ZoneDepartmentRepository { get; }
        IZoneEmployeeRepository ZoneEmployeeRepository { get; }
        IZoneGroupRepository ZoneGroupRepository { get; }
        IZoneRepository ZoneRepository { get; }
        IFingerprintDeviceRepository FingerprintDeviceRepository { get; }
        IFingerprintTransactionRepository FingerprintTransactionRepository { get; }
        IFingerprintDeviceLogRepository FingerprintDeviceLogRepository { get; }
        IRequestRepository RequestRepository { get; }
        IRequestAssignmentRepository RequestAssignmentRepository { get; }
        IRequestAttachmentRepository RequestAttachmentRepository { get; }
        IRequestJustificationRepository RequestJustificationRepository { get; }
        IRequestPermissionRepository RequestPermissionRepository { get; }
        IRequestTaskRepository RequestTaskRepository { get; }
        IRequestTaskEmployeeRepository RequestTaskEmployeeRepository { get; }
        IRequestVacationRepository RequestVacationRepository { get; }
        IRequestOvertimeRepository RequestOvertimeRepository { get; }
        IHolidayRepository HolidayRepository { get; }
        IVacationBalanceRepository VacationBalanceRepository { get; }
        INotificationRepository NotificationRepository { get; }
        ISummonRepository SummonRepository { get; }
        ISummonLogSanctionRepository SummonLogSanctionRepository { get; }
        ISummonLogRepository SummonLogRepository { get; }
        ISummonNotifyWayRepository SummonNotifyWayRepository { get; }
        ISummonEmployeeRepository SummonEmployeeRepository { get; }
        ISummonDepartmentRepository SummonDepartmentRepository { get; }
        ISummonGroupRepository SummonGroupRepository { get; }
        ISummonSanctionRepository SummonSanctionRepository { get; }
        ISanctionRepository SanctionRepository { get; }
        INotificationUserRepository NotificationUserRepository { get; }
        INotificationUserFCMTokenRepository NotificationUserFCMTokenRepository { get; }
        IEmployeeOTPRepository EmployeeOTPRepository { get; }
        IMenuItemRepository MenuItemRepository { get; }
        IOldScreenRepository OldScreenRepository { get; }
        IMenuItemActionRepository MenuItemActionRepository { get; }
        IMenuItemNameTranslationRepository MenuItemNameTranslationRepository { get; }
        IPlanScreenRepository PlanScreenRepository { get; }
        IDefaultLookupsNameTranslationRepository DefaultLookupsNameTranslationRepository { get; }

        IDefaultVacationTypeRepository DefaultVacationTypeRepository { get; }
        IDefaultShiftTypeRepository DefaultShiftTypeRepository { get; }
        IDefaultJustificationTypeRepository DefaultJustificationTypeRepository { get; }

        IDefaultTaskTypeRepository DefaultTaskTypeRepository { get; }

        IDefaultPermissionTypeRepository DefaultPermissionTypeRepository { get; }

        IDefaultOfficialHolidayRepository DefaultOfficialHolidayRepository { get; }

        IDefaultDepartmentsRepository DefaultDepartmentsRepository { get; }

        IDefaultJobTitlesRepository DefaultJobTitlesRepository { get; }

        IDefaultPenaltiesRepository DefaultPenaltiesRepository { get; }



    }
}