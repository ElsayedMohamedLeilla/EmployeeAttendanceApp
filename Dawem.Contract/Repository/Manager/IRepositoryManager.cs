using Dawem.Contract.RealTime.Firebase;
using Dawem.Contract.Repository.Attendances;
using Dawem.Contract.Repository.Core;
using Dawem.Contract.Repository.Employees;
using Dawem.Contract.Repository.Localization;
using Dawem.Contract.Repository.Lookups;
using Dawem.Contract.Repository.Others;
using Dawem.Contract.Repository.Permissions;
using Dawem.Contract.Repository.Provider;
using Dawem.Contract.Repository.Requests;
using Dawem.Contract.Repository.Schedules.SchedulePlans;
using Dawem.Contract.Repository.Schedules.Schedules;
using Dawem.Contract.Repository.Summons;
using Dawem.Contract.Repository.UserManagement;

namespace Dawem.Contract.Repository.Manager
{
    public interface IRepositoryManager
    {
        ICompanyRepository CompanyRepository { get; }
        ISubscriptionRepository SubscriptionRepository { get; }
        ISubscriptionLogRepository SubscriptionLogRepository { get; }
        IPlanRepository PlanRepository { get; }
        IDawemSettingRepository DawemSettingRepository { get; }
        ICountryRepository CountryRepository { get; }
        IUserRepository UserRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IPermissionScreenRepository PermissionScreenRepository { get; }
        IPermissionScreenActionRepository PermissionScreenActionRepository { get; }
        IPermissionLogRepository PermissionLogRepository { get; }
        IUserBranchRepository UserBranchRepository { get; }
        IBranchRepository BranchRepository { get; }
        IUserTokenRepository UserTokenRepository { get; }
        IScreenRepository ScreenRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        ITranslationRepository TranslationRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IJustificationsTypeRepository JustificationsTypeRepository { get; }
        IVacationsTypeRepository VacationsTypeRepository { get; }
        IPermissionsTypeRepository PermissionsTypeRepository { get; }
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
        IRequestRepository RequestRepository { get; }
        IRequestAssignmentRepository RequestAssignmentRepository { get; }
        IRequestAttachmentRepository RequestAttachmentRepository { get; }
        IRequestJustificationRepository RequestJustificationRepository { get; }
        IRequestPermissionRepository RequestPermissionRepository { get; }
        IRequestTaskRepository RequestTaskRepository { get; }
        IRequestTaskEmployeeRepository RequestTaskEmployeeRepository { get; }
        IRequestVacationRepository RequestVacationRepository { get; }
        IHolidayRepository HolidayRepository { get; }
        IVacationBalanceRepository VacationBalanceRepository { get; }
        INotificationStoreRepository NotificationStoreRepository { get; }
        ISummonRepository SummonRepository { get; }
        ISummonMissingLogRepository SummonMissingLogRepository { get; }
        ISummonNotifyWayRepository SummonNotifyWayRepository { get; }
        ISummonEmployeeRepository SummonEmployeeRepository { get; }
        ISummonDepartmentRepository SummonDepartmentRepository { get; }
        ISummonGroupRepository SummonGroupRepository { get; }
        ISummonSanctionRepository SummonSanctionRepository { get; }
        ISanctionRepository SanctionRepository { get; }
        INotificationUserRepository NotificationUserRepository { get; }
        INotificationUserDeviceTokenRepository NotificationUserDeviceTokenRepository { get; }
    }
}